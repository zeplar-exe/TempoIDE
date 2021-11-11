using System.Collections.Generic;
using System.IO;
using System.Linq;
using TempoIDE.Core.SettingsConfig.Settings;
using TempoIDE.Core.SettingsConfig.Settings.SettingsFiles;

namespace TempoIDE.Core.SettingsConfig.Directories.Plugins
{
    public class UserPluginConfig : Config
    {
        public IEnumerable<PluginMapping> Mappings { get; private set; } = Enumerable.Empty<PluginMapping>();
        public IEnumerable<PluginMapping> OrderedMappings => Mappings?.OrderByDescending(m => m.LoadPriority);

        public UserPluginConfig(FileInfo file) : base(file)
        {
            Parse();
        }

        public UserPluginConfig(Stream stream) : base(stream)
        {
            Parse();
        }

        private void Parse()
        {
            var mappings = new List<PluginMapping>();
            
            foreach (var setting in Document.Settings)
            {
                switch (setting.Key.ToLower())
                {
                    case PluginMapping.MappingKey:
                        if (ReportIfUnexpectedSettingType(setting, out SettingTree tree))
                            break;

                        var mapping = new PluginMapping();

                        foreach (var mappingSetting in tree.Settings)
                        {
                                switch (mappingSetting.Key)
                                {
                                    case "enabled":
                                        if (mappingSetting.Value is not BooleanSetting enabled)
                                            break;

                                        mapping.IsEnabled = enabled.Value;
                                        break;
                                    case "pinned":
                                        if (mappingSetting.Value is not BooleanSetting pinned)
                                            break;

                                        mapping.IsPinned = pinned.Value;
                        
                                        break;
                                    case "name":
                                        if (mappingSetting.Value is not TextSetting name)
                                            break;

                                        mapping.Name = name.Value;
                        
                                        break;
                                    case "path":
                                        if (mappingSetting.Value is not TextSetting path)
                                            break;

                                        mapping.RelativePath = path.Value;
                        
                                        break;
                                    case "priority":
                                        if (mappingSetting.Value is not NumericSetting priority)
                                            break;

                                        mapping.LoadPriority = (int)priority.Value;
                                        break;
                                }
                        }
                        
                        mappings.Add(mapping);
                        
                        break;
                    default:
                        ReportUnexpectedSetting(setting);
                        break;
                }
            }

            Mappings = mappings;
        }

        public override void Write()
        {
            using var writer = CreateWriter();
            
            foreach (var mapping in Mappings)
                writer.WriteLineAsync(mapping.ToString());
        }
    }
}