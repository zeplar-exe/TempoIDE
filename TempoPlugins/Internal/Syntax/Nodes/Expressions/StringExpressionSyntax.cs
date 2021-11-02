using Jammo.ParserTools;
using TempoPlugins.Internal.Lexer;

namespace TempoPlugins.Internal.Syntax.Nodes.Expressions
{
    public class StringExpressionSyntax : ExpressionSyntax
    {
        public TelToken Literal;

        public new static StringExpressionSyntax Parse(EnumerableNavigator<TelToken> navigator)
        {
            return new StringExpressionSyntax { Literal = navigator.Current };
        }
        
        public override string ToString()
        {
            return Literal.ToString();
        }
    }
}