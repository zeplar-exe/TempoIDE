using System.Collections.Generic;
using System.Linq;
using Jammo.ParserTools;

namespace TempoPlugins
{
    public class TelLexer
    {
        private readonly string text;
        private EnumerableNavigator<LexerToken> navigator;

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
            
            navigator = new Lexer(text, new TokenizerOptions(BasicTokenType.Whitespace), 
                new LexerOptions 
                {
                    IncludeUnderscoreAsAlphabetic = true,
                    IncludePeriodAsNumeric = true
                }).ToNavigator();

            foreach (var token in navigator.EnumerateFromIndex())
            {
                column += token.RawToken.Length;

                switch (token.Id)
                {
                    case LexerTokenId.Alphabetic:
                    {
                        switch (token.RawToken)
                        {
                            case "define":
                                AddToken(token.ToString(), TelTokenId.DefineInstruction);
                                break;
                            case "as":
                                AddToken(token.ToString(), TelTokenId.AsInstruction);
                                break;
                            case "import":
                                AddToken(token.ToString(), TelTokenId.ImportInstruction);
                                break;
                            case "from":
                                AddToken(token.ToString(), TelTokenId.FromInstruction);
                                break;
                            case "protocol":
                                AddToken(token.ToString(), TelTokenId.ProtocolKeyword);
                                break;
                            case "and":
                                AddToken(token.ToString(), TelTokenId.And);
                                break;
                            case "or":
                                AddToken(token.ToString(), TelTokenId.Or);
                                break;
                            case "not":
                                AddToken(token.ToString(), TelTokenId.Not);
                                break;
                            default:
                                AddToken(token.ToString(), TelTokenId.Identifier);
                                break;
                        }

                        break;
                    }
                    case LexerTokenId.Numeric:
                    {
                        AddToken(token.ToString(), TelTokenId.NumericLiteral);
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
                        
                        foreach (var literalToken in navigator.EnumerateFromIndex())
                        {
                            stringTokens.Add(literalToken);

                            if (literalToken.Is(token.Id) && !LastIs(LexerTokenId.Backslash))
                            { // Passing the id guarantees the closing token will be the same
                                AddToken(
                                    string.Concat(stringTokens.SelectMany(t => t.RawToken)),
                                    TelTokenId.StringLiteral);

                                break;
                            }
                        }

                        break;
                    }
                    case LexerTokenId.LeftParenthesis:
                    {
                        AddToken(token.ToString(), TelTokenId.OpenParenthesis);

                        break;
                    }
                    case LexerTokenId.RightParenthesis:
                    {
                        AddToken(token.ToString(), TelTokenId.CloseParenthesis);
                        break;
                    }
                    case LexerTokenId.OpenBracket:
                    {
                        AddToken(token.ToString(), TelTokenId.OpenBracket);
                        break;
                    }
                    case LexerTokenId.CloseBracket:
                    {
                        AddToken(token.ToString(), TelTokenId.CloseBracket);
                        break;
                    }
                    case LexerTokenId.OpenCurlyBracket:
                    {
                        AddToken(token.ToString(), TelTokenId.OpenCurlyBracket);
                        break;
                    }
                    case LexerTokenId.CloseCurlyBracket:
                    {
                        AddToken(token.ToString(), TelTokenId.CloseCurlyBracket);
                        break;
                    }
                    case LexerTokenId.NewLine:
                    {
                        line++;
                        column = 0;
                        
                        break;
                    }
                    case LexerTokenId.Period:
                    {
                        AddToken(token.ToString(), TelTokenId.Period);
                        break;
                    }
                    case LexerTokenId.Comma:
                    {
                        AddToken(token.ToString(), TelTokenId.Comma);
                        break;
                    }
                    case LexerTokenId.Plus:
                    {
                        AddToken(token.ToString(), TelTokenId.Plus);
                        break;
                    }
                    case LexerTokenId.Dash:
                    {
                        AddToken(token.ToString(), TelTokenId.Minus);
                        break;
                    }
                    case LexerTokenId.Star:
                    {
                        AddToken(token.ToString(), TelTokenId.Multiply);
                        break;
                    }
                    case LexerTokenId.Slash:
                    {
                        AddToken(token.ToString(), TelTokenId.Divide);
                        break;
                    }
                    case LexerTokenId.LessThan:
                    {
                        if (NextIs(LexerTokenId.Equals))
                        {
                            navigator.Skip();

                            AddToken(token.ToString(), TelTokenId.LessThanOrEqual);
                            break;
                        }

                        AddToken(token.ToString(), TelTokenId.LessThan);
                        break;
                    }
                    case LexerTokenId.GreaterThan:
                    {
                        if (NextIs(LexerTokenId.Equals))
                        {
                            navigator.Skip();

                            AddToken(token.ToString(), TelTokenId.GreaterThanOrEqual);
                            break;
                        }

                        AddToken(token.ToString(), TelTokenId.GreaterThan);
                        break;
                    }
                    case LexerTokenId.Equals:
                    {
                        if (NextIs(LexerTokenId.Equals))
                        {
                            navigator.Skip();

                            AddToken(token.ToString(), TelTokenId.Equals);
                            break;
                        }

                        if (NextIs(LexerTokenId.GreaterThan))
                        {
                            navigator.Skip();

                            AddToken(token.ToString(), TelTokenId.DelegateOperator);
                            break;
                        }

                        AddToken(token.ToString(), TelTokenId.Assignment);
                        break;
                    }
                    case LexerTokenId.ExclamationMark:
                    {
                        if (NextIs(LexerTokenId.Equals))
                        {
                            navigator.Skip();

                            AddToken(token.ToString(), TelTokenId.NotEqual);
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
            }

            return tokens;
        }

        private bool NextIs(LexerTokenId id)
        {
            return navigator.TakeIf(t=> t.Is(id), out _);
        }
        
        private bool LastIs(LexerTokenId id)
        {
            return navigator.TryPeekLast(out var token) && token.Is(id);
        }

        private void AddToken(string rawText, TelTokenId id)
        {
            tokens.Add(new TelToken(rawText, line, column, id));
        }

        private void ReportError(LexerToken token)
        {
            errors.Add(new LexerError("Unexpected character", token.ToString(), line, column));
        }
    }
}