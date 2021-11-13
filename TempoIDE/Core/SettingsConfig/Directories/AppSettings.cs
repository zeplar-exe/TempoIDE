using System.IO;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Core.SettingsConfig.Directories
{
    public class AppSettings : SettingDirectoryWrapper
    {
        public readonly SkinSettings SkinSettings;

        public AppSettings(DirectoryInfo directory) : base(directory)
        {
            SkinSettings = new SkinSettings(directory.ToRelativeDirectory("skins").CreateIfMissing());
        }

        public override void Write()
        {
            SkinSettings.Write();
        }

        public override void Dispose()
        {
            SkinSettings?.Dispose();
        }
    }
}