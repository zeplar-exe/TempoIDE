namespace TempoIDE.Core.SettingsConfig
{
    public abstract class SettingValue
    {
        private readonly string raw;

        protected SettingValue(string raw)
        {
            this.raw = raw;
        }

        public override string ToString()
        {
            return raw;
        }
    }
}