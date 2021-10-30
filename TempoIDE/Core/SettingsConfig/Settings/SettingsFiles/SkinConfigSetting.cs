using System.IO;

namespace TempoIDE.Core.SettingsConfig.Settings.SettingsFiles
{
    public readonly struct SkinConfigSetting
    {
        public readonly string GivenName;
        public readonly FileInfo SkinFile;

        public SkinConfigSetting(string givenName, FileInfo skinFile)
        {
            GivenName = givenName;
            SkinFile = skinFile;
        }
    }
}