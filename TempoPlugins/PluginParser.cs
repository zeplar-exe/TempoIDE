using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Jammo.ParserTools;
using Newtonsoft.Json.Linq;

namespace TempoPlugins
{
    public static class PluginParser
    {
        public static PluginStream Parse(string input)
        {
            var stream = new PluginStream();
            var tokenizer = new Tokenizer(input);

            var previous = new BasicTokenCollection();
            var state = new StateMachine<ParserState>(ParserState.Any);
            
            foreach (var token in tokenizer)
            {
                switch (state.Current)
                {
                    case ParserState.Any:
                    {
                        switch (token.Text.ToLower())
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
                        if (token.Type is BasicTokenType.Whitespace or BasicTokenType.Newline)
                            break;
                        
                        if (token.Type is BasicTokenType.Alphabetical or BasicTokenType.Numerical)
                        {
                            stream.Version += token.Text;
                        }
                        else
                        {
                            state.MoveLast();
                        }
                        
                        break;
                    }
                    case ParserState.MetaData:
                    {
                        if (token.Type is BasicTokenType.Whitespace or BasicTokenType.Newline)
                            break;
                        
                        if (token.Text != "}")
                        {
                            if (token.Text == ":")
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
                        if (token.Text != "\"")
                            break;

                        BasicToken metaToken;
                        var tokens = new BasicTokenCollection();
                        
                        while ((metaToken = tokenizer.Next()) != null)
                        {
                            if (metaToken.Text == "\"")
                                break;
                            
                            tokens.Add(metaToken);
                        }

                        var name = previous.LastOrDefault(t => t.Text == ":");
                        
                        if (name == null)
                            break;

                        var index = previous.IndexOf(name);
                        
                        BasicToken nameToken;
                        var nameTokens = new BasicTokenCollection();
                        
                        while (true)
                        {
                            if (--index < 0)
                                break;

                            nameToken = previous[index];
                            
                            if (nameToken.Type is BasicTokenType.Whitespace or BasicTokenType.Newline)
                                continue;
                            
                            if (nameToken.Type == BasicTokenType.Punctuation)
                                break;
                            
                            nameTokens.Add(nameToken);
                        }

                        stream.Metadata[nameTokens.ToString()] = tokens.ToString();
                        
                        state.MoveLast();
                        
                        break;
                    }
                }
                
                previous.Add(token);
            }

            return stream;
        }

        private class StateMachine<TStates> : IEnumerable<TStates> where TStates : Enum
        {
            private readonly List<TStates> previous = new();

            public TStates Current { get; private set; }

            public StateMachine() { }
            public StateMachine(TStates start)
            {
                Current = start;
            }

            public void MoveTo(TStates state)
            {
                previous.Add(Current);
                Current = state;
            }

            public void MoveLast()
            {
                Current = previous.Last();
                previous.Remove(Current);
            }
            
            public TStates PeekLast()
            {
                return previous.Last();
            }

            public IEnumerator<TStates> GetEnumerator()
            {
                return previous.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private enum ParserState
        {
            Any,
            Version,
            MetaData,
            MetaDataSetter
        }
    }
}