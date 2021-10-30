using System.IO;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.SettingsConfig.Internal;
using TempoIDE.Core.SettingsConfig.Internal.Parser;
using TempoIDE.Core.SettingsConfig.Settings;
using TempoIDE.Properties;

namespace TempoIDE.Core.SettingsConfig
{
    public class SettingsDirectory
    {
        public readonly AppSettings AppSettings;
        public readonly EditorSettings EditorSettings;
        public readonly ExplorerSettings ExplorerSettings;

        private SettingsDirectory(AppSettings appSettings, EditorSettings editorSettings, ExplorerSettings explorerSettings)
        {
            AppSettings = appSettings;
            EditorSettings = editorSettings;
            ExplorerSettings = explorerSettings;
        }

        public static SettingsDirectory Create(DirectoryInfo root)
        {
            return new SettingsDirectory(
                AppSettings.Create(root.ToNestedDirectory("app")),
                EditorSettings.Create(root.ToNestedDirectory("editor")),
                ExplorerSettings.Create(root.ToNestedDirectory("explorer")));
        }
    }
}