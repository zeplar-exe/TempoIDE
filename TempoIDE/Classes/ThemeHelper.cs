using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace TempoIDE.Classes
{
    public static class ThemeHelper
    {
        public static string SkinsUri = "/Skins/";
        
        public static void LoadTheme(Theme theme)
        {
            var themePath = new Uri(SkinsUri + theme + ".xaml", UriKind.Relative);
            
            App.Current.Resources.MergedDictionaries[0] = Application.LoadComponent(themePath) as ResourceDictionary;
        }
    }

    public enum Theme
    {
        Light,
        Dark
    }
}