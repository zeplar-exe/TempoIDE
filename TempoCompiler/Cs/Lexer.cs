using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;

namespace TempoCompiler.Cs
{
    internal class Lexer : IEnumerable<LexerToken>
    {
        private static TokenDef[] tokens =
        {
            new TokenDef("abstract", TokenType.Abstract, 5),
            new TokenDef("as", TokenType.As, 5),
            new TokenDef("async", TokenType.Async, 5),
            new TokenDef("base", TokenType.Base, 5),
            new TokenDef("bool", TokenType.Bool, 5),
            new TokenDef("break", TokenType.Break, 5),
            new TokenDef("byte", TokenType.Byte, 5),
            new TokenDef("case", TokenType.Case, 5),
            new TokenDef("catch", TokenType.Catch, 5),
            new TokenDef("char", TokenType.Char, 5),
            new TokenDef("checked", TokenType.Checked, 5),
            new TokenDef("class", TokenType.Class, 5),
            new TokenDef("const", TokenType.Const, 5),
            new TokenDef("continue", TokenType.Continue, 5),
            new TokenDef("decimal", TokenType.Decimal, 5),
            new TokenDef("default", TokenType.Default, 5),
            new TokenDef("delegate", TokenType.Delegate, 5),
            new TokenDef("do", TokenType.Do, 5),
            new TokenDef("double", TokenType.Double, 5),
            new TokenDef("else", TokenType.Else, 5),
            new TokenDef("enum", TokenType.Enum, 5),
            new TokenDef("event", TokenType.Event, 5),
            new TokenDef("explicit", TokenType.Explicit, 5),
            new TokenDef("extern", TokenType.Extern, 5),
            new TokenDef("false", TokenType.False, 5),
            new TokenDef("finally", TokenType.Finally, 5),
            new TokenDef("fixed", TokenType.Fixed, 5),
            new TokenDef("float", TokenType.Float, 5),
            new TokenDef("for", TokenType.For, 5),
            new TokenDef("foreach", TokenType.Foreach, 5),
            new TokenDef("goto", TokenType.Goto, 5),
            new TokenDef("if", TokenType.If, 5),
            new TokenDef("implicit", TokenType.Implicit, 5),
            new TokenDef("in", TokenType.In, 5),
            new TokenDef("int", TokenType.Int, 5),
            new TokenDef("interface", TokenType.Interface, 5),
            new TokenDef("internal", TokenType.Internal, 5),
            new TokenDef("is", TokenType.Is, 5),
            new TokenDef("lock", TokenType.Lock, 5),
            new TokenDef("long", TokenType.Long, 5),
            new TokenDef("namespace", TokenType.Namespace, 5),
            new TokenDef("new", TokenType.New, 5),
            new TokenDef("null", TokenType.Null, 5),
            new TokenDef("object", TokenType.Object, 5),
            new TokenDef("operator", TokenType.Operator, 5),
            new TokenDef("out", TokenType.Out, 5),
            new TokenDef("override", TokenType.Override, 5),
            new TokenDef("params", TokenType.Params, 5),
            new TokenDef("private", TokenType.Private, 5),
            new TokenDef("protected", TokenType.Protected, 5),
            new TokenDef("public", TokenType.Public, 5),
            new TokenDef("readonly", TokenType.Readonly, 5),
            new TokenDef("ref", TokenType.Ref, 5),
            new TokenDef("return", TokenType.Return, 5),
            new TokenDef("sbyte", TokenType.Sbyte, 5),
            new TokenDef("sealed", TokenType.Sealed, 5),
            new TokenDef("short", TokenType.Short, 5),
            new TokenDef("sizeof", TokenType.Sizeof, 5),
            new TokenDef("stackalloc", TokenType.Stackalloc, 5),
            new TokenDef("static", TokenType.Static, 5),
            new TokenDef("string", TokenType.String, 5),
            new TokenDef("struct", TokenType.Struct, 5),
            new TokenDef("switch", TokenType.Switch, 5),
            new TokenDef("this", TokenType.This, 5),
            new TokenDef("throw", TokenType.Throw, 5),
            new TokenDef("true", TokenType.True, 5),
            new TokenDef("try", TokenType.Try, 5),
            new TokenDef("typeof", TokenType.Typeof, 5),
            new TokenDef("unit", TokenType.Uint, 5),
            new TokenDef("ulong", TokenType.Ulong, 5),
            new TokenDef("unchecked", TokenType.Unchecked, 5),
            new TokenDef("unsafe", TokenType.Unsafe, 5),
            new TokenDef("ushort", TokenType.Ushort, 5),
            new TokenDef("using", TokenType.Using, 5),
            new TokenDef("virtual", TokenType.Virtual, 5),
            new TokenDef("void", TokenType.Void, 5),
            new TokenDef("volatile", TokenType.Volatile, 5),
            new TokenDef("while", TokenType.While, 5),
            new TokenDef("where", TokenType.Where, 5),
            
            new TokenDef("[A-Za-z1-9_]*", TokenType.Identifier, 6),
            new TokenDef(@"\d*", TokenType.Number, 5),
            
            new TokenDef(@"\.", TokenType.Period, 5),
            new TokenDef(",", TokenType.Comma, 5),
            new TokenDef("\"", TokenType.DoubleQuote, 5),
            new TokenDef("'", TokenType.SingleQuote, 5),
            
            new TokenDef("(", TokenType.LeftParen, 5),
            new TokenDef(")", TokenType.RightParen, 5),
            new TokenDef("[", TokenType.LeftBracket, 5),
            new TokenDef("]", TokenType.RightBracket, 5),
            new TokenDef("{", TokenType.BlockOpen, 5),
            new TokenDef("}", TokenType.BlockClose, 5),
            
            new TokenDef(@"\s", TokenType.Whitespace, 5),
        };

        private string text;
        private Vector2 readPosition = new Vector2(0, 0);

        public Lexer(string input)
        {
            text = input;
        }

        public IEnumerator<LexerToken> GetEnumerator()
        {
            Array.Sort(tokens);
            
            yield return new LexerToken("", TokenType.EndOfFile);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public readonly struct LexerToken
    {
        public readonly string Value;
        public readonly TokenType Type;

        public LexerToken(string value, TokenType type)
        {
            Value = value;
            Type = type;
        }
    }
}