using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Settings
{
    public class Setting : ISetting
    {
        public readonly string Key;
        public readonly SettingValue Value;
        public StringContext Context { get; }

        public Setting(string key, SettingValue value, StringContext context = new())
        {
            Key = key;
            Value = value;
            Context = context;
        }

        public static Setting Create(string key, string value, StringContext context = new())
        {
            return new Setting(key, new TextSetting(value), context);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
        
        public string ToFullString()
        {
            return $"{Key}=\"{Value}\"";
        }
    }
}