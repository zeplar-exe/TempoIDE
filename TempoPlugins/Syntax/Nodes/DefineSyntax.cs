using Jammo.ParserTools;

namespace TempoPlugins.Syntax.Nodes
{
    public class DefineSyntax : TelSyntaxNode
    {
        public TelToken DefineToken;
        public string Identifier;
        public AsSyntax Assignment;

        public static DefineSyntax Parse(EnumerableNavigator<TelToken> navigator)
        {
            var syntax = new DefineSyntax { DefineToken = navigator.Current };

            while (navigator.TryMoveNext(out var token))
            {
                if (syntax.HasError)
                    break;
                
                switch (token.Id)
                {
                    case TelTokenId.Identifier:
                        syntax.Identifier = token.ToString();
                        break;
                    case TelTokenId.AsInstruction:
                        syntax.Assignment = AsSyntax.Parse(navigator);

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