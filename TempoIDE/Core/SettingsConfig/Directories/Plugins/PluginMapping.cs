using System.Text;
using TempoIDE.Core.SettingsConfig.Settings;

namespace TempoIDE.Core.SettingsConfig.Directories.Plugins
{
    public class PluginMapping
    {
        public const string MappingKey = "plugin_def";
        
        public bool IsPinned { get; private set; }
        public bool IsEnabled { get; private set; }
        
        public string Name { get; set; }
        public string RelativePath { get; set; }
        public int LoadPriority { get; set; } = -1;

        public void Enable() => IsEnabled = true;
        public void Disable() => IsEnabled = false;
        
        public void Pin() => IsPinned = true;
        public void Unpin() => IsPinned = false;

        public void Parse(SettingTree tree)
        {
            foreach (var setting in tree.Settings)
            {
                switch (setting.Key)
                {
                    case "enabled":
                        if (setting.Value is not BooleanSetting enabled)
                            break;

                        IsEnabled = enabled.Value;
                        break;
                    case "pinned":
                        if (setting.Value is not BooleanSetting pinned)
                            break;

                        IsPinned = pinned.Value;
                        
                        break;
                    case "name":
                        if (setting.Value is not TextSetting name)
                            break;

                        Name = name.Value;
                        
                        break;
                    case "path":
                        if (setting.Value is not TextSetting path)
                            break;

                        RelativePath = path.Value;
                        
                        break;
                    case "priority":
                        if (setting.Value is not NumericSetting priority)
                            break;

                        LoadPriority = (int)priority.Value;
                        
                        break;
                }
            }
        }

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