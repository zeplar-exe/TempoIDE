using System.Text;
using SettingsConfig.Settings;

namespace TempoIDE.Core.SettingsConfig.Directories.Plugins
{
    public class PluginMapping
    {
        public const string MappingKey = "plugin_def";
        
        public bool IsPinned { get; set; }
        public bool IsEnabled { get; set; }
        
        public string Name { get; set; }
        public string RelativePath { get; set; }
        public int LoadPriority { get; set; } = -1;

        public void Enable() => IsEnabled = true;
        public void Disable() => IsEnabled = false;
        
        public void Pin() => IsPinned = true;
        public void Unpin() => IsPinned = false;

        public override string ToString()
        {
            var builder = new StringBuilder();

            var root = new Setting(MappingKey, new SettingTree(new[]
            {
                Setting.Create("enabled", IsEnabled),
                Setting.Create("pinned", IsPinned),
                Setting.Create("name", Name),
                Setting.Create("path", RelativePath),
                Setting.Create("priority", LoadPriority),
            }));

            builder.Append(root);

            return builder.ToString();
        }
    }
}