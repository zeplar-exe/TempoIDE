using System.Collections.Generic;
using System.IO;
using TempoIDE.Core;
using TempoIDE.Core.Helpers;
using TempoIDE.Plugins.Core;

namespace TempoIDE.Plugins
{
    internal static class PluginHelper
    {
        internal static List<Plugin> loaded = new();
        
        public const string PluginPath = "data/plugins";
        
        public static void LoadPlugins()
        {
            if (!IOHelper.RelativeDirectoryExists(PluginPath))
            {
                ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_DIRECTORY, 
                    $"Could not find the relative directory '{PluginPath}'.\n" +
                    "Is your executable in the correct place?");
                
                return;
            }
            
            var info = new DirectoryInfo(IOHelper.GetRelativePath(PluginPath));
            
            foreach (var plugin in info.EnumerateFiles("*.plugin", SearchOption.AllDirectories))
            {
                using var stream = new PluginStream(plugin.OpenRead());
                stream.Parse();

                if (!stream.TryGetDirectoryPath(out var directoryPath))
                {
                    ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_PLUGIN,
                        $"Failed to load the plugin '{plugin.Name}'.\n" +
                        "Its directory is missing or invalid.");
                    
                    return;
                }

                var dllPath = Path.Join(directoryPath, stream.PluginName + ".dll");

                if (!File.Exists(dllPath))
                {
                    ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_PLUGIN,
                        $"Failed to load the plugin dll '{dllPath}.dll' because it does not exist.");
                    
                    return;
                }

                try
                {
                    //loaded.Add(Plugin.ReflectPluginFromAssembly(dllPath));
                }
                catch (InvalidPluginPackageException)
                {
                    throw;
                }
                catch (NoAttacherException)
                {
                    throw;
                }
                catch (TooManyAttachersException)
                {
                    throw;
                }
                catch (NoAttachMethodException)
                {
                    throw;
                }
                catch (InvalidAttacherReturnException)
                {
                    throw;
                }
            }
        }
    }
}