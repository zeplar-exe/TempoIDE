using System.Linq;
using System.Windows.Media;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TempoControls;

namespace TempoIDE.SyntaxSchemes
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

            new KeywordColor(label, processedText).Visit(tree.GetRoot());
        }

        private class KeywordColor : CSharpSyntaxWalker
        {
            private readonly ColoredLabel label;
            private readonly FormattedText processedText;
            private readonly IProgrammingLanguageSyntaxScheme scheme;

            public KeywordColor(ColoredLabel label, FormattedText processedText) : base(SyntaxWalkerDepth.Token)
            {
                this.label = label;
                this.processedText = processedText;
                scheme = (IProgrammingLanguageSyntaxScheme)label.Scheme;
            }

            public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
            {
                processedText.SetForegroundBrush(
                    scheme.Member, 
                    node.Name.SpanStart, 
                    node.Name.Span.Length);
            }

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