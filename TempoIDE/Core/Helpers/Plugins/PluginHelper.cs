using System.Collections.Generic;
using System.Linq;
using TempoIDE.Core.SettingsConfig.Directories.Plugins;

namespace TempoIDE.Core.Helpers.Plugins
{
    public static class PluginHelper
    {
        private static readonly List<PluginWorker> loadedPlugins = new();

        public static IEnumerable<PluginWorker> LoadedPlugins => loadedPlugins.AsReadOnly();
        public static IEnumerable<PluginWorker> EnabledPlugins => LoadedPlugins.Where(t => t.Enabled);
        public static IEnumerable<PluginWorker> DisabledPlugins => LoadedPlugins.Where(t => !t.Enabled);

        public static void LoadPluginsUsingDiff()
        {
            var pluginDirectory = AppDataHelper.Directory.ToRelativeDirectory("plugins").CreateIfMissing();
            var directories = pluginDirectory.EnumerateDirectories().ToArray();

            foreach (var loaded in loadedPlugins)
            {
                if (directories.All(d => !d.EqualsOther(loaded.PluginDirectory.Directory)))
                    loadedPlugins.Remove(loaded);
            }

            foreach (var directory in pluginDirectory.EnumerateDirectories())
            {
                if (loadedPlugins.All(p => !p.PluginDirectory.Directory.EqualsOther(directory)))
                    loadedPlugins.Add(new PluginWorker(new PluginDirectory(directory)));
            }
        }
        
        public static bool DownloadPlugin(PluginInfo pluginInfo)
        {
            return false;
        }
        
        public static bool UninstallPlugin(PluginWorker worker)
        {
            var index = loadedPlugins.IndexOf(worker);
            
            if (index == -1)
                return false;

            loadedPlugins.RemoveAt(index);
            
            worker.Delete();

            return true;
        }
    }
}