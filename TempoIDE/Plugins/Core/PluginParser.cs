using System.Collections.Generic;
using System.Linq;
using Jammo.ParserTools;
using Jammo.ParserTools.Lexing;
using Jammo.ParserTools.Tokenization;
using Jammo.ParserTools.Tools;

namespace TempoIDE.Plugins.Core;

public static class PluginParser
{
    public static PluginStream Parse(string input)
    {
        var stream = new PluginStream();
        var navigator = new Lexer(input).ToNavigator();

        var previous = new List<LexerToken>();
        var state = new StateMachine<ParserState>(ParserState.Any);
            
        foreach (var token in navigator.EnumerateFromIndex())
        {
            switch (state.Current)
            {
                case ParserState.Any:
                {
                    switch (token.ToString().ToLower())
                    {
                        case "version":
                            state.MoveTo(ParserState.Version);
                            break;
                        case "meta":
                            state.MoveTo(ParserState.MetaData);
                            break;
                    }

                    break;
                }
                case ParserState.Version:
                {
                    var readStarted = false;
                        
                    while (navigator.TryPeekNext(out var versionToken))
                    {
                        previous.Add(versionToken);

                        if (versionToken.Is(LexerTokenId.Numeric))
                        {
                            readStarted = true;
                            stream.Version += versionToken.ToString();
                        }
                        else if (readStarted)
                        {
                            state.MoveLast();
                            break;
                        }
                    }
                        
                    break;
                }
                case ParserState.MetaData:
                {
                    if (token.Is(LexerTokenId.Whitespace) || token.Is(LexerTokenId.Newline))
                        break;
                        
                    if (token.Is(LexerTokenId.CloseCurlyBracket))
                    {
                        if (token.Is(LexerTokenId.Colon))
                        {
                            state.MoveTo(ParserState.MetaDataSetter);
                        }
                    }
                    else
                    {
                        state.MoveLast();
                    }
                        
                    break;
                }
                case ParserState.MetaDataSetter:
                {
                    if (!token.Is(LexerTokenId.Backslash))
                        break;
                        
                    var tokens = new List<LexerToken>();
                        
                    while (navigator.TryMoveNext(out var metaToken))
                    {
                        if (metaToken.Is(LexerTokenId.Backslash))
                            break;
                            
                        tokens.Add(metaToken);
                    }

                    var name = previous.LastOrDefault(t => t.Is(LexerTokenId.Colon));
                        
                    if (name == null)
                        break;

                    var index = previous.IndexOf(name);

                    var nameTokens = new List<LexerToken>();
                        
                    while (true)
                    {
                        if (--index < 0)
                            break;

                        var nameToken = previous[index];
                            
                        if (nameToken.Is(LexerTokenId.Whitespace) || nameToken.Is(LexerTokenId.Newline))
                            continue;
                            
                        if (nameToken.Token.Type == BasicTokenType.Punctuation)
                            break;
                            
                        nameTokens.Add(nameToken);
                    }

                    stream.Metadata[string.Concat(nameTokens.Select(t => t.Token))] = string.Concat(tokens.ToString);
                        
                    state.MoveLast();
                        
                    break;
                }
            }
                
            previous.Add(token);
        }

        return stream;
    }

    private enum ParserState
    {
        Any,
        Version,
        MetaData,
        MetaDataSetter
    }
}