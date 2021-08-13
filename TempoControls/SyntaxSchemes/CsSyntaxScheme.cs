using System;
using System.Linq;
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

        public void Highlight(ColoredLabel label, HighlightInfo info)
        {
            var tree = CSharpSyntaxTree.ParseText(
                string.Concat(
                    label.Characters
                        .Skip(info.Range.Start)
                        .Take(info.Range.Size)
                        .Select(c => c.Value)
                    )
                );
            
            new KeywordColor(label, info).Visit(tree.GetRoot());
        }

        private class KeywordColor : CSharpSyntaxWalker
        {
            private readonly ColoredLabel label;
            private readonly HighlightInfo info;
            private readonly IProgrammingLanguageSyntaxScheme scheme;

            public KeywordColor(ColoredLabel label, HighlightInfo info) : base(SyntaxWalkerDepth.Token)
            {
                this.label = label;
                this.info = info;
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
                    info.ProcessedText.SetForegroundBrush(
                        scheme.Identifier, 
                        info.Range.Start + token.SpanStart, 
                        token.Span.Length);
                }
            }
        }
    }
}