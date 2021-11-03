using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TempoIDE.Core.SettingsConfig.Directories.Plugins
{
    public class PluginSettingsOverrides : SettingDirectoryWrapper
    {
        private readonly List<SettingsFileOverride> overrides = new();

        public IEnumerable<SettingsFileOverride> Overrides => overrides.AsReadOnly();

        public PluginSettingsOverrides(DirectoryInfo directory) : base(directory)
        {
            
        }

        public override void Parse()
        {
            overrides.AddRange(GetOverrides());
        }

        private IEnumerable<SettingsFileOverride> GetOverrides()
        {
            return Directory
                .EnumerateFiles("*.txt", SearchOption.AllDirectories)
                .Select(f => new SettingsFileOverride(f));
        }

        public override void Write()
        {
            foreach (var @override in Overrides)
                @override.Write();
        }
    }
}