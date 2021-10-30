namespace TempoIDE.Core.SettingsConfig.Settings
{
    public class Comment : SettingValue
    {
        public readonly string Text;

        public Comment(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}