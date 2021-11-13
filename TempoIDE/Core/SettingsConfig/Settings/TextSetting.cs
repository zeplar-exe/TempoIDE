namespace TempoIDE.Core.SettingsConfig.Settings
{
    public class TextSetting : SettingValue
    {
        public string Value { get; }
        
        public TextSetting(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}