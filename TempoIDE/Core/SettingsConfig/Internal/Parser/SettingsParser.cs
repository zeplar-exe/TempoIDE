using System.Collections.Generic;
using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser
{
    public class SettingsParser
    {
        private readonly EnumerableNavigator<GenericLexerToken<SettingsTokenId>> navigator;

        private readonly List<ParserError> errors = new();

        public IEnumerable<ParserError> Errors => errors;

        public SettingsParser(string text)
        {
            navigator = new SettingsLexer(text).Lex().ToNavigator();
        }

        public IEnumerable<DataPair> Parse()
        {
            foreach (var token in navigator.EnumerateFromIndex())
            {
                switch (token.Id)
                {
                    case SettingsTokenId.Identifier:
                    {
                        if (!navigator.TakeIf(t => t.Is(SettingsTokenId.Equals), out var equals))
                        {
                            ReportError("Expected a '='");
                            break;
                        }

                        if (navigator.TakeIf(t => t.Is(SettingsTokenId.OpenCurlyBracket), out var openBlock))
                        {
                            yield return new DataPair(token.Text, new ExtendedData(ParseBlock()));
                            
                            break;
                        }
                        
                        break;
                    }
                    default:
                        ReportError("Unexpected token.");
                        break;
                }
            }
            
            yield break;
        }

        private SettingsNode ParseBlock()
        {
            return null;
        }

        private void ReportError(string message)
        {
            errors.Add(new ParserError(message, navigator.Current.Context));
        }
    }
}