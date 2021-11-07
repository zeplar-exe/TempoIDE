using System.Collections.Generic;
using System.IO;
using System.Linq;
using TempoIDE.Core.SettingsConfig.Settings;
using TempoIDE.Core.SettingsConfig.Settings.SettingsFiles;

namespace TempoIDE.Core.SettingsConfig.Directories.Plugins
{
    public class UserPluginConfig : Config
    {
        private List<PluginMapping> MappingsList { get; } = new();

        public IEnumerable<PluginMapping> Mappings => MappingsList.AsReadOnly();
        public IEnumerable<PluginMapping> OrderedMappings => MappingsList.OrderByDescending(m => m.LoadPriority);

        public UserPluginConfig(FileInfo file) : base(file)
        {
            
        }

        public UserPluginConfig(Stream stream) : base(stream)
        {
            
        }

        public override void Parse()
        {
            foreach (var setting in Document.Settings)
            {
                switch (setting.Key.ToLower())
                {
                    case "plugin_def":
                        if (ReportIfUnexpectedSettingType(setting, out SettingTree tree))
                            break;

                        var mapping = new PluginMapping();

                        foreach (var pluginSetting in tree.Settings)
                        {
                            switch (pluginSetting.Key.ToLower())
                            {
                                case "name":
                                    if (ReportIfUnexpectedSettingType(setting, out TextSetting text))
                                        break;
                                        
                                    mapping.Name = text.ToString();
                                    break;
                            }
                        }
                        
                        break;
                    default:
                        ReportUnexpectedSetting(setting);
                        break;
                }
            }
        }

        public override void Write()
        {
            using var writer = CreateWriter();
            
            foreach (var mapping in MappingsList)
                writer.WriteLineAsync(mapping.ToString());
        }
    }
}