using TempoPlugins.Internal.Lexer;

namespace TempoPlugins.Internal.Syntax.Nodes.Expressions
{
    public class UnknownExpressionSyntax : ExpressionSyntax
    {
        public TelToken Token;

        public UnknownExpressionSyntax(TelToken token)
        {
            Token = token;
        }
        
        public override string ToString()
        {
            return Token.ToString();
        }
    }
}