using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TempoIDE.Core.SettingsConfig.Settings
{
    public class SettingTree : SettingValue
    {
        public IEnumerable<Setting> Settings { get; }
        
        public Setting this[string key] => Settings.FirstOrDefault(s => s.Key == key);
        
        public SettingTree(IEnumerable<Setting> settings)
        {
            Settings = settings;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append('[');

            foreach (var setting in Settings)
                builder.AppendLine(setting.ToString());
            
            builder.Append(']');
            
            return builder.ToString();
        }
    }
}