namespace TempoPlugins.Syntax.Nodes.Expressions
{
    public class BinaryExpressionSyntax : ExpressionSyntax
    {
        public ExpressionSyntax Left;
        public TelToken Operator;
        public ExpressionSyntax Right;
        
        public override string ToString()
        {
            return $"{Left?.ToString() ?? ""} {Operator?.ToString() ?? ""} {Right?.ToString() ?? ""}";
        }
    }
}