using System.IO;

namespace TempoIDE.Core.SettingsConfig.Settings.SettingsFiles
{
    public sealed class SkinConfig : Config
    {
        public const string DefaultSkinIdentifier = "_default";
        
        public string CurrentSkin { get; private set; } = DefaultSkinIdentifier;
        public string PreviousSkin { get; private set; } 
        
        public SkinConfig(FileInfo file) : base(file) { }
        public SkinConfig(Stream stream) : base(stream) { }

        public override void Parse()
        {
            foreach (var setting in Document.Settings)
            {
                if (ReportIfUnexpectedSettingType(setting, out TextSetting text))
                    break;
                
                switch (setting.Key.ToLower())
                {
                    case "previous_skin":
                        PreviousSkin = text.ToString();
                        break;
                    case "current_skin":
                        CurrentSkin = text.ToString() ?? DefaultSkinIdentifier;
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
            using var writer = CreateWriter();
            
            if (!string.IsNullOrEmpty(PreviousSkin))
                writer.WriteLineAsync(Setting.Create("previous_skin", PreviousSkin).ToFullString());
            
            writer.WriteAsync(Setting.Create("current_skin", CurrentSkin).ToFullString());
        }

        public void SetSkin(SkinDefinition definition)
        {
            if (definition.Name == CurrentSkin)
                return;
            
            PreviousSkin = CurrentSkin;
            CurrentSkin = definition.Name;
        }
    }
}