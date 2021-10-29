namespace TempoIDE.Core.SettingsConfig
{
    public abstract class TextSetting : SettingValue
    {
        public string Value;
        
        protected TextSetting(string value)
        {
            Value = value;
        }
    }
}