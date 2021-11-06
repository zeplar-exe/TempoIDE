using System.Collections.Generic;
using System.IO;
using System.Linq;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.SettingsConfig.Settings.SettingsFiles;

namespace TempoIDE.Core.SettingsConfig.Directories
{
    public class SkinSettings : SettingDirectoryWrapper
    {
        private readonly List<SkinDefinition> skinDefinitions = new();

        public readonly SkinConfig SkinConfig;
        public IEnumerable<SkinDefinition> SkinDefinitions => skinDefinitions.AsReadOnly();
        
        public SkinSettings(DirectoryInfo directory) : base(directory)
        {
            SkinConfig = new SkinConfig(Directory.ToFile("skin.txt").CreateIfMissing());
        }
        
        private IEnumerable<SkinDefinition> GetSkinDefinitions()
        {
            foreach (var file in Directory
                .ToRelativeDirectory("skins").CreateIfMissing()
                .EnumerateFiles("*.txt", SearchOption.AllDirectories))
            {
                var def = new SkinDefinition(file);
                def.Parse();
                
                yield return def;
            }
        }

        public override void Parse()
        {
            foreach (var definition in SkinDefinitions ?? Enumerable.Empty<SkinDefinition>())
                definition.Dispose();
            
            skinDefinitions.Clear();
            
            SkinConfig.Parse();
            skinDefinitions.AddRange(GetSkinDefinitions());;
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
    }
}