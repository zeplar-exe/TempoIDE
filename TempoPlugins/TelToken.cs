namespace TempoPlugins
{
    public class TelToken
    {
        public readonly string Text;
        public readonly int Line;
        public readonly int Column;
        public readonly TelTokenId Id;

        public TelToken(string text, int line, int column, TelTokenId id)
        {
            Text = text;
            Line = line;
            Column = column;
            Id = id;
        }

        public bool Is(TelTokenId id) => Id == id;

        public override string ToString()
        {
            return Text;
        }
    }
}