using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TempoIDE.Core.SettingsConfig.Settings;
using TempoIDE.Core.SettingsConfig.Settings.SettingsFiles;

namespace TempoIDE.Core.SettingsConfig.Directories.Plugins
{
    public class SettingsFileOverride : Config
    {
        public IEnumerable<Setting> OverridenSettings { get; private set; } = Enumerable.Empty<Setting>();
        
        public string RelativePath { get; }
        public PluginSettingsFileMode Mode { get; private set; } = PluginSettingsFileMode.Overwrite;
        
        public SettingsFileOverride(FileInfo file, string relativePath) : base(file)
        {
            RelativePath = relativePath;
            
            Parse();
        }

        public SettingsFileOverride(Stream stream) : base(stream)
        {
            Parse();
        }

        private void Parse()
        {
            var settings = new List<Setting>();
            
            foreach (var setting in Document.Settings)
            {
                switch (setting.Key.ToLower())
                {
                    case "mode":
                        if (!Enum.TryParse(setting.Value.ToString(), out PluginSettingsFileMode tempMode))
                            ReportWarning("Invalid mode.", setting.Context);
                        Mode = tempMode;
                        break;
                    default:
                        settings.Add(setting);
                        break;
                }
            }

            OverridenSettings = settings;
        }

        public override void Write()
        {
            using var writer = CreateWriter();
            
            writer.WriteLineAsync(new Setting("mode", new TextSetting(Mode.ToString()), default).ToString());
            
            foreach (var setting in OverridenSettings)
                writer.WriteLineAsync(setting.ToString());
        }
    }

    public enum PluginSettingsFileMode
    {
        None = 0,
        
        Overwrite
    }
}