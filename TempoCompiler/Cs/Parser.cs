using System;
using System.Collections.Generic;
using TempoCompiler.Cs.Syntax;

namespace TempoCompiler.Cs
{
    public class Parser
    {
        private SyntaxElement root;
        private SyntaxElement focusedElement;
        
        public Parser(string input)
        {
            var lexer = new Lexer(input);

            root = new BlockSyntax();
            focusedElement = root;

            foreach (var token in lexer)
            {
                if (token.Type == Token.If)
                {
                    InitMoveFocus(new IfStatementSyntax());
                }
                else if (token.Type == Token.Else)
                {
                    InitMoveFocus(new ElseStatementSyntax());
                }
                else if (token.Type == Token.BlockOpen)
                {
                    InitMoveFocus(new BlockSyntax());
                }
                else if (token.Type == Token.BlockClose)
                {
                    TraverseBackward();
                }
                else if (token.Type == Token.Semicolon)
                {
                    TraverseBackward();
                }
                else
                {
                    focusedElement.Children.Add(new UnknownSyntax());
                }
            }
            
            Console.WriteLine(string.Join("\n", root));
        }

        private void InitMoveFocus(SyntaxElement element)
        {
            focusedElement.Children.Add(element);
            element.Parent = focusedElement;

            focusedElement = element;
        }

        private void TraverseBackward()
        {
            focusedElement = focusedElement.Parent;
        }
    }
}