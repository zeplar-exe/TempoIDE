using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TempoIDE.Core.SettingsConfig.Directories.Plugins
{
    public class PluginSettingsOverrides : SettingDirectoryWrapper
    {
        public IEnumerable<SettingsFileOverride> Overrides { get; private set; }

        public PluginSettingsOverrides(DirectoryInfo directory) : base(directory)
        {
            Overrides = GetOverrides();
        }

        public override void Parse()
        {
            Overrides = GetOverrides();
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