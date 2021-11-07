using System.Globalization;

namespace TempoIDE.Core.SettingsConfig.Settings
{
    public class NumericSetting : SettingValue
    {
        public double Value { get; }

        public NumericSetting(double value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}