namespace TempoPlugins.Syntax.Nodes
{
    public class ParserError
    {
        public readonly string Message;

        public ParserError(string message)
        {
            Message = message;
        }
    }
}