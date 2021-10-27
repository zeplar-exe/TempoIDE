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
            navigator = new Lexer(text, new LexerOptions(t => t.Is(LexerTokenId.Whitespace))
            {
                IncludeUnderscoreAsAlphabetic = true,
                IncludePeriodAsNumeric = true
            }).ToNavigator();
        }

        public IEnumerable<GenericLexerToken<SettingsTokenId>> Lex()
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
                                CreateToken(SettingsTokenId.IfKeyword);
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
                            case "NOT":
                                yield return CreateToken(SettingsTokenId.NotKeyword);
                                break;
                            default:
                                yield return CreateToken(SettingsTokenId.Identifier);
                                break;
                        }
                        
                        break;
                    }
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
                    default:
                        ReportError("Unexpected token.");
                        break;
                }
            }
        }

        private GenericLexerToken<SettingsTokenId> CreateToken(SettingsTokenId id)
        {
            return new GenericLexerToken<SettingsTokenId>(navigator.Current.ToString(), navigator.Current.Context, id);
        }
        private void ReportError(string message)
        {
            errors.Add(new LexerError(message, navigator.Current.ToString(), navigator.Current.Context));
        }
    }
}