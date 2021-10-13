using Jammo.ParserTools;
using TempoPlugins.Syntax.Nodes.Expressions;

namespace TempoPlugins.Syntax.Nodes
{
    public class AsSyntax : TelSyntaxNode
    {
        public TelToken AsToken;
        public ExpressionSyntax Value;

        internal static AsSyntax Parse(EnumerableNavigator<TelToken> navigator)
        {
            var syntax = new AsSyntax { AsToken = navigator.Current };

            while (navigator.TryMoveNext(out var token))
            {
                if (syntax.HasError)
                    break;
                
                switch (token.Id)
                {
                    case TelTokenId.Newline:
                        syntax.ReportError("Expected an expression.");
                        break;
                    default:
                        if (!token.IsIdentifier())
                        {
                            syntax.ReportError("Expected an expression.");
                            break;
                        }
                        
                        syntax.Value = ExpressionSyntax.Parse(navigator);

                        return syntax;
                }
            }

            return syntax;
        }
    }
}