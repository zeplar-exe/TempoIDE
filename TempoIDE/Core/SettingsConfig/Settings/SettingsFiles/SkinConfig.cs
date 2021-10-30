using System.IO;
using TempoIDE.Core.SettingsConfig.Internal.Parser;
using TempoIDE.Core.SettingsConfig.Settings.Exceptions;

namespace TempoIDE.Core.SettingsConfig.Settings.SettingsFiles
{
    public sealed class SkinConfig : Config
    {
        public string CurrentSkin = "_default";
        public string PreviousSkin; 
        
        public SkinConfig(FileInfo file) : base(file) { Parse(); }
        public SkinConfig(Stream stream) : base(stream) { Parse(); }

        public override void Parse()
        {
            var settings = new SettingsParser(Stream).ParseSettings();
            
            foreach (var setting in settings)
            {
                switch (setting.Key)
                {
                    case "previous_skin":
                        PreviousSkin = setting.Value.ToString();
                        break;
                    case "current_skin":
                        CurrentSkin = setting.Value.ToString();
                        
                        if (string.IsNullOrEmpty(CurrentSkin))
                            ReportEmptySetting(setting);
                        
                        break;
                    default:
                        ReportUnexpectedSetting(setting);
                        break;
                }
            }
        }

        public override void Write()
        {
            Stream.Seek(0, SeekOrigin.Begin);

            using var writer = new StreamWriter(Stream, leaveOpen: true);
            
            if (!string.IsNullOrEmpty(PreviousSkin))
                writer.WriteLine($"previous_skin={PreviousSkin}");
            
            writer.WriteAsync($"current_skin={CurrentSkin}");
        }
    }
}