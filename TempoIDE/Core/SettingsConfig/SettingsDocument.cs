using System.Collections.Generic;
using System.Linq;
using System.Text;
using TempoIDE.Core.SettingsConfig.Settings;

namespace TempoIDE.Core.SettingsConfig
{
    public class SettingsDocument
    {
        private List<Setting> SettingsList { get; } = new();

        public IEnumerable<Setting> Settings => SettingsList.AsReadOnly();

        public Setting this[string key] => SettingsList.FirstOrDefault(s => s.Key == key);

        public SettingsDocument(IEnumerable<Setting> settings)
        {
            SettingsList.AddRange(settings);
        }
        
        public void AddSetting(Setting setting) => SettingsList.Add(setting);
        public bool RemoveSetting(Setting setting) => SettingsList.Remove(setting);

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var setting in SettingsList)
                builder.Append(setting);

            return builder.ToString();
        }
    }
}