using System.Text.RegularExpressions;

namespace TempoCompiler
{
    public static class Lexer
    {
        public readonly struct LexerTokenDefinition
        {
            public readonly LexerTokenType Type;
            public readonly Regex Regex;
            public readonly int Precedence;

            public LexerTokenDefinition(LexerTokenType type, string regex, int precedence)
            {
                Type = type;
                Regex = new Regex(regex);
                Precedence = precedence;
            }

            public LexerTokenDefinition(LexerTokenType type, Regex regex, int precedence)
            {
                Type = type;
                Regex = regex;
                Precedence = precedence;
            }
        }

        public readonly struct LexerToken
        {

        }
    }
}