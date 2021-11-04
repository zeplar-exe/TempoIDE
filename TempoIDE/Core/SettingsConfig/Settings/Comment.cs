using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Settings
{
    public class Comment : ISetting
    {
        public readonly string Text;
        public StringContext Context { get; }

        public Comment(string text, StringContext context = new())
        {
            Text = text;
            Context = context;
        }

        public override string ToString()
        {
            return Text;
        }
        
        public string ToFullString()
        {
            return ToString();
        }
    }
}