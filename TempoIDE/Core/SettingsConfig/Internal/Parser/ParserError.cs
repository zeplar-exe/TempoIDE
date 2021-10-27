using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser
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