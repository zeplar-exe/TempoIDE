using System;
using System.Collections.Generic;
using System.IO;
using TempoIDE.Core.SettingsConfig.Settings;
using TempoIDE.Core.SettingsConfig.Settings.SettingsFiles;

namespace TempoIDE.Core.SettingsConfig.Directories.Plugins
{
    public class SettingsFileOverride : Config
    {
        private readonly List<Setting> overriddenSettings = new();

        public IReadOnlyCollection<Setting> OverridenSettings => overriddenSettings.AsReadOnly();
        public PluginSettingsFileMode Mode;
        
        public SettingsFileOverride(FileInfo file) : base(file)
        {
            
        }

        public SettingsFileOverride(Stream stream) : base(stream)
        {
            
        }

        public override void Parse()
        {
            foreach (var setting in Document.Settings)
            {
                switch (setting.Key.ToLower())
                {
                    case "mode":
                        if (!Enum.TryParse(setting.Value.ToString(), out Mode))
                            ReportWarning("Invalid mode.", setting.Context);
                        break;
                    default:
                        overriddenSettings.Add(setting);
                        break;
                }
            }
        }

        public override void Write()
        {
            using var writer = new StreamWriter(Stream, leaveOpen: true);
            
            if (Mode != PluginSettingsFileMode.None)
                writer.WriteLineAsync(
                    new Setting("mode", new TextSetting(Mode.ToString()), default).ToString());
            
            foreach (var setting in OverridenSettings)
                writer.WriteLineAsync(setting.ToString());
        }
    }

    public enum PluginSettingsFileMode
    {
        None = 0,
        
        Replace,
        Overwrite
    }
}