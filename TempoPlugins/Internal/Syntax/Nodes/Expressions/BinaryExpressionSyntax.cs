using Jammo.ParserTools;
using TempoPlugins.Internal.Lexer;

namespace TempoPlugins.Internal.Syntax.Nodes.Expressions
{
    public class BinaryExpressionSyntax : ExpressionSyntax
    {
        public ExpressionSyntax Left;
        public TelToken Operator;
        public ExpressionSyntax Right;

        public new static BinaryExpressionSyntax Parse(EnumerableNavigator<TelToken> navigator)
        {
            var syntax = new BinaryExpressionSyntax();

            
            
            return syntax;
        }
        
        public override string ToString()
        {
            return $"{Left?.ToString() ?? ""} {Operator?.ToString() ?? ""} {Right?.ToString() ?? ""}";
        }
    }
}