using System.Windows.Media;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TempoControls.Core.Types;
using TempoControls.Core.Types.Collections;

namespace TempoControls.Core.SyntaxSchemes
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

        public void Highlight(ColoredLabel label, SyntaxCharCollection characters)
        {
            var tree = CSharpSyntaxTree.ParseText(label.Text);

            new KeywordColor(this, characters).Visit(tree.GetRoot());
        }

        private class KeywordColor : CSharpSyntaxWalker
        {
            private readonly SyntaxCharCollection text;
            private readonly IProgrammingLanguageSyntaxScheme scheme;

            public KeywordColor(IProgrammingLanguageSyntaxScheme scheme, SyntaxCharCollection text) : base(SyntaxWalkerDepth.Token)
            {
                this.text = text;
                this.scheme = scheme;
            }

            public override void VisitToken(SyntaxToken token)
            {
                if (token.IsKeyword())
                {
                    text.UpdateRangeForeground(
                        new IntRange(token.SpanStart, token.Span.End),
                        scheme.Identifier);
                }
            }

            public override void VisitLiteralExpression(LiteralExpressionSyntax node)
            {
                if (node.IsKind(SyntaxKind.StringLiteralExpression))
                {
                    text.UpdateRangeForeground(
                        new IntRange(node.SpanStart, node.Span.End),
                        scheme.String);
                }
                else if (node.IsKind(SyntaxKind.NumericLiteralExpression))
                {
                    text.UpdateRangeForeground(
                        new IntRange(node.SpanStart, node.Span.End),
                        scheme.Number);
                }
            }
        }
    }
}