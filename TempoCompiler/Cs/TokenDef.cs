using System;
using System.Text.RegularExpressions;

namespace TempoCompiler.Cs
{
    public class TokenDef : IComparable<TokenDef>
    {
        public readonly Regex Pattern;
        public readonly Token Type;
        public readonly int Precedence;

        public TokenDef(string pattern, Token type, int precedence)
        {
            Pattern = new Regex(pattern);
            Type = type;
            Precedence = precedence;
        }

        public int CompareTo(TokenDef other)
        {
            return other == null ? 1 : Precedence.CompareTo(other.Precedence);
        }
    }
}