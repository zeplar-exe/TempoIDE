using Jammo.ParserTools;

namespace TempoPlugins.Internal.Syntax.Nodes
{
    public readonly struct ParserError
    {
        public readonly string Message;
        public readonly StringContext Context;

        public ParserError(string message, StringContext context)
        {
            Message = message;
            Context = context;
        }
    }
}