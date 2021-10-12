using Jammo.ParserTools;
using TempoPlugins.Syntax;
using TempoPlugins.Syntax.Nodes;

namespace TempoPlugins
{
    public static class TelParser
    {
        public static TelSyntaxTree Parse(string text)
        {
            var root = new TelCompilationRoot();
            var state = new StateMachine<ParserState>();
            var navigator = new TelLexer(text).Lex().ToNavigator();

            var token = navigator.Current;
            
            do
            {
                switch (state.Current)
                {
                    case ParserState.Any:
                    {
                        switch (token.Id)
                        {
                            case TelTokenId.DefineInstruction:
                            {
                                root.AddNode(DefineSyntax.Parse(navigator));

                                break;
                            }
                            case TelTokenId.Newline:
                                continue;
                        }

                        break;
                    }
                }
            } while (navigator.TryMoveNext(out token));

            return new TelSyntaxTree(root);
        }

        private enum ParserState
        {
            Any
        }
    }
}