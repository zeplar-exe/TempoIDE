using System.IO;

namespace TempoIDE.Core.SettingsConfig.Settings.SettingsFiles
{
    public sealed class SkinConfig : Config
    {
        public const string DefaultSkinIdentifier = "_default";
        
        public string CurrentSkin = DefaultSkinIdentifier;
        public string PreviousSkin; 
        
        public SkinConfig(FileInfo file) : base(file) { }
        public SkinConfig(Stream stream) : base(stream) { }

        public override void Parse()
        {
            foreach (var setting in EnumerateSettings())
            {
                switch (setting.Key.ToLower())
                {
                    case "previous_skin":
                        PreviousSkin = setting.Value.ToString();
                        break;
                    case "current_skin":
                        CurrentSkin = setting.Value.ToString();
                        ReportIfEmptySetting(setting);
                        
                        break;
                    default:
                        ReportUnexpectedSetting(setting);
                        break;
                }
            }
        }

        public override void Write()
        {
            using var writer = new StreamWriter(Stream, leaveOpen: true);
            
            if (!string.IsNullOrEmpty(PreviousSkin))
                writer.WriteLineAsync($"previous_skin={PreviousSkin}");
            
            writer.WriteAsync($"current_skin={CurrentSkin}");
        }

        public void SetSkin(SkinDefinition definition)
        {
            PreviousSkin = CurrentSkin;
            CurrentSkin = definition.Name;
        }
    }
}