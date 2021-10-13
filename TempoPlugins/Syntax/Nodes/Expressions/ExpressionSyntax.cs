using Jammo.ParserTools;

namespace TempoPlugins.Syntax.Nodes.Expressions
{
    public abstract class ExpressionSyntax : TelSyntaxNode
    {
        internal static ExpressionSyntax Parse(EnumerableNavigator<TelToken> navigator)
        {
            switch (navigator.Current.Id)
            {
                case TelTokenId.StringLiteral:
                    return StringExpressionSyntax.Parse(navigator);
                case TelTokenId.NumericLiteral:
                    return NumericExpressionSyntax.Parse(navigator);
                default:
                    return new UnknownExpressionSyntax(navigator.Current);
            }
        }

        public abstract override string ToString();
    }
}