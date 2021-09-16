using System;
using System.Collections.Generic;
using System.IO;
using TempoIDE.Core.Static;
using TempoPlugins;

namespace TempoIDE.Core.Plugins
{
    public static class PluginHelper
    {
        private static List<Plugin> loaded = new();
        
        public const string PluginPath = "data/plugins";
        
        public static void LoadPlugins()
        {
            if (!IOHelper.RelativeDirectoryExists(PluginPath))
            {
                ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_DIRECTORY, 
                    $"Could not find the relative directory '{PluginPath}'.\n" +
                    "Is your executable in the correct place?");
            }

            var info = new DirectoryInfo(IOHelper.GetRelativePath(PluginPath));
            
            foreach (var plugin in info.EnumerateFiles("*.plugin", SearchOption.AllDirectories))
            {
                using var stream = PluginParser.Parse(IOHelper.ReadFullStream(plugin.OpenRead()));

                if (!stream.TryGetDirectoryPath(out var directoryPath))
                {
                    ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_PLUGIN,
                        $"Failed to load the plugin '{plugin.Name}'.\n" +
                        "Its directory is missing or invalid.");
                }

                var dllPath = Path.Join(directoryPath, stream.PluginName);

                if (!File.Exists(dllPath))
                {
                    ApplicationHelper.EmitErrorCode(ApplicationErrorCode.TI_INVALID_PLUGIN,
                        $"Failed to load the plugin dll '{dllPath}.dll' because it does not exist.");
                }

                try
                {
                    var reflectedPlugin = Plugin.ReflectPluginFromAssembly(dllPath);
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