using System.IO;
using TempoIDE.Core.SettingsConfig;
using TempoIDE.Core.Wrappers;

namespace TempoIDE.Core.Helpers
{
    public static class SettingsHelper
    {
        private static bool changed;
        private static DirectoryWatcher watcher;
        
        public static SettingsDirectory Settings { get; private set; }
        
        static SettingsHelper()
        {
            MoveDirectory(AppDataHelper.Directory);
            
            ApplicationHelper.ApplicationTick += On_ApplicationTick;
        }

        public static void MoveDirectory(DirectoryInfo directory)
        {
            watcher?.Dispose();
            
            watcher = new DirectoryWatcher(directory);
            Settings = new SettingsDirectory(directory);
            
            watcher.Changed += DirectoryChanged;
        }

        private static void DirectoryChanged(object sender, FileSystemEventArgs e)
        {
            changed = true;
        }

        private static void On_ApplicationTick(ulong _)
        {
            if (changed == false)
                return;
            
            changed = false;
            
            Settings.Parse();
        }
    }
}