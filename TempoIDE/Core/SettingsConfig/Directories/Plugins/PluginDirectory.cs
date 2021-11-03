using System.IO;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Core.SettingsConfig.Directories.Plugins
{
    public class PluginDirectory : SettingDirectoryWrapper
    {
        public readonly PluginSettingsOverrides SettingsOverrides;
        
        public PluginDirectory(DirectoryInfo directory) : base(directory)
        {
            SettingsOverrides = new PluginSettingsOverrides(Directory.ToRelativeDirectory("appdata")); 
        }

        public override void Parse()
        {
            SettingsOverrides.Parse();
        }

        public override void Write()
        {
            SettingsOverrides.Write();
        }
    }
}