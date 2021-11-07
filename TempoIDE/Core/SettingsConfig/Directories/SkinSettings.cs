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
        public SkinDefinition[] SkinDefinitions { get; private set; }
        
        public SkinSettings(DirectoryInfo directory) : base(directory)
        {
            SkinConfig = new SkinConfig(Directory.ToFile("skin.txt").CreateIfMissing());
        }

        public override void Parse()
        {
            foreach (var definition in SkinDefinitions ?? Enumerable.Empty<SkinDefinition>())
                definition.Dispose();
            
            SkinConfig.Parse();
            SkinDefinitions = GetSkinDefinitions();
        }
        
        private SkinDefinition[] GetSkinDefinitions()
        {
            var list = new List<SkinDefinition>();
            
            foreach (var file in Directory
                .ToRelativeDirectory("skins").CreateIfMissing()
                .EnumerateFiles("*.txt", SearchOption.AllDirectories))
            {
                var def = new SkinDefinition(file);
                def.Parse();
                
                list.Add(def);
            }

            return list.ToArray();
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