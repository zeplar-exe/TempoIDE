using Jammo.ParserTools;

namespace TempoPlugins.Syntax.Nodes.Expressions
{
    public class NumericExpressionSyntax : ExpressionSyntax
    {
        public TelToken Literal;
        
        public new static NumericExpressionSyntax Parse(EnumerableNavigator<TelToken> navigator)
        {
            var syntax = new NumericExpressionSyntax();
            var token = navigator.Current;

            do
            {
                switch (token.Id)
                {
                    case TelTokenId.Plus or TelTokenId.Minus or TelTokenId.Multiply or TelTokenId.Divide:
                    {
                        return new ArithmeticExpressionSyntax(); //...
                    }
                }
            } while (navigator.TryMoveNext(out token));
            
            return syntax;
        }
    }
}