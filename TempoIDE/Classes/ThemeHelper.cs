using System;
using System.Windows;

namespace TempoIDE.Classes
{
    public static class ThemeHelper
    {
        public static string SkinsUri = "/Skins/";
        
        public static void LoadTheme(Theme theme)
        {
            var themePath = new Uri(SkinsUri + theme.ToString() + ".xaml", UriKind.Relative);
            
            App.Current.Resources.MergedDictionaries[0] = Application.LoadComponent(themePath) as ResourceDictionary;
        }
    }

    public enum Theme
    {
        Light,
        Dark
    }
}