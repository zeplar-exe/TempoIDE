using System.IO;
using TempoIDE.Core.SettingsConfig.Directories.Plugins;

namespace TempoIDE.Core.Helpers.Plugins
{
    public static class PluginsHelper
    {
        public static PluginsDirectory Plugins;

        public static void MoveDirectory(DirectoryInfo directory)
        {
            Plugins = new PluginsDirectory(directory);
        }
    }
}