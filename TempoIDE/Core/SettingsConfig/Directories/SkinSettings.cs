using System.Collections.Generic;
using System.IO;
using System.Linq;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.SettingsConfig.Settings.SettingsFiles;

namespace TempoIDE.Core.SettingsConfig.Directories
{
    public class SkinSettings : SettingDirectoryWrapper
    {
        public SkinConfig SkinConfig { get; }
        public IEnumerable<SkinDefinition> SkinDefinitions { get; private set; }
        
        public SkinSettings(DirectoryInfo directory) : base(directory)
        {
            SkinConfig = new SkinConfig(Directory.ToFile("skin.txt").CreateIfMissing());
            SkinDefinitions = Directory.ToRelativeDirectory("skins")
                .CreateIfMissing()
                .EnumerateFiles("*.txt", SearchOption.AllDirectories)
                .Select(file => new SkinDefinition(file))
                .ToList();
        }
        
        public override void Write()
        {
            SkinConfig.Write();
        }

        public bool TryGetSkin(string name, out SkinDefinition definition)
        {
            definition = SkinDefinitions.FirstOrDefault(s => s.Name == name);

            return definition != null;
        }
        
        public override void Dispose()
        {
            SkinConfig?.Dispose();
            DisposeDefinitions();
        }

        private void DisposeDefinitions()
        {
            foreach (var def in SkinDefinitions ?? Enumerable.Empty<SkinDefinition>())
                def.Dispose();
        }
    }
}