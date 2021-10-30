using System.IO;

namespace TempoIDE.Core.SettingsConfig
{
    public class EditorSettings
    {
        public static EditorSettings Create(DirectoryInfo directory)
        {
            return new EditorSettings();
        }
    }
}