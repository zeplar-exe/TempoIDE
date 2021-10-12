namespace TempoPlugins.Syntax.Nodes.Expressions
{
    public class UnknownExpressionSyntax : ExpressionSyntax
    {
        public TelToken Token;

        public UnknownExpressionSyntax(TelToken token)
        {
            Token = token;
        }
    }
}