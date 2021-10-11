using Jammo.ParserTools;

namespace TempoPlugins.Syntax.Nodes.Expressions
{
    public class ArithmeticExpressionSyntax : NumericExpressionSyntax
    {
        public new static ArithmeticExpressionSyntax Parse(EnumerableNavigator<TelToken> navigator)
        {
            var syntax = new ArithmeticExpressionSyntax();

            return syntax;
        }
    }
}