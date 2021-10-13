using Jammo.ParserTools;

namespace TempoPlugins.Syntax.Nodes
{
    public class DefineSyntax : TelSyntaxNode
    {
        public TelToken DefineToken;
        public TelIdentifier Identifier;
        public AsSyntax Initializer;

        internal static DefineSyntax Parse(EnumerableNavigator<TelToken> navigator)
        {
            var syntax = new DefineSyntax { DefineToken = navigator.Current };

            while (navigator.TryMoveNext(out var token))
            {
                if (syntax.HasError)
                    break;

                switch (token.Id)
                {
                    // TODO: Switch state so you can identifiy a mismatch like As coming before the identifier
                    case TelTokenId.Identifier:
                        syntax.Identifier = new TelIdentifier { Name = token.ToString() };

                        if (navigator.TakeIf(t => t.Is(TelTokenId.AsInstruction), out _))
                            syntax.Initializer = AsSyntax.Parse(navigator);

                        return syntax;
                    default:
                        syntax.ReportError("Expected an identifier.");
                        break;
                }
            }

            return syntax;
        }
    }
}