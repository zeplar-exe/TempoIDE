using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.MSBuild;
using TempoIDE.Classes.Types;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.SyntaxSchemes
{
    public class CsSyntaxScheme : IProgrammingLanguageSyntaxScheme
    {
        public Brush Default => Brushes.White;
        public Brush String => Brushes.SeaGreen;
        public Brush Number => Brushes.LightCoral;
        public Brush Comment => Brushes.ForestGreen;
        public Brush Identifier => Brushes.CornflowerBlue;
        public Brush Type => Brushes.MediumPurple;
        public Brush Method => Brushes.LightGreen;
        public Brush Member => Brushes.CadetBlue;

        public void Highlight(ColoredLabel textBox)
        {
            int? readStartIndex = null;
            var word = "";

            var xmlData = ResourceCache.IntellisenseCs;
            var keywords = xmlData.Root.Element("keywords").Elements("kw");

            var text = textBox.Text;
            
            var tree = CSharpSyntaxTree.ParseText(text);
            var root = tree.GetCompilationUnitRoot();

            foreach (var usingDir in root.Usings)
            {
                var keywordSpan = new IntRange(usingDir.UsingKeyword.SpanStart, usingDir.UsingKeyword.Span.Length);

                textBox.UpdateIndex(keywordSpan, Identifier, new Typeface("Verdana"));

                var index = keywordSpan.End + 1;
                foreach (var name in usingDir.Name.ChildNodes())
                {
                    var nameText = name.GetText();
                    
                    textBox.UpdateIndex(new IntRange(index, index + nameText.Length), Member, new Typeface("Verdana"));

                    index += nameText.Length + 1;
                }
            } // TODO: Just highlight everything for now
            
            return;
            
            for (var index = 0; index < text.Length; index++)
            {
                var character = text[index];

                textBox.UpdateIndex(index, Default, new Typeface("Verdana"));

                if (char.IsNumber(character))
                {
                    if (readStartIndex != null)
                        word += character;
                    else
                        textBox.UpdateIndex(index, Number, new Typeface("Verdana"));
                }
                else if (char.IsLetter(character))
                {
                    if (readStartIndex == null)
                        readStartIndex = index;

                    word += character;
                }
                else
                {
                    if (readStartIndex == null)
                        continue;

                    if (keywords.Any(kw => word == kw.Value))
                        textBox.UpdateIndex(new IntRange(
                                readStartIndex.ToRealValue(),
                                readStartIndex.ToRealValue() + word.Length),
                            Identifier, new Typeface("Verdana"));
                    
                    word = "";
                    readStartIndex = null;
                }
            }

            if (readStartIndex == null)
                return;

            if (keywords.Any(kw => word == kw.Value))
            {
                textBox.UpdateIndex(new IntRange(
                        readStartIndex.ToRealValue(), 
                        readStartIndex.ToRealValue() + word.Length), 
                    Identifier, new Typeface("Verdana"));
            }
        }

        public AutoCompletion[] GetAutoCompletions(SyntaxTextBox textBox)
        {
            var xmlData = ResourceCache.IntellisenseCs;
            var keywords = xmlData.Root.Element("keywords").Elements("kw");
            
            var tree = CSharpSyntaxTree.ParseText(textBox.TextArea.Text);
            var root = tree.GetCompilationUnitRoot();

            // TODO: AutoComplete context

            var typingWord = textBox.GetTypingWord(true);
                 
            if (string.IsNullOrWhiteSpace(typingWord))
                return null;
            
            return keywords
                .Select(kw => kw.Value)
                .Where(kw => kw.StartsWith(typingWord) && kw != typingWord)
                .Select(kw => new AutoCompletion(kw))
                .ToArray();
        }
    }
}