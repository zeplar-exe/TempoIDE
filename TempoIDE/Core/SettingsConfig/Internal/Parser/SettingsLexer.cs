using System.Collections.Generic;
using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser
{
    public class SettingsLexer
    {
        private readonly EnumerableNavigator<LexerToken> navigator;

        private readonly List<LexerError> errors = new();
        
        public IEnumerable<LexerError> Errors => errors;

        public SettingsLexer(string text)
        {
            navigator = new Lexer(text, new LexerOptions
            {
                IncludeUnderscoreAsAlphabetic = true,
                IncludePeriodAsNumeric = true
            }).ToNavigator();
        }

        public IEnumerable<SettingsToken> Lex()
        {
            foreach (var token in navigator.EnumerateFromIndex())
            {
                switch (token.Id)
                {
                    case LexerTokenId.Alphabetic:
                    case LexerTokenId.AlphaNumeric:
                    {
                        switch (token.ToString())
                        {
                            case "IF":
                                yield return CreateToken(SettingsTokenId.IfKeyword);
                                break;
                            case "OR":
                                yield return CreateToken(SettingsTokenId.OrKeyword);
                                break;
                            case "AND":
                                yield return CreateToken(SettingsTokenId.AndKeyword);
                                break;
                            case "NOR":
                                yield return CreateToken(SettingsTokenId.NorKeyword);
                                break;
                            case "NAND":
                                yield return CreateToken(SettingsTokenId.NandKeyword);
                                break;
                            case "XOR":
                                yield return CreateToken(SettingsTokenId.XorKeyword);
                                break;
                            case "XAND":
                                yield return CreateToken(SettingsTokenId.XandKeyword);
                                break;
                            default:
                                yield return CreateToken(SettingsTokenId.Identifier);
                                break;
                        }
                        
                        break;
                    }
                    case LexerTokenId.DoubleQuote:
                        var fullString = new List<string>();
                        
                        foreach (var stringToken in navigator.EnumerateFromIndex())
                        {
                            if (stringToken.Is(LexerTokenId.DoubleQuote))
                            {
                                navigator.TryPeekLast(out var last);
                                
                                if (!last.Is(LexerTokenId.Backslash))
                                    break;
                            }
                            
                            fullString.Add(stringToken.ToString());
                        }

                        yield return new SettingsToken(
                            string.Concat(fullString), token.Context,
                            SettingsTokenId.StringLiteral);
                        
                        break;
                    case LexerTokenId.Numeric:
                        yield return CreateToken(SettingsTokenId.NumericLiteral);
                        break;
                    case LexerTokenId.LeftParenthesis:
                        yield return CreateToken(SettingsTokenId.OpenParenthesis);
                        break;
                    case LexerTokenId.RightParenthesis:
                        yield return CreateToken(SettingsTokenId.CloseParenthesis);
                        break;
                    case LexerTokenId.OpenCurlyBracket:
                        yield return CreateToken(SettingsTokenId.OpenCurlyBracket);
                        break;
                    case LexerTokenId.CloseCurlyBracket:
                        yield return CreateToken(SettingsTokenId.CloseCurlyBracket);
                        break;
                    case LexerTokenId.Period:
                        yield return CreateToken(SettingsTokenId.Period);
                        break;
                    case LexerTokenId.Equals:
                        if (navigator.TakeIf(t => t.Is(LexerTokenId.GreaterThan), out _))
                        {
                            yield return CreateToken(SettingsTokenId.IsOperator);
                            break;
                        }

                        yield return CreateToken(SettingsTokenId.Equals);
                        break;
                    case LexerTokenId.Newline:
                    case LexerTokenId.Whitespace:
                        continue;
                    default:
                        ReportError("Unexpected token.");
                        yield return CreateToken(SettingsTokenId.Unknown);
                        break;
                }
            }
        }

        private SettingsToken CreateToken(SettingsTokenId id)
        {
            return new SettingsToken(navigator.Current.ToString(), navigator.Current.Context, id);
        }
        private void ReportError(string message)
        {
            errors.Add(new LexerError(message, navigator.Current.ToString(), navigator.Current.Context));
        }
    }
}