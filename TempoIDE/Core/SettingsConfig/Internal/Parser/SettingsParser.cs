using System;
using System.Collections.Generic;
using Jammo.ParserTools;
using TempoIDE.Core.SettingsConfig.Internal.Parser.Nodes;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser
{
    public class SettingsParser
    {
        private readonly EnumerableNavigator<SettingsToken> navigator;

        private readonly List<ParserError> errors = new();

        public IEnumerable<ParserError> Errors => errors;

        public SettingsParser(string text)
        {
            navigator = new SettingsLexer(text).Lex().ToNavigator();
        }

        public IEnumerable<Setting> Parse()
        {
            foreach (var token in navigator.EnumerateFromIndex())
            {
                switch (token.Id)
                {
                    case SettingsTokenId.Identifier:
                    {
                        if (!navigator.TakeIf(t => t.Is(SettingsTokenId.Equals), out _))
                        {
                            ReportError("Expected a '='");
                            break;
                        }

                        if (navigator.TakeIf(t => t.Is(SettingsTokenId.OpenCurlyBracket), out _))
                        {
                            yield return new Setting(token.Text, ParseMethod(token));
                            
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

        private MethodSetting ParseMethod(SettingsToken name)
        {
            
        }

        private BlockNode ParseBlock()
        {
            var node = new BlockNode(navigator.Current.Context);
            
            foreach (var token in navigator.EnumerateFromIndex())
            {
                if (token.Is(SettingsTokenId.CloseCurlyBracket))
                    break;

                if (token.Is(SettingsTokenId.Identifier)) 
                {
                    
                }
                else if (token.IsKeyword())
                {
                    switch (token.Id)
                    {
                        case SettingsTokenId.OrKeyword:
                            if (!navigator.TakeIf(t => t.Is(SettingsTokenId.OpenCurlyBracket), out _))
                            {
                                ReportError("Expected a '{'");
                                break;
                            }
                            
                            node.AddNode(ParseBlock());
                            
                            break;
                    }
                }
                else
                {
                    ReportError("Unexpected token.");
                }
            }

            return node;
        }

        private void ReportError(string message)
        {
            errors.Add(new ParserError(message, navigator.Current.Context));
        }
    }
}