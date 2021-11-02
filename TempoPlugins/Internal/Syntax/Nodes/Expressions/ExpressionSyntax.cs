using Jammo.ParserTools;
using TempoPlugins.Internal.Lexer;

namespace TempoPlugins.Internal.Syntax.Nodes.Expressions
{
    public abstract class ExpressionSyntax : TelSyntaxNode
    {
        public abstract override string ToString();
    }
}