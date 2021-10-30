namespace TempoIDE.Core.SettingsConfig
{
    public class Setting
    {
        public readonly string Key;
        public readonly SettingValue Value;

        public Setting(string key, SettingValue value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}