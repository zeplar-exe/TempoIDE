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

            if (!navigator.TryMoveNext(out _))
                return syntax;
            
            syntax.Value = ExpressionSyntax.Parse(navigator);

            return syntax;
        }
    }
}