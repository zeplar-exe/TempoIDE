using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace TempoIDE.Core.Static
{
    public static class SkinHelper
    {
        public const string SkinsUri = "./data/skins";
        
        public static bool LoadSkin(string skin)
        {
            var path = new Uri($"{SkinsUri}/{skin}", UriKind.Relative);

            try
            {
                var component = Application.LoadComponent(path);
                var dict = Application.Current.Resources.MergedDictionaries;
                
                dict.Clear();
                dict.Add(component as ResourceDictionary);
            }
            catch (Exception e)
            {
                ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_SKIN, 
                    $"The skin '{skin}' failed to load because it is not a valid xaml file");
                
                #if DEBUG
                Console.WriteLine(e.Message);
                #endif

                return false;
            }

            return true;
        }

        public static void LoadDefaultSkin()
        {
            var component = Application.LoadComponent(new Uri("./DefaultSkin.xaml"));
            var dict = Application.Current.Resources.MergedDictionaries;
                
            dict.Clear();
            dict.Add((ResourceDictionary)component);
        }

        public static IEnumerable<FileInfo> GetSkinFiles()
        {
            var directory = new DirectoryInfo(SkinsUri);
            
            if (!directory.Exists)
                return Enumerable.Empty<FileInfo>();

            return directory.EnumerateFiles("*.xaml", SearchOption.TopDirectoryOnly);
        }
    }
}