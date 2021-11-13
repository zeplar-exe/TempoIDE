using System.Collections.Generic;
using System.IO;
using Jammo.ParserTools;
using TempoIDE.Core.SettingsConfig.Internal.Lexer;
using TempoIDE.Core.SettingsConfig.Internal.Parser.Nodes;
using TempoIDE.Core.SettingsConfig.Settings;
using TempoIDE.Core.SettingsConfig.Settings.Methods;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser
{
    public class SettingsParser
    {
        private readonly EnumerableNavigator<SettingsToken> navigator;

        private readonly List<ParserError> errors = new();

        public IEnumerable<ParserError> Errors => errors.AsReadOnly();

        public SettingsParser(Stream stream)
        {
            using var reader = new StreamReader(stream, leaveOpen: true);

            navigator = new SettingsLexer(reader.ReadToEnd()).Lex().ToNavigator();
        }
        
        public SettingsParser(string text)
        {
            navigator = new SettingsLexer(text).Lex().ToNavigator();
        }

        public SettingsDocument Parse()
        {
            return new SettingsDocument(ParseSettings());
        }
        
        public IEnumerable<Setting> ParseSettings()
        {
            foreach (var token in navigator.EnumerateFromIndex())
            {
                switch (token.Id)
                {
                    case SettingsTokenId.Identifier:
                    {
                        if (TryParseSetting(token, out var setting))
                            yield return setting;
                        
                        break;
                    }
                    case SettingsTokenId.Comment:
                        continue;
                    default:
                        ReportError("Unexpected token.");
                        break;
                }
            }
            
            yield break;
        }

        private SettingTree ParseTree()
        {
            var children = new List<Setting>();

            foreach (var token in navigator.EnumerateFromIndex())
            {
                switch (token.Id)
                {
                    case SettingsTokenId.Identifier:
                        if (TryParseSetting(token, out var setting))
                            children.Add(setting);
                        break;
                    case SettingsTokenId.CloseBracket:
                        return new SettingTree(children);
                }
            }

            return new SettingTree(children);
        }

        private bool TryParseSetting(SettingsToken name, out Setting setting)
        {
            setting = default;
            
            if (!navigator.TryMoveNext(out var equalsToken) || !equalsToken.Is(SettingsTokenId.Equals))
            {
                ReportError("Expected a '='");
                
                return false;
            }

            if (navigator.TakeIf(t => t.Is(SettingsTokenId.OpenCurlyBracket), out _))
            {
                setting = new Setting(name.ToString(), ParseMethod(), name.Context);

                return true;
            }
            
            if (navigator.TakeIf(t => t.Is(SettingsTokenId.OpenBracket), out _))
            {
                setting = new Setting(name.ToString(), ParseTree(), name.Context);

                return true;
            }

            if (!navigator.TryMoveNext(out var literalToken) || !literalToken.IsLiteral())
            {
                ReportError("Expected a literal value.");
                
                setting = new Setting(name.ToString(), new UnknownSetting(literalToken.ToString()), name.Context);

                return true;
            }

            switch (literalToken.Id)
            {
                case SettingsTokenId.StringLiteral:
                    setting = Setting.Create(name.ToString(), literalToken.ToString(), name.Context);
                    return true;
                case SettingsTokenId.NumericLiteral:
                    if (int.TryParse(literalToken.Text, out var iResult))
                        setting = Setting.Create(name.ToString(), iResult, name.Context);
                    else if (float.TryParse(literalToken.ToString(), out var fResult))
                        setting = Setting.Create(name.ToString(), fResult, name.Context);
                    else if (double.TryParse(literalToken.ToString(), out var dResult))
                        setting = Setting.Create(name.ToString(), dResult, name.Context);
                    else
                        return false;
                    break;
                case SettingsTokenId.BooleanTrue or SettingsTokenId.BooleanFalse:
                    setting = Setting.Create(name.ToString(), bool.Parse(literalToken.ToString().ToLower()));
                    break;
                default:
                    setting = new Setting(name.ToString(), new UnknownSetting(literalToken.ToString()), name.Context);
                    break;
            }

            return true;
        }
        
        private MethodSetting ParseMethod()
        {
            var setting = new MethodSetting();
            
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