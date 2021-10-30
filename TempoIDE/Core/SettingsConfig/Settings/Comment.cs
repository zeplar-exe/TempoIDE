using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Settings
{
    public class Comment : ISetting
    {
        public readonly string Text;
        public StringContext Context { get; }

        public Comment(string text, StringContext context)
        {
            Text = text;
            Context = context;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}