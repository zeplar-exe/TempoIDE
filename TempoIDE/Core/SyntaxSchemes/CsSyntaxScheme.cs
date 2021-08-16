using System.Windows.Media;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TempoControls;

namespace TempoIDE.Core.SyntaxSchemes
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

        public void Highlight(ColoredLabel label, FormattedText processedText)
        {
            var tree = CSharpSyntaxTree.ParseText(label.Text); // TODO: Handle stuff here

            new KeywordColor(this, processedText).Visit(tree.GetRoot());
        }

        private class KeywordColor : CSharpSyntaxWalker
        {
            private readonly FormattedText processedText;
            private readonly IProgrammingLanguageSyntaxScheme scheme;

            public KeywordColor(IProgrammingLanguageSyntaxScheme scheme, FormattedText processedText) : base(SyntaxWalkerDepth.Token)
            {
                this.processedText = processedText;
                this.scheme = scheme;
            } // TODO: Highlighting, THEN inspections

            public override void VisitToken(SyntaxToken token)
            {
                if (token.IsKeyword())
                {
                    processedText.SetForegroundBrush(
                        scheme.Identifier, 
                        token.SpanStart, 
                        token.Span.Length);
                }
            }
        }
    }
}