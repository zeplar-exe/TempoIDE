using System.IO;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Core.SettingsConfig.Directories.Plugins
{
    public class PluginDirectory : SettingDirectoryWrapper
    {
        public readonly PluginSettingOverrides SettingOverrides;
        
        public PluginDirectory(DirectoryInfo directory) : base(directory)
        {
            SettingOverrides = new PluginSettingOverrides(Directory.ToRelativeDirectory("settings")); 
        }

        public override void Write()
        {
            SettingOverrides.Write();
        }

        public override void Dispose()
        {
            SettingOverrides.Dispose();
        }
    }
}