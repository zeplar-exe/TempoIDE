using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Settings
{
    public class Setting : ISetting
    {
        public readonly string Key;
        public readonly SettingValue Value;
        public StringContext Context { get; }

        public Setting(string key, SettingValue value, StringContext context)
        {
            Key = key;
            Value = value;
            Context = context;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}