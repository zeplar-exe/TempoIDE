using System.Collections.Generic;
using System.IO;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Core.SettingsConfig.Directories.Plugins
{
    public class PluginSettings : SettingDirectoryWrapper
    {
        public UserPluginConfig PluginConfig { get; }
        
        public IEnumerable<PluginDirectory> Plugins { get; private set; }
        
        public PluginSettings(DirectoryInfo directory) : base(directory)
        {
            PluginConfig = new UserPluginConfig(directory.ToFile("plugin_config").CreateIfMissing());
        }

        public override void Parse()
        {
            var list = new List<PluginDirectory>();
            
            foreach (var mapping in PluginConfig.OrderedMappings)
            {
                var directory = Directory.ToRelativeDirectory(mapping.RelativePath);
                    
                if (!directory.Exists)
                {
                    ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_PLUGIN, 
                        $"The given plugin directory '{mapping.RelativePath}' does not exist.");
                        
                    continue;
                }

                list.Add(new PluginDirectory(directory));
            }

            Plugins = list.ToArray();
        }

        public override void Write()
        {
            PluginConfig.Write();
            
            foreach (var pluginDirectory in Plugins)
                pluginDirectory.Write();
        }
    }
}