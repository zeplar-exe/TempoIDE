using System.Collections.Generic;
using Jammo.ParserTools;

namespace TempoPlugins
{
    public static partial class TelParser
    {
        public static IEnumerable<TelToken> Lex(string input)
        {
            var state = new StateMachine<LexerState>(LexerState.Any);
            var lexer = new Lexer(input);
            var preceding = new LinkedList<LexerToken>();

            // TODO: Lexer theory (recursive descent?)
            
            foreach (var token in lexer)
            {
                switch (state.Current)
                {
                    case LexerState.Any:
                    {
                        switch (token.Id)
                        {
                            case LexerTokenId.Alphabetic:
                            case LexerTokenId.AlphaNumeric:
                            case LexerTokenId.Underscore:
                            case LexerTokenId.Minus:
                            {
                                state.MoveTo(LexerState.Identifier);
                                
                                break;
                            }
                            case LexerTokenId.LeftParenthesis:
                            {
                                state.MoveTo(LexerState.ParenthesisGroup);
                                
                                break;
                            }
                            case LexerTokenId.OpenBracket:
                            {
                                state.MoveTo(LexerState.BracketGroup);
                                
                                break;    
                            }
                            case LexerTokenId.OpenCurlyBracket:
                            {
                                state.MoveTo(LexerState.Block);
                                
                                break;
                            }
                            case LexerTokenId.Space:
                            {
                                yield return new TelToken(token.ToString(), TelTokenId.Whitespace);
                                
                                break;
                            }
                            case LexerTokenId.NewLine:
                            {
                                yield return new TelToken(token.ToString(), TelTokenId.Newline);
                                
                                break;
                            }
                            default:
                            {
                                break;
                            }
                        }
                        
                        break;
                    }
                    case LexerState.Identifier:
                    {
                        break;
                    }
                    case LexerState.ParenthesisGroup:
                    {
                        break;
                    }
                    case LexerState.BracketGroup:
                    {
                        break;
                    }
                    case LexerState.Block:
                    {
                        break;
                    }
                }

                preceding.AddFirst(token);
            }
        }

        private enum LexerState
        {
            Any,
            Identifier,
            
            ParenthesisGroup,
            BracketGroup,
            Block
        }

        private class Token
        {
            public string Text { get; private set; }

            public Token(string text = "")
            {
                Text = text;
            }

            public void Append(string text)
            {
                Text += text;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        public class TelToken
        {
            public readonly string Text;
            public readonly TelTokenId Id;

            public TelToken(string text, TelTokenId id)
            {
                Text = text;
                Id = id;
            }
        }

        public enum TelTokenId
        {
            Whitespace,
            Newline,
            
            Identifier,
            
            DefineInstruction,
            AsInstruction,
            ImportInstruction,
            FromInstruction,
            
            ProtocolKeyword,
            
            DelegateOperator,
            Plus, Dash, Star, Slash,
            LessThan, LessThanOrEqual,
            MoreThan, MoreThanOrEqual,
            Assignment, Equals, Not, NotEqual,
            Or, And,

            OpenParenthesis,
            CloseParenthesis,
            OpenBracket,
            CloseBracket,
            OpenCurlyBracket,
            CloseCurlyBracket,
        }
    }
}