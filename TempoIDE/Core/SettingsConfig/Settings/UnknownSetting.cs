namespace TempoIDE.Core.SettingsConfig.Settings
{
    public class UnknownSetting : SettingValue
    {
        public string Raw { get; }

        public UnknownSetting(string raw)
        {
            Raw = raw;
        }

        public override string ToString()
        {
            return Raw;
        }
    }
}