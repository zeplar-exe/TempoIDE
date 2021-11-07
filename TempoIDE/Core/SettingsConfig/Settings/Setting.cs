using Jammo.ParserTools;
using TempoIDE.Core.Interfaces;

namespace TempoIDE.Core.SettingsConfig.Settings
{
    public class Setting : IProcessedStringPart
    {
        public string Key { get; }
        public SettingValue Value { get; }
        
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

        public static Setting Create(string key, string value, StringContext context = new())
        {
            return new Setting(key, new TextSetting(value), context);
        }
        
        public static Setting Create(string key, bool value, StringContext context = new())
        {
            return new Setting(key, new BooleanSetting(value), context);
        }
        
        public static Setting Create(string key, int value, StringContext context = new())
        {
            return new Setting(key, new NumericSetting(value), context);
        }
        
        public static Setting Create(string key, float value, StringContext context = new())
        {
            return new Setting(key, new NumericSetting(value), context);
        }
        
        public static Setting Create(string key, double value, StringContext context = new())
        {
            return new Setting(key, new NumericSetting(value), context);
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