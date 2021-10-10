using System;
using System.Collections.Generic;
using System.Linq;
using Jammo.ParserTools;

namespace TempoPlugins
{
    public static class TelLexer
    {
        public static IEnumerable<TelToken> Lex(string input)
        {
            var lexer = new Lexer(input, new LexerOptions
            {
                IncludeUnderscoreAsAlphabetic = true,
                IncludePeriodAsNumeric = true
            });
            var preceding = new LinkedList<LexerToken>();

            bool LastIs(LexerTokenId id) => preceding.LastOrDefault()?.Is(id) ?? false;
            
            foreach (var token in lexer)
            {
                switch (token.Id)
                {
                    case LexerTokenId.Alphabetic:
                    {
                        switch (token.RawToken)
                        {
                            case "define":
                                yield return new TelToken(token.ToString(), TelTokenId.DefineInstruction);
                                break;
                            case "as":
                                yield return new TelToken(token.ToString(), TelTokenId.AsInstruction);
                                break;
                            case "import":
                                yield return new TelToken(token.ToString(), TelTokenId.ImportInstruction);
                                break;
                            case "from":
                                yield return new TelToken(token.ToString(), TelTokenId.FromInstruction);
                                break;
                            case "protocol":
                                yield return new TelToken(token.ToString(), TelTokenId.ProtocolKeyword);
                                break;
                            default:
                                yield return new TelToken(token.ToString(), TelTokenId.Identifier);
                                break;
                        }
                        
                        break;
                    }
                    case LexerTokenId.Numeric:
                    {
                        yield return new TelToken(token.ToString(), TelTokenId.NumericLiteral);
                        break;
                    }
                    case LexerTokenId.Quote:
                    case LexerTokenId.DoubleQuote:
                    {
                        if (LastIs(LexerTokenId.Backslash))
                        {
                            yield return new TelToken(token.ToString(), TelTokenId.Unknown);
                            break;
                        }
                        
                        var tokens = new List<LexerToken> { token };

                        foreach (var literalToken in lexer)
                        {
                            tokens.Add(literalToken);
                            
                            if (literalToken.Is(token.Id))
                            {
                                if (!LastIs(LexerTokenId.Backslash))
                                {
                                    yield return new TelToken(
                                        string.Concat(tokens.SelectMany(t => t.RawToken)),
                                        TelTokenId.StringLiteral);
                                    
                                    break;
                                }
                            }

                            preceding.AddFirst(token);
                        }
                        
                        break;
                    }
                    case LexerTokenId.LeftParenthesis:
                    {
                        yield return new TelToken(token.ToString(), TelTokenId.OpenParenthesis);
                        
                        break;
                    }
                    case LexerTokenId.RightParenthesis:
                    {
                        yield return new TelToken(token.ToString(), TelTokenId.CloseParenthesis);
                        break;
                    }
                    case LexerTokenId.OpenBracket:
                    {
                        yield return new TelToken(token.ToString(), TelTokenId.OpenBracket);
                        break;    
                    }
                    case LexerTokenId.CloseBracket:
                    {
                        yield return new TelToken(token.ToString(), TelTokenId.CloseBracket);
                        break;
                    }
                    case LexerTokenId.OpenCurlyBracket:
                    {
                        yield return new TelToken(token.ToString(), TelTokenId.OpenCurlyBracket);
                        break;
                    }
                    case LexerTokenId.CloseCurlyBracket:
                    {
                        yield return new TelToken(token.ToString(), TelTokenId.CloseCurlyBracket);
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
                    case LexerTokenId.Period:
                    {
                        yield return new TelToken(token.ToString(), TelTokenId.Period);
                        break;
                    }
                    case LexerTokenId.Comma:
                    {
                        yield return new TelToken(token.ToString(), TelTokenId.Comma);
                        break;
                    }
                    case LexerTokenId.Plus:
                    {
                        yield return new TelToken(token.ToString(), TelTokenId.Plus);
                        break;
                    }
                    case LexerTokenId.Dash:
                    {
                        yield return new TelToken(token.ToString(), TelTokenId.Minus);
                        break;
                    }
                    case LexerTokenId.Star:
                    {
                        yield return new TelToken(token.ToString(), TelTokenId.Multiply);
                        break;
                    }
                    case LexerTokenId.Slash:
                    {
                        yield return new TelToken(token.ToString(), TelTokenId.Divide);
                        break;
                    }
                    case LexerTokenId.LessThan:
                    {
                        if (lexer.PeekNext().Is(LexerTokenId.Equals))
                        {
                            lexer.Skip();
                            
                            yield return new TelToken(token.ToString(), TelTokenId.LessThanOrEqual);
                            break;
                        }
                        
                        yield return new TelToken(token.ToString(), TelTokenId.LessThan);
                        break;
                    }
                    case LexerTokenId.GreaterThan:
                    {
                        if (lexer.PeekNext().Is(LexerTokenId.Equals))
                        {
                            lexer.Skip();
                            
                            yield return new TelToken(token.ToString(), TelTokenId.GreaterThanOrEqual);
                            break;
                        }
                        
                        yield return new TelToken(token.ToString(), TelTokenId.GreaterThan);
                        break;
                    }
                    case LexerTokenId.Equals:
                    {
                        if (lexer.PeekNext().Is(LexerTokenId.Equals))
                        {
                            lexer.Skip();
                            
                            yield return new TelToken(token.ToString(), TelTokenId.Equals);
                            break;
                        }

                        if (lexer.PeekNext().Is(LexerTokenId.GreaterThan))
                        {
                            lexer.Skip();
                            
                            yield return new TelToken(token.ToString(), TelTokenId.DelegateOperator);
                            break;
                        }
                        
                        yield return new TelToken(token.ToString(), TelTokenId.Assignment);
                        break;
                    }
                    case LexerTokenId.ExclamationMark:
                    {
                        if (lexer.PeekNext().Is(LexerTokenId.Equals))
                        {
                            lexer.Skip();
                            
                            yield return new TelToken(token.ToString(), TelTokenId.NotEqual);
                            break;
                        }
                        
                        yield return new TelToken(token.ToString(), TelTokenId.Not);
                        break;
                    }
                    case LexerTokenId.Vertical:
                    {
                        if (lexer.PeekNext().Is(LexerTokenId.Vertical))
                        {
                            lexer.Skip();
                            
                            yield return new TelToken(token.ToString(), TelTokenId.Or);
                            break;
                        }
                        
                        yield return new TelToken(token.ToString(), TelTokenId.Unknown);
                        break;
                    }
                    case LexerTokenId.Amphersand:
                    {
                        if (lexer.PeekNext().Is(LexerTokenId.Amphersand))
                        {
                            lexer.Skip();
                            
                            yield return new TelToken(token.ToString(), TelTokenId.And);
                            break;
                        }
                        
                        yield return new TelToken(token.ToString(), TelTokenId.Unknown);
                        break;
                    }
                    default:
                    {
                        yield return new TelToken(token.ToString(), TelTokenId.Unknown);
                        break;
                    }
                }
                
                preceding.AddFirst(token);
            }
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
}