using System.IO;

namespace TempoIDE.Core.SettingsConfig
{
    public class ExplorerSettings
    {
        public static ExplorerSettings Create(DirectoryInfo directory)
        {
            return new ExplorerSettings();
        }
    }
}