using System;
using System.Collections.Generic;
using Jammo.ParserTools;
using TempoIDE.Core.SettingsConfig.Internal.Parser.Nodes;
using TempoIDE.Core.SettingsConfig.Settings;
using TempoIDE.Core.SettingsConfig.Settings.Methods;

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
                            yield return new Setting(token.ToString(), ParseMethod(token));
                            
                            break;
                        }

                        if (!navigator.TryMoveNext(out var literalToken) || !literalToken.IsLiteral())
                        {
                            ReportError("Expected a literal value.");
                            break;
                        }

                        yield return new Setting(token.ToString(), new TextSetting(literalToken.ToString()));
                        
                        break;
                    }
                    case SettingsTokenId.Comment:
                        yield return new Setting("", new Comment(token.ToString()));
                        break;
                    default:
                        ReportError("Unexpected token.");
                        break;
                }
            }
            
            yield break;
        }
        
        private MethodSetting ParseMethod(SettingsToken name)
        {
            var setting = new MethodSetting(name.ToString());
            
            foreach (var block in ParseBlock().Nodes)
            {
                // Test if member access, method invocation, etc
            }

            return setting;
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
                    if (navigator.TakeIf(t => t.Is(SettingsTokenId.Period), out _))
                    {
                        
                    }
                }
                else if (token.IsLogical())
                {
                    if (!navigator.TakeIf(t => t.Is(SettingsTokenId.OpenCurlyBracket), out _))
                    {
                        ReportError("Expected a '{'");
                        continue;
                    }
                    
                    node.AddNode(ParseBlock());
                }
                else if (token.IsControl())
                {
                    
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