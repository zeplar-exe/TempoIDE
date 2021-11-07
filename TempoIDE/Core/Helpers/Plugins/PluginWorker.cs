using System.Linq;
using TempoIDE.Core.SettingsConfig.Directories.Plugins;

namespace TempoIDE.Core.Helpers.Plugins
{
    public class PluginWorker
    {
        public readonly PluginDirectory PluginDirectory;
        
        public bool Enabled { get; private set; }
        public bool RequiresRestart => PluginDirectory.SettingsOverrides.Overrides.Any();

        public PluginWorker(PluginDirectory pluginDirectory)
        {
            PluginDirectory = pluginDirectory;
            PluginDirectory.Parse();
        }

        public void Start()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public void Delete()
        {
            PluginDirectory.Directory.Delete(true);
        }

        private void OnCommandReceived(string command)
        {
            if (!Enabled)
                return;
        }
    }
}