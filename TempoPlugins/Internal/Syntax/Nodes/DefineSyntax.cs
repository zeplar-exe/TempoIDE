using Jammo.ParserTools;
using TempoPlugins.Internal.Lexer;

namespace TempoPlugins.Internal.Syntax.Nodes
{
    public class DefineSyntax : TelSyntaxNode
    {
        public TelToken DefineToken;
        public TelIdentifier Identifier;
        public AsSyntax Initializer;
    }
}