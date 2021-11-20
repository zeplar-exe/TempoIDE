using System;
using System.IO;
using TempoIDE.Core.SettingsConfig.Directories;
using TempoIDE.Core.Wrappers;

namespace TempoIDE.Core.Helpers
{
    public static class SettingsHelper
    {
        private static bool changed;
        private static DirectoryInfo directoryInfo;
        private static DirectoryWatcher watcher;
        
        public static SettingsDirectory Settings { get; private set; }

        public static event EventHandler SettingsUpdated;

        public static void Start()
        {
            ApplicationHelper.ApplicationTick += On_ApplicationTick;
        }

        public static void MoveDirectory(DirectoryInfo directory)
        {
            directoryInfo = directory;
            
            watcher?.Dispose();
            watcher = new DirectoryWatcher(directory);
            
            Update();
            
            watcher.Changed += DirectoryChanged;
        }

        public static void Update()
        {
            ApplicationHelper.AppDispatcher.Invoke(delegate
            {
                Settings = new SettingsDirectory(directoryInfo);
            }); // Required because the program bitches about thread ownership
            
            SettingsUpdated?.Invoke(default, EventArgs.Empty);
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
            
            Update();
        }
    }
}