using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xaml;
using XamlReader = System.Windows.Markup.XamlReader;

namespace TempoIDE.Core.Static
{
    public static class SkinHelper
    {
        public const string SkinsUri = "data/skins";
        
        public static bool LoadSkin(string skin)
        {
            var path = Path.Join(Directory.GetCurrentDirectory(), SkinsUri, skin);

            try
            {
                var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);

                var component = XamlReader.Load(stream);
                var dict = Application.Current.Resources.MergedDictionaries;
                
                dict.Clear();
                dict.Add(component as ResourceDictionary);
            }
            catch (Exception e)
            {
                ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_SKIN, 
                    $"The skin '{skin}' failed to load because it is not a valid xaml file.");
                
                #if DEBUG
                Console.WriteLine(e.Message);
                #endif

                return false;
            }

            return true;
        }

        public static void LoadDefaultSkin()
        {
            var component = Application.LoadComponent(
                new Uri("Core/Static/DefaultSkin.xaml", UriKind.Relative));
            var dict = Application.Current.Resources.MergedDictionaries;
                
            dict.Clear();
            dict.Add((ResourceDictionary)component);
        }

        public static IEnumerable<FileInfo> GetSkinFiles()
        {
            var directory = new DirectoryInfo(Path.GetRelativePath(Directory.GetCurrentDirectory(), SkinsUri));
            
            if (!directory.Exists)
                return Enumerable.Empty<FileInfo>();

            return directory.EnumerateFiles("*.xaml", SearchOption.TopDirectoryOnly);
        }
    }
}