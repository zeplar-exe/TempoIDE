using Jammo.CsAnalysis;
using Jammo.CsAnalysis.CodeInspection;
using Jammo.CsAnalysis.CodeInspection.Rules;
using Jammo.CsAnalysis.Compilation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NetSpell.SpellChecker;
using NetSpell.SpellChecker.Dictionary;

namespace TempoIDE.Core.Inspections.TempoRules
{
    public class MisspelledWordInspection : InspectionRule
    {
        public override InspectionInfo GetInspectionInfo()
        {
            return new InspectionInfo(
                "TEMPO_0001",
                "MisspelledWordInspection",
                "Found a typo.");
        }

        public override void TestStringLiteral(LiteralExpressionSyntax syntax, CompilationWrapper context)
        {
            SpellCheck(syntax.ToString(), context);
        }

        public override void TestSingleLineComment(SyntaxTrivia syntax, CompilationWrapper context)
        {
            SpellCheck(syntax.ToString(), context);
        }

        public override void TestMultiLineComment(SyntaxTrivia syntax, CompilationWrapper context)
        {
            SpellCheck(syntax.ToString(), context);
        }

        private void SpellCheck(string text, CompilationWrapper context)
        {
            var spelling = new Spelling
            {
                Text = text,
                WordIndex = 0,
                SuggestionMode = Spelling.SuggestionEnum.NearMiss,
                Dictionary = new WordDictionary { DictionaryFolder = "data/dictionaries" }
            };

            spelling.MisspelledWord += delegate(object _, SpellingEventArgs e)
            {
                context.CreateInspection(new Inspection(
                    e.Word, 
                    new IndexSpan(e.TextIndex, e.TextIndex + e.Word.Length), 
                    this));
            };
            
            spelling.SpellCheck();
        }
    }
}