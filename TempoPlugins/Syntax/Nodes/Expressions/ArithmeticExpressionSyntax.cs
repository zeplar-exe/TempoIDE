using Jammo.ParserTools;

namespace TempoPlugins.Syntax.Nodes.Expressions
{
    public class ArithmeticExpressionSyntax : BinaryExpressionSyntax
    {
        public new static ArithmeticExpressionSyntax Parse(EnumerableNavigator<TelToken> navigator)
        {
            var syntax = new ArithmeticExpressionSyntax();
            var token = navigator.Current;

            do
            {
                switch (token)
                {
                    
                }

                navigator.TryPeekNext(out token);
                
                if (!token.Is(TelTokenId.NumericLiteral) && !token.IsArithmetic())
                    break;
            } while (navigator.TryMoveNext(out token));

            return syntax;
        }
    }
}