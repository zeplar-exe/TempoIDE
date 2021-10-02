using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using TempoIDE.Properties;

namespace TempoIDE.Core.Helpers
{
    public static class SkinHelper
    {
        public const string SkinsPath = "data\\skins";
        
        public static bool TryLoadSkin(string skin)
        {
            var path = IOHelper.GetRelativePath(Path.Join(SkinsPath, skin));
            
            if (!File.Exists(path))
            {
                ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_FILE, 
                    $"Could not find the relative file '{Path.Join(SkinsPath, skin)}'.\n" +
                    "Is your executable in the correct place?");

                return false;
            }
            
            try
            {
                if (IOHelper.TryReadRelativeFile(Path.Join(SkinsPath, skin), out var stream, out var e))
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
                ApplicationHelper.Logger.Debug(e.Message);
                #endif

                return false;
            }

            Settings.Default.ApplicationSkin = skin;
            Settings.Default.Save();
            
            return true;
        }

        public static void LoadDefaultSkin()
        {
            var component = Application.LoadComponent(
                new Uri(Path.Join(SkinsPath, "DefaultSkin.xaml"), UriKind.Relative));
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