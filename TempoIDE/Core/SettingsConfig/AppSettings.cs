using System.IO;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.SettingsConfig.Settings.SettingsFiles;

namespace TempoIDE.Core.SettingsConfig
{
    public class AppSettings
    {
        public readonly SkinConfig SkinConfig;

        public AppSettings(SkinConfig skinConfig)
        {
            SkinConfig = skinConfig;
        }

        public static AppSettings Create(DirectoryInfo directory)
        {
            return new AppSettings(
                new SkinConfig(directory.ToFile("skin.txt")));
        }
    }
}