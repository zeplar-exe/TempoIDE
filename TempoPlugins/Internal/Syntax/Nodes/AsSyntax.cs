using Jammo.ParserTools;
using TempoPlugins.Internal.Lexer;
using TempoPlugins.Internal.Syntax.Nodes.Expressions;

namespace TempoPlugins.Internal.Syntax.Nodes
{
    public class AsSyntax : TelSyntaxNode
    {
        public TelToken AsToken;
        public ExpressionSyntax Value;
    }
}