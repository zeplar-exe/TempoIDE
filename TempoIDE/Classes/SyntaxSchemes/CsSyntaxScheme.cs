using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
            textBox.UpdateIndex(new IntRange(0, textBox.Characters.Count), Default, new Typeface("Verdana"));
            
            var text = textBox.Text;
            var tree = CSharpSyntaxTree.ParseText(text);

            new KeywordColor(textBox).Visit(tree.GetRoot());
        }

        public AutoCompletion[] GetAutoCompletions(SyntaxTextBox textBox)
        {
            var xmlData = ResourceCache.IntellisenseCs;
            var keywords = xmlData.Root.Element("keywords").Elements("kw");

            var typingWord = textBox.GetTypingWord(true);
                 
            if (string.IsNullOrWhiteSpace(typingWord))
                return null;
            
            return keywords
                .Where(kw => kw.Value.StartsWith(typingWord) && kw.Value != typingWord)
                .Select(kw => new AutoCompletion(kw.Value))
                .ToArray();
        }

        private class KeywordColor : CSharpSyntaxWalker
        {
            private readonly ColoredLabel label;
            private readonly IProgrammingLanguageSyntaxScheme scheme;

            public KeywordColor(ColoredLabel label) : base(SyntaxWalkerDepth.Token)
            {
                this.label = label;
                scheme = (IProgrammingLanguageSyntaxScheme)label.Scheme;
            }

            public override void VisitClassDeclaration(ClassDeclarationSyntax node)
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
            }

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