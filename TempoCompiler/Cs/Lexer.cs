using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TempoCompiler.Cs
{
    internal class Lexer : IEnumerable<LexerToken>
    {
        private static TokenDef[] tokens =
        {
            new TokenDef("abstract", Token.Abstract, 5),
            new TokenDef("as", Token.As, 5),
            new TokenDef("async", Token.Async, 5),
            new TokenDef("base", Token.Base, 5),
            new TokenDef("bool", Token.Bool, 5),
            new TokenDef("break", Token.Break, 5),
            new TokenDef("byte", Token.Byte, 5),
            new TokenDef("case", Token.Case, 5),
            new TokenDef("catch", Token.Catch, 5),
            new TokenDef("char", Token.Char, 5),
            new TokenDef("checked", Token.Checked, 5),
            new TokenDef("class", Token.Class, 5),
            new TokenDef("const", Token.Const, 5),
            new TokenDef("continue", Token.Continue, 5),
            new TokenDef("decimal", Token.Decimal, 5),
            new TokenDef("default", Token.Default, 5),
            new TokenDef("delegate", Token.Delegate, 5),
            new TokenDef("do", Token.Do, 5),
            new TokenDef("double", Token.Double, 5),
            new TokenDef("else", Token.Else, 5),
            new TokenDef("enum", Token.Enum, 5),
            new TokenDef("event", Token.Event, 5),
            new TokenDef("explicit", Token.Explicit, 5),
            new TokenDef("extern", Token.Extern, 5),
            new TokenDef("false", Token.False, 5),
            new TokenDef("finally", Token.Finally, 5),
            new TokenDef("fixed", Token.Fixed, 5),
            new TokenDef("float", Token.Float, 5),
            new TokenDef("for", Token.For, 5),
            new TokenDef("foreach", Token.Foreach, 5),
            new TokenDef("goto", Token.Goto, 5),
            new TokenDef("if", Token.If, 5),
            new TokenDef("implicit", Token.Implicit, 5),
            new TokenDef("in", Token.In, 5),
            new TokenDef("int", Token.Int, 5),
            new TokenDef("interface", Token.Interface, 5),
            new TokenDef("internal", Token.Internal, 5),
            new TokenDef("is", Token.Is, 5),
            new TokenDef("lock", Token.Lock, 5),
            new TokenDef("long", Token.Long, 5),
            new TokenDef("namespace", Token.Namespace, 5),
            new TokenDef("new", Token.New, 5),
            new TokenDef("null", Token.Null, 5),
            new TokenDef("object", Token.Object, 5),
            new TokenDef("operator", Token.Operator, 5),
            new TokenDef("out", Token.Out, 5),
            new TokenDef("override", Token.Override, 5),
            new TokenDef("params", Token.Params, 5),
            new TokenDef("private", Token.Private, 5),
            new TokenDef("protected", Token.Protected, 5),
            new TokenDef("public", Token.Public, 5),
            new TokenDef("readonly", Token.Readonly, 5),
            new TokenDef("ref", Token.Ref, 5),
            new TokenDef("return", Token.Return, 5),
            new TokenDef("sbyte", Token.Sbyte, 5),
            new TokenDef("sealed", Token.Sealed, 5),
            new TokenDef("short", Token.Short, 5),
            new TokenDef("sizeof", Token.Sizeof, 5),
            new TokenDef("stackalloc", Token.Stackalloc, 5),
            new TokenDef("static", Token.Static, 5),
            new TokenDef("string", Token.String, 5),
            new TokenDef("struct", Token.Struct, 5),
            new TokenDef("switch", Token.Switch, 5),
            new TokenDef("this", Token.This, 5),
            new TokenDef("throw", Token.Throw, 5),
            new TokenDef("true", Token.True, 5),
            new TokenDef("try", Token.Try, 5),
            new TokenDef("typeof", Token.Typeof, 5),
            new TokenDef("unit", Token.Uint, 5),
            new TokenDef("ulong", Token.Ulong, 5),
            new TokenDef("unchecked", Token.Unchecked, 5),
            new TokenDef("unsafe", Token.Unsafe, 5),
            new TokenDef("ushort", Token.Ushort, 5),
            new TokenDef("using", Token.Using, 5),
            new TokenDef("virtual", Token.Virtual, 5),
            new TokenDef("void", Token.Void, 5),
            new TokenDef("volatile", Token.Volatile, 5),
            new TokenDef("while", Token.While, 5),
            new TokenDef("where", Token.Where, 5),
            
            new TokenDef(@"\=", Token.Assignment, 5),
            new TokenDef("==", Token.Equals, 5),
            new TokenDef("!=", Token.DoesNotEqual, 5),
            new TokenDef(@"\!", Token.Not, 5),
            
            new TokenDef(@"\+", Token.Plus, 5),
            new TokenDef(@"\-", Token.Minus, 5),
            new TokenDef(@"\*", Token.Multiply, 5),
            new TokenDef(@"\/", Token.Divide, 5),
            new TokenDef(@"\%", Token.Modulus, 5),
            
            new TokenDef(@"\>", Token.GreaterThan, 5),
            new TokenDef(@"\<", Token.LessThan, 5),
            new TokenDef(@"\>=", Token.GreaterThanOrEqual, 5),
            new TokenDef(@"\<=", Token.LessThanOrEqual, 5),
            
            new TokenDef(@"\??", Token.NullCoalesce, 5),
            new TokenDef(@"\??=", Token.NullCoalesceAssignment, 5),
            new TokenDef(@"\?:", Token.Ternary, 5),
            
            new TokenDef(@"\@", Token.Verbatim, 5),
            new TokenDef(@"\$", Token.Formatted, 5),
            
            new TokenDef(@"\|\|", Token.Or, 5),
            new TokenDef(@"\&\&", Token.And, 5),
            new TokenDef(@"\|", Token.HorizontalLine, 5),
            new TokenDef(@"\&", Token.Ampersand, 5),
            new TokenDef(@"`", Token.Slave, 5),
            new TokenDef(@"~", Token.Tilde, 5),
            new TokenDef(@"\\", Token.Backslash, 5),

            new TokenDef("[A-Za-z1-9_]+", Token.Identifier, 6),
            new TokenDef(@"\d+", Token.Number, 5),

            new TokenDef(@"\.", Token.Period, 5),
            new TokenDef(",", Token.Comma, 5),
            new TokenDef(":", Token.Colon, 5),
            new TokenDef(";", Token.Semicolon, 5),
            new TokenDef("\"", Token.DoubleQuote, 5),
            new TokenDef("\'", Token.SingleQuote, 5),
            
            new TokenDef(@"\(", Token.LeftParen, 5),
            new TokenDef(@"\)", Token.RightParen, 5),
            new TokenDef(@"\[", Token.LeftBracket, 5),
            new TokenDef(@"\]", Token.RightBracket, 5),
            new TokenDef(@"\{", Token.BlockOpen, 5),
            new TokenDef(@"\}", Token.BlockClose, 5),
            
            new TokenDef(@"\s", Token.Whitespace, 5),
            new TokenDef(@"\/\/.*|\/\*.*\*\/", Token.Comment, 5),
            new TokenDef(@"\/\/\/.*", Token.XmlComment, 5),
            new TokenDef("\n", Token.Newline, 5),
        };

        private readonly string text;
        private int readPosition;

        public Lexer(string input)
        {
            text = input;
        }

        public IEnumerator<LexerToken> GetEnumerator()
        {
            Array.Sort(tokens);

            List<LexerToken> matches = new List<LexerToken>(100);
            
            while (readPosition < text.Length)
            {
                foreach (var token in tokens)
                {
                    var match = token.Pattern.Match(text, readPosition);

                    if (match.Success)
                    {
                        if (match.Index == readPosition)
                        {
                            matches.Add(new LexerToken(match.Value, token.Type, match.Index));
                        }
                    }
                }

                var longest = matches.OrderByDescending(m => m.Value.Length).FirstOrDefault();

                if (longest == null)
                    yield break;

                yield return longest; // TODO: Errors

                readPosition += longest.Value.Length;

                matches.Clear();
            }
            
            yield return new LexerToken("", Token.EndOfFile, text.Length - 1);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class LexerToken
    {
        public readonly string Value;
        public readonly Token Type;
        public readonly int StartIndex;

        public LexerToken(string value, Token type, int startIndex)
        {
            Value = value;
            Type = type;
            StartIndex = startIndex;
        }
    }
}