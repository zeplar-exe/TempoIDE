using System;
using System.Globalization;
using System.Windows;
using TempoIDE.Core.SettingsConfig.Settings.SettingsFiles;
using TempoIDE.Core.Wrappers;

namespace TempoIDE.Core.SettingsConfig.Converters
{
    public class SkinConverter : ValueConverter<SkinDefinition, ResourceDictionary>
    {
        public override ResourceDictionary Convert(SkinDefinition value, object parameter, CultureInfo culture)
        {
            var dictionary = new ResourceDictionary
            {
                { nameof(value.PrimaryBackgroundColor), value.PrimaryBackgroundColor },
                { nameof(value.SecondaryBackgroundColor), value.SecondaryBackgroundColor },
                { nameof(value.PrimaryBorderColor), value.PrimaryBorderColor },
                { nameof(value.SecondaryBorderColor), value.SecondaryBorderColor },
                { nameof(value.ToolbarBackgroundColor), value.ToolbarBackgroundColor },
                { nameof(value.TextForegroundColor), value.TextForegroundColor },
                { nameof(value.TextBoxBackgroundColor), value.TextBoxBackgroundColor },
                { nameof(value.SeparatorColor), value.SeparatorColor },
                { nameof(value.AcceptButtonColor), value.AcceptButtonColor },
                { nameof(value.DeclineButtonColor), value.DeclineButtonColor },
                { nameof(value.NotificationColor), value.NotificationColor }
            };

            return dictionary;
        }

        public override SkinDefinition ConvertBack(ResourceDictionary value, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}