using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using TempoIDE.Core.SettingsConfig;
using TempoIDE.Core.SettingsConfig.Converters;
using TempoIDE.Core.SettingsConfig.Settings.SettingsFiles;

namespace TempoIDE.Core.Helpers
{
    public static class SkinHelper
    {
        public static SkinDefinition CurrentSkin
        {
            get
            {
                var skinSettings = SettingsHelper.Settings.AppSettings.SkinSettings;
                var skinName = skinSettings.SkinConfig.CurrentSkin;

                if (skinName == SkinConfig.DefaultSkinIdentifier)
                {
                    return GetDefaultSkin();
                }
                
                return skinSettings.SkinDefinitions.FirstOrDefault(skin => skin.Name == skinName);
            }
        }
        
        public static bool TryLoadSkin(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            var skinSettings = SettingsHelper.Settings.AppSettings.SkinSettings;

            if (name == SkinConfig.DefaultSkinIdentifier)
            {
                LoadDefaultSkin();
                
                return true;
            }
            
            if (!skinSettings.TryGetSkin(name, out var definition))
            {
                ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_SKIN_NAME, 
                    $"Could not find the requested skin '{name}'.\n");
                
                return false;
            }

            var converter = new SkinConverter();
            var resource = converter.Convert(definition, null, CultureInfo.CurrentCulture);
            
            var dict = Application.Current.Resources.MergedDictionaries;

            dict.Clear();
            dict.Add(resource);

            skinSettings.SkinConfig.SetSkin(definition);
            skinSettings.SkinConfig.Write();
            
            return true;
        }

        public static void LoadDefaultSkin()
        {
            var settings = SettingsHelper.Settings.AppSettings.SkinSettings;
            var skin = GetDefaultSkin();
            var dict = Application.Current.Resources.MergedDictionaries;
            var converter = new SkinConverter();

            dict.Clear();
            dict.Add(converter.Convert(skin, null, CultureInfo.CurrentCulture));

            if (settings.SkinConfig.CurrentSkin == SkinConfig.DefaultSkinIdentifier) 
                return;
            
            settings.SkinConfig.SetSkin(skin);
            settings.SkinConfig.Write();
        }

        public static SkinDefinition GetDefaultSkin()
        {
            var definition = new SkinDefinition(
                ResourceHelper.GetResourceFile("default_skin.txt"), SkinConfig.DefaultSkinIdentifier);
            definition.Parse();

            return definition;
        }
    }
}