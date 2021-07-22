using System.Collections.Generic;

namespace TempoCompiler.Cs
{
    public class Parser
    {
        private static Token[] ignoredTokens =
        {
            Token.Whitespace,
            Token.Comment,
            Token.XmlComment,
            Token.Newline,
        };
        
        public Parser(string input)
        {
            var lexer = new Lexer(input);
        }
    }
}