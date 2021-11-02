using Jammo.ParserTools;

namespace TempoPlugins.Internal.Lexer
{
    public class TelToken
    {
        public readonly string Text;
        public readonly StringContext Context;
        public readonly TelTokenId Id;

        public TelToken(string text, StringContext context, TelTokenId id)
        {
            Text = text;
            Context = context;
            Id = id;
        }

        public bool Is(TelTokenId id) => Id == id;

        public override string ToString()
        {
            return Text;
        }
    }
}