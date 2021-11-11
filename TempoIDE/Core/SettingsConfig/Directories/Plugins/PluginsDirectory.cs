using System.Collections.Generic;
using System.IO;
using System.Linq;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.Helpers.Plugins;

namespace TempoIDE.Core.SettingsConfig.Directories.Plugins
{
    public class PluginsDirectory : SettingDirectoryWrapper
    {
        public UserPluginConfig PluginConfig { get; }

        public IEnumerable<PluginWorker> Plugins { get; }
        
        public PluginsDirectory(DirectoryInfo directory) : base(directory)
        {
            PluginConfig = new UserPluginConfig(directory.ToFile("plugin_config.txt").CreateIfMissing());
            
            var list = new List<PluginWorker>();
            
            foreach (var mapping in PluginConfig.OrderedMappings)
            {
                var mappingDirectory = Directory.ToRelativeDirectory(mapping.RelativePath);
                    
                if (!mappingDirectory.Exists)
                {
                    ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_PLUGIN, 
                        $"The given plugin directory '{mapping.RelativePath}' does not exist.");
                        
                    continue;
                }
                
                list.Add(new PluginWorker(new PluginDirectory(mappingDirectory)));
            }
            
            Plugins = list;
        }

        public override void Write()
        {
            PluginConfig.Write();
            
            foreach (var worker in Plugins)
                worker.PluginDirectory.Write();
        }

        public override void Dispose()
        {
            PluginConfig.Dispose();
            DisposeWorkers();
        }

        private void DisposeWorkers()
        {
            foreach (var worker in Plugins ?? Enumerable.Empty<PluginWorker>())
                worker.Dispose();
        }
    }
}