using TempoIDE.Core.SettingsConfig.Internal.Parser;

namespace TempoIDE.Core.SettingsConfig
{
    public abstract class ExtendedSetting : SettingValue
    {
        internal ExtendedSetting(SettingsNode node) : base(node.ToString())
        {
            
        }
    }
}