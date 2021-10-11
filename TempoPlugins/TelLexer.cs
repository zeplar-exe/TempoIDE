using System.Collections.Generic;
using System.Linq;
using Jammo.ParserTools;

namespace TempoPlugins
{
    public class TelLexer
    {
        private readonly string text;
        
        private int line;
        private int column;

        private readonly List<TelToken> tokens = new();
        private readonly List<LexerError> errors = new();

        public TelLexer(string text)
        {
            this.text = text;
        }

        public IEnumerable<TelToken> Tokens => tokens;
        public IEnumerable<LexerError> Errors => errors;

        public IEnumerable<TelToken> Lex()
        {
            line = 0;
            column = 0;
            
            errors.Clear();
            
            var lexer = new Lexer(text, new LexerOptions
            {
                IncludeUnderscoreAsAlphabetic = true,
                IncludePeriodAsNumeric = true
            });
            var preceding = new LinkedList<LexerToken>();

            bool LastIs(LexerTokenId id) => preceding.LastOrDefault()?.Is(id) ?? false;
            bool NextIs(LexerTokenId id) => lexer.PeekNext()?.Is(id) ?? false;
            
            foreach (var token in lexer)
            {
                column += token.RawToken.Length;
                
                switch (token.Id)
                {
                    case LexerTokenId.Alphabetic:
                    {
                        switch (token.RawToken)
                        {
                            case "define":
                                tokens.Add(new TelToken(token.ToString(), TelTokenId.DefineInstruction));
                                break;
                            case "as":
                                tokens.Add(new TelToken(token.ToString(), TelTokenId.AsInstruction));
                                break;
                            case "import":
                                tokens.Add(new TelToken(token.ToString(), TelTokenId.ImportInstruction));
                                break;
                            case "from":
                                tokens.Add(new TelToken(token.ToString(), TelTokenId.FromInstruction));
                                break;
                            case "protocol":
                                tokens.Add(new TelToken(token.ToString(), TelTokenId.ProtocolKeyword));
                                break;
                            default:
                                tokens.Add(new TelToken(token.ToString(), TelTokenId.Identifier));
                                break;
                        }
                        
                        break;
                    }
                    case LexerTokenId.Numeric:
                    {
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.NumericLiteral));
                        break;
                    }
                    case LexerTokenId.Quote:
                    case LexerTokenId.DoubleQuote:
                    {
                        if (LastIs(LexerTokenId.Backslash))
                        {
                            ReportError(token);
                            break;
                        }
                        
                        var stringTokens = new List<LexerToken> { token };

                        foreach (var literalToken in lexer)
                        {
                            stringTokens.Add(literalToken);
                            
                            if (literalToken.Is(token.Id))
                            {
                                if (!LastIs(LexerTokenId.Backslash))
                                {
                                    tokens.Add(new TelToken(
                                        string.Concat(stringTokens.SelectMany(t => t.RawToken)),
                                        TelTokenId.StringLiteral));
                                    
                                    break;
                                }
                            }

                            preceding.AddFirst(token);
                        }
                        
                        break;
                    }
                    case LexerTokenId.LeftParenthesis:
                    {
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.OpenParenthesis));
                        
                        break;
                    }
                    case LexerTokenId.RightParenthesis:
                    {
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.CloseParenthesis));
                        break;
                    }
                    case LexerTokenId.OpenBracket:
                    {
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.OpenBracket));
                        break;    
                    }
                    case LexerTokenId.CloseBracket:
                    {
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.CloseBracket));
                        break;
                    }
                    case LexerTokenId.OpenCurlyBracket:
                    {
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.OpenCurlyBracket));
                        break;
                    }
                    case LexerTokenId.CloseCurlyBracket:
                    {
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.CloseCurlyBracket));
                        break;
                    }
                    case LexerTokenId.Space:
                    {
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.Whitespace));
                        break;
                    }
                    case LexerTokenId.NewLine:
                    {
                        line++;
                        column = 0;
                        
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.Newline));
                        break;
                    }
                    case LexerTokenId.Period:
                    {
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.Period));
                        break;
                    }
                    case LexerTokenId.Comma:
                    {
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.Comma));
                        break;
                    }
                    case LexerTokenId.Plus:
                    {
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.Plus));
                        break;
                    }
                    case LexerTokenId.Dash:
                    {
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.Minus));
                        break;
                    }
                    case LexerTokenId.Star:
                    {
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.Multiply));
                        break;
                    }
                    case LexerTokenId.Slash:
                    {
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.Divide));
                        break;
                    }
                    case LexerTokenId.LessThan:
                    {
                        if (NextIs(LexerTokenId.Equals))
                        {
                            lexer.Skip();
                            
                            tokens.Add(new TelToken(token.ToString(), TelTokenId.LessThanOrEqual));
                            break;
                        }
                        
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.LessThan));
                        break;
                    }
                    case LexerTokenId.GreaterThan:
                    {
                        if (NextIs(LexerTokenId.Equals))
                        {
                            lexer.Skip();
                            
                            tokens.Add(new TelToken(token.ToString(), TelTokenId.GreaterThanOrEqual));
                            break;
                        }
                        
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.GreaterThan));
                        break;
                    }
                    case LexerTokenId.Equals:
                    {
                        if (NextIs(LexerTokenId.Equals))
                        {
                            lexer.Skip();
                            
                            tokens.Add(new TelToken(token.ToString(), TelTokenId.Equals));
                            break;
                        }

                        if (NextIs(LexerTokenId.GreaterThan))
                        {
                            lexer.Skip();
                            
                            tokens.Add(new TelToken(token.ToString(), TelTokenId.DelegateOperator));
                            break;
                        }
                        
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.Assignment));
                        break;
                    }
                    case LexerTokenId.ExclamationMark:
                    {
                        if (NextIs(LexerTokenId.Equals))
                        {
                            lexer.Skip();
                            
                            tokens.Add(new TelToken(token.ToString(), TelTokenId.NotEqual));
                            break;
                        }
                        
                        tokens.Add(new TelToken(token.ToString(), TelTokenId.Not));
                        break;
                    }
                    case LexerTokenId.Vertical:
                    {
                        if (NextIs(LexerTokenId.Vertical))
                        {
                            lexer.Skip();
                            
                            tokens.Add(new TelToken(token.ToString(), TelTokenId.Or));
                            break;
                        }
                        
                        ReportError(token);
                        break;
                    }
                    case LexerTokenId.Amphersand:
                    {
                        if (NextIs(LexerTokenId.Amphersand))
                        {
                            lexer.Skip();
                            
                            tokens.Add(new TelToken(token.ToString(), TelTokenId.And));
                            break;
                        }
                        
                        ReportError(token);
                        break;
                    }
                    default:
                    {
                        ReportError(token);
                        break;
                    }
                }
                
                preceding.AddFirst(token);
            }

            return tokens;
        }

        private void ReportError(LexerToken token)
        {
            errors.Add(new LexerError("Unexpected character", token.ToString(), line, column));
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

        public bool Is(TelTokenId id) => Id == id;

        public override string ToString()
        {
            return Text;
        }
    }
}