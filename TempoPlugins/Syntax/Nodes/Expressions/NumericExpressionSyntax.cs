using Jammo.ParserTools;

namespace TempoPlugins.Syntax.Nodes.Expressions
{
    public class NumericExpressionSyntax : ExpressionSyntax
    {
        public TelToken Literal;
        
        public new static NumericExpressionSyntax Parse(EnumerableNavigator<TelToken> navigator)
        {
            return new NumericExpressionSyntax { Literal = navigator.Current };
        }

        public override string ToString()
        {
            return Literal.ToString();
        }
    }
}