namespace TempoPlugins
{
    public readonly struct LexerError
    {
        public readonly string Message;
        public readonly string Text;
        public readonly int Line;
        public readonly int Column;

        public LexerError(string message, string text, int line, int column)
        {
            Message = message;
            Text = text;
            Line = line;
            Column = column;
        }
    }
}