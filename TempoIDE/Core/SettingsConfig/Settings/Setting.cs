using Jammo.ParserTools;
using TempoIDE.Core.Interfaces;

namespace TempoIDE.Core.SettingsConfig.Settings
{
    public class Setting : IProcessedStringPart
    {
        public readonly string Key;
        public readonly SettingValue Value;
        
        public StringContext Context { get; }

        internal Setting(string key, SettingValue value, StringContext context = new())
        {
            Key = key;
            Value = value;
            Context = context;
        }
        
        public Setting(string key, SettingValue value)
        {
            Key = key;
            Value = value;
        }

        public static Setting Create(string key, string value)
        {
            return new Setting(key, new TextSetting(value));
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