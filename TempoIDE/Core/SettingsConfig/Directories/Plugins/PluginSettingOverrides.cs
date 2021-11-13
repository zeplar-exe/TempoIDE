using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TempoIDE.Core.SettingsConfig.Directories.Plugins
{
    public class PluginSettingOverrides : SettingDirectoryWrapper
    {
        private readonly string[] protectedRelativePaths =
        {
            
        };

        public IEnumerable<SettingsFileOverride> Overrides { get; }

        public PluginSettingOverrides(DirectoryInfo directory) : base(directory)
        {
            Overrides = Directory
                .EnumerateFiles("*.txt", SearchOption.AllDirectories)
                .Where(f => !protectedRelativePaths.Any(p => f.FullName.EndsWith(p)))
                .Select(f => new SettingsFileOverride(f, Path.GetRelativePath(Directory.FullName, f.FullName)))
                .ToArray();
        }
        
        public override void Write()
        {
            foreach (var settingOverrides in Overrides)
                settingOverrides.Write();
        }

        public override void Dispose()
        {
            
        }
    }
}