using System.Collections.Generic;
using System.IO;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.SettingsConfig.Settings.SettingsFiles;

namespace TempoIDE.Core.SettingsConfig
{
    public class AppSettings : SettingDirectoryWrapper
    {
        public readonly SkinSettings SkinSettings;

        public AppSettings(DirectoryInfo directory) : base(directory)
        {
            SkinSettings = new SkinSettings(directory.ToRelativeDirectory("skins").CreateIfMissing());
        }
        
        public override void Parse()
        {
            SkinSettings.Parse();
        }

        public override void Write()
        {
            SkinSettings.Write();
        }
    }
}