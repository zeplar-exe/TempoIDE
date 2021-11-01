using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.SettingsConfig.Settings.SettingsFiles;

namespace TempoIDE.Core.SettingsConfig
{
    public class SkinSettings : SettingDirectoryWrapper
    {
        public readonly SkinConfig SkinConfig;
        public IEnumerable<SkinDefinition> SkinDefinitions { get; private set; }
        
        public SkinSettings(DirectoryInfo directory) : base(directory)
        {
            SkinConfig = new SkinConfig(Directory.ToFile("skin.txt").CreateIfMissing());
            SkinDefinitions = GetSkinDefinitions(Directory);
        }
        
        private static IEnumerable<SkinDefinition> GetSkinDefinitions(DirectoryInfo appDirectory)
        {
            return appDirectory
                .ToRelativeDirectory("skins").CreateIfMissing()
                .GetFiles("*.txt", SearchOption.AllDirectories)
                .Select(f => new SkinDefinition(f));
        }

        public override void Parse()
        {
            foreach (var definition in SkinDefinitions)
                definition.Dispose();
            
            SkinConfig.Parse();
            SkinDefinitions = GetSkinDefinitions(Directory);
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