using System.Windows.Media;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TempoControls.Controls;
using TempoControls.Core.Types;

namespace TempoControls.SyntaxSchemes
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

        public void Highlight(ColoredLabel label)
        {
            label.UpdateIndex(new IntRange(0, label.Characters.Count), Default, new Typeface("Verdana"));
            
            var tree = CSharpSyntaxTree.ParseText(label.Text);
            
            new KeywordColor(label).Visit(tree.GetRoot());
        } // TODO: Use EnvironmentCache compilations

        private class KeywordColor : CSharpSyntaxWalker
        {
            private readonly ColoredLabel label;
            private readonly IProgrammingLanguageSyntaxScheme scheme;
            private FormattedText formatted;

            public KeywordColor(ColoredLabel label) : base(SyntaxWalkerDepth.Token)
            {
                this.label = label;
                scheme = (IProgrammingLanguageSyntaxScheme)label.Scheme;
            }
// TODO: This, then move onto inspections
            /*public override void VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                var nameSpan = new IntRange(node.Identifier.SpanStart, node.Identifier.Span.End);
                
                label.UpdateIndex(nameSpan, scheme.Type, new Typeface("Verdana"));
            }

            public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
            {
                var nameSpan = new IntRange(node.Identifier.SpanStart, node.Identifier.Span.End);
                
                label.UpdateIndex(nameSpan, scheme.Method, new Typeface("Verdana"));
            }
            
            public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
            {
                var nameSpan = new IntRange(node.SpanStart, node.Span.End);

                label.UpdateIndex(nameSpan, scheme.Member, new Typeface("Verdana"));
            }*/

            public override void VisitToken(SyntaxToken token)
            {
                if (token.IsKeyword())
                {
                    var keywordSpan = new IntRange(token.SpanStart, token.Span.End);

                    label.UpdateIndex(keywordSpan, scheme.Identifier, new Typeface("Verdana"));
                }
            }
        }
    }
}