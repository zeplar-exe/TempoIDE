namespace TempoPlugins.Syntax.Nodes.Expressions
{
    public class BinaryExpressionSyntax : ExpressionSyntax
    {
        public ExpressionSyntax Left;
        public TelToken Operator;
        public ExpressionSyntax Right;
    }
}