namespace TempoIDE.Core.SettingsConfig
{
    public class TextSetting : SettingValue
    {
        public readonly string Value;
        
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