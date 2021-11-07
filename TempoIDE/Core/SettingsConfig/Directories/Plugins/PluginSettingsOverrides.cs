using System.IO;
using System.Linq;

namespace TempoIDE.Core.SettingsConfig.Directories.Plugins
{
    public class PluginSettingsOverrides : SettingDirectoryWrapper
    {
        private readonly string[] protectedRelativePaths =
        {
            
        };

        public SettingsFileOverride[] Overrides { get; private set; }

        public PluginSettingsOverrides(DirectoryInfo directory) : base(directory)
        {
            
        }

        public override void Parse()
        {
            Overrides = Directory
                .EnumerateFiles("*.txt", SearchOption.AllDirectories)
                .Where(f => protectedRelativePaths.Any(p => f.FullName.EndsWith(p)))
                .Select(f => new SettingsFileOverride(f))
                .ToArray();
        }

        public override void Write()
        {
            foreach (var @override in Overrides)
                @override.Write();
        }
    }
}