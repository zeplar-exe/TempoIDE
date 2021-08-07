using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
            var text = textBox.Text;

            var tree = CSharpSyntaxTree.ParseText(text);
            var root = tree.GetCompilationUnitRoot();
            
            new KeywordColor(textBox).Visit(tree.GetRoot());
            
            return;

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
            }
        }

        public AutoCompletion[] GetAutoCompletions(SyntaxTextBox textBox)
        {
            var xmlData = ResourceCache.IntellisenseCs;
            var keywords = xmlData.Root.Element("keywords").Elements("kw");

            var typingWord = textBox.GetTypingWord(true);
                 
            if (string.IsNullOrWhiteSpace(typingWord))
                return null;
            
            return keywords
                .Select(kw => kw.Value)
                .Where(kw => kw.StartsWith(typingWord) && kw != typingWord)
                .Select(kw => new AutoCompletion(kw))
                .ToArray();
        }

        private class KeywordColor : CSharpSyntaxWalker
        {
            private readonly ColoredLabel label;

            public KeywordColor(ColoredLabel label)
            {
                this.label = label;
            }

            public override void VisitToken(SyntaxToken token)
            {
                if (token.IsContextualKeyword())
                {
                    var keywordSpan = new IntRange(token.SpanStart, token.Span.Length);

                    label.UpdateIndex(keywordSpan, Brushes.Blue, new Typeface("Verdana"));
                }
            }
        }
    }
}