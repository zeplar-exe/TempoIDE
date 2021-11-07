namespace TempoIDE.Core.SettingsConfig.Settings
{
    public class BooleanSetting : SettingValue
    {
        public bool Value { get; }
        
        public BooleanSetting(bool value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}