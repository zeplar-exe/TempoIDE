using System;
using System.IO;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.SettingsConfig.Directories;
using TempoIDE.Core.Wrappers;

namespace TempoIDE.Core.SettingsConfig
{
    public static class SettingsHelper
    {
        private static bool changed;
        private static DirectoryWatcher watcher;
        
        public static SettingsDirectory Settings { get; private set; }

        public static event EventHandler SettingsUpdated;
        
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
            
            Settings.Parse();
            SettingsUpdated?.Invoke(default, EventArgs.Empty);
            
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
            SettingsUpdated?.Invoke(default, EventArgs.Empty);
        }
    }
}