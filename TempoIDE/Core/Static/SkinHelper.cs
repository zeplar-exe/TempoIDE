using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using XamlReader = System.Windows.Markup.XamlReader;

namespace TempoIDE.Core.Static
{
    public static class SkinHelper
    {
        public const string SkinsPath = "data/skins";
        
        public static bool LoadSkin(string skin)
        {
            var path = Path.Join(Directory.GetCurrentDirectory(), SkinsPath, skin);

            if (!File.Exists(path))
            {
                ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_FILE, 
                    $"Could not find the relative file '{Path.Join(SkinsPath, skin)}'. " +
                    "Is your executable in the correct place?");

                return false;
            }
            
            try
            {
                if (IOHelper.TryOpenRelativeFile(Path.Join(SkinsPath, skin), out var stream, out var e))
                {
                    using (stream)
                    {
                        var component = XamlReader.Load(stream);
                        var dict = Application.Current.Resources.MergedDictionaries;

                        dict.Clear();
                        dict.Add(component as ResourceDictionary);
                    }
                }
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
                new Uri(SkinsPath + "DefaultSkin.xaml", UriKind.Relative));
            var dict = Application.Current.Resources.MergedDictionaries;
                
            dict.Clear();
            dict.Add((ResourceDictionary)component);
        }

        public static IEnumerable<FileInfo> GetSkinFiles()
        {
            try
            {
                var directory = new DirectoryInfo(Path.GetRelativePath(Directory.GetCurrentDirectory(), SkinsPath));

                if (!directory.Exists)
                    return Enumerable.Empty<FileInfo>();

                return directory.EnumerateFiles("*.xaml", SearchOption.TopDirectoryOnly);
            }
            catch (DirectoryNotFoundException)
            {
                ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_DIRECTORY, 
                    $"Could not find the relative directory '{SkinsPath}'.\n" +
                    "Is your executable in the correct place?");
                
                return Enumerable.Empty<FileInfo>();
            }
        }
    }
}