using Jammo.ParserTools;

namespace TempoPlugins.Syntax.Nodes.Expressions
{
    public class StringExpressionSyntax : ExpressionSyntax
    {
        public TelToken Literal;

        public new static StringExpressionSyntax Parse(EnumerableNavigator<TelToken> navigator)
        {
            return new StringExpressionSyntax { Literal = navigator.Current };
        }
    }
}