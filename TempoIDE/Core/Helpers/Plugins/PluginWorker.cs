using System.IO;
using System.Linq;
using TempoIDE.Core.SettingsConfig.Directories.Plugins;

namespace TempoIDE.Core.Helpers.Plugins
{
    public class PluginWorker
    {
        public PluginDirectory PluginDirectory { get; }

        public bool Enabled { get; private set; }
        public bool RequiresRestart => PluginDirectory.SettingOverrides.Overrides.Any();

        public PluginWorker(PluginDirectory pluginDirectory)
        {
            PluginDirectory = pluginDirectory;
        }

        public void Start()
        {
            OverwriteSettings();
            
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public void OverwriteSettings()
        {
            foreach (var settingOverride in PluginDirectory.SettingOverrides.Overrides)
            {
                if (settingOverride.Mode == PluginSettingsFileMode.None)
                    return;
                
                var path = Path.Join(SettingsHelper.Settings.Directory.FullName, settingOverride.RelativePath);
                var file = new FileInfo(path).CreateIfMissing();

                switch (settingOverride.Mode)
                {
                    case PluginSettingsFileMode.Overwrite:
                    {
                        File.WriteAllLines(path, settingOverride.OverridenSettings.Select(s => s.ToString()));
                        break;
                    }
                }
            }
        }

        public void Dispose()
        {
            Disable();
            PluginDirectory.Dispose();
        }
    }
}