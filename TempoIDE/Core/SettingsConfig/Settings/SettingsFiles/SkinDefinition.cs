using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace TempoIDE.Core.SettingsConfig.Settings.SettingsFiles
{
    public sealed class SkinDefinition : Config
    {
        public string Name { get; }
        
        public Brush PrimaryBackgroundColor;
        public Brush SecondaryBackgroundColor;
        public Brush PrimaryBorderColor;
        public Brush SecondaryBorderColor;
        public Brush ToolbarBackgroundColor;
        public Brush TextForegroundColor;
        public Brush TextBoxBackgroundColor;
        public Brush SeparatorColor;
        public Brush AcceptButtonColor;
        public Brush DeclineButtonColor;
        public Brush NotificationColor;
        
        public SkinDefinition(FileInfo file) : base(file)
        {
            Name = Path.GetFileNameWithoutExtension(FilePath);
            
            Parse();
        }

        public SkinDefinition(Stream stream, string name) : base(stream)
        {
            Name = name;
            
            Parse();
        }

        private void Parse()
        {
            foreach (var setting in Document.Settings)
            {
                if (ReportIfUnexpectedSettingType(setting, out TextSetting _))
                    continue;
                
                switch (setting.Key.ToLower())
                {
                    case "primary_background_color":
                        PrimaryBackgroundColor = FromSetting(setting);
                        break;
                    case "secondary_background_color":
                        SecondaryBackgroundColor = FromSetting(setting);
                        break;
                    case "primary_border_color":
                        PrimaryBorderColor = FromSetting(setting);
                        break;
                    case "secondary_border_color":
                        SecondaryBorderColor = FromSetting(setting);
                        break;
                    case "toolbar_background_color":
                        ToolbarBackgroundColor = FromSetting(setting);
                        break;
                    case "text_foreground_color":
                        TextForegroundColor = FromSetting(setting);
                        break;
                    case "textbox_background_color":
                        TextBoxBackgroundColor = FromSetting(setting);
                        break;
                    case "separator_color":
                        SeparatorColor = FromSetting(setting);
                        break;
                    case "accept_button_color":
                        AcceptButtonColor = FromSetting(setting);
                        break;
                    case "decline_button_color":
                        DeclineButtonColor = FromSetting(setting);
                        break;
                    case "notification_color":
                        NotificationColor = FromSetting(setting);
                        break;
                    default:
                        ReportUnexpectedSetting(setting);
                        continue;
                }
                
                ReportIfEmptySetting(setting);
                // If it's a valid setting, we check if it's empty
            }
        }

        private static Brush FromSetting(Setting setting)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFromString(setting.Value.ToString());
        }

        public override void Write()
        {
            using var writer = CreateWriter();
            
            writer.WriteLine(CreateSetting(PrimaryBackgroundColor));
            writer.WriteLine(CreateSetting(SecondaryBackgroundColor));
            writer.WriteLine(CreateSetting(PrimaryBorderColor));
            writer.WriteLine(CreateSetting(SecondaryBorderColor));
            writer.WriteLine(CreateSetting(ToolbarBackgroundColor));
            writer.WriteLine(CreateSetting(TextForegroundColor));
            writer.WriteLine($"textbox_background_color={TextBoxBackgroundColor}");
            writer.WriteLine(CreateSetting(SeparatorColor));
            writer.WriteLine(CreateSetting(AcceptButtonColor));
            writer.WriteLine(CreateSetting(DeclineButtonColor));
            writer.WriteLine(CreateSetting(NotificationColor));
        }

        private static string CreateSetting(Brush field)
        {
            var builder = new StringBuilder();
            
            builder.Append(CreateKey(field));
            builder.Append(field);
            builder.AppendLine();

            return builder.ToString();
        }

        private static string CreateKey(Brush field)
        {
            var builder = new StringBuilder();
            var split = Regex.Split(nameof(field), @"(?<!^)(?=[A-Z])");

            builder.AppendJoin('_', split);

            return builder.ToString();
        }
    }
}