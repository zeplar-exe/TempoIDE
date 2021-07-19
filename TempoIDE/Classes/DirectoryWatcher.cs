using System;
using System.IO;
using System.Windows;

namespace TempoIDE.Classes
{
    public class DirectoryWatcher
    {
        public event RoutedEventHandler Changed;
        
        private FileSystemWatcher watcher;
        private DirectoryInfo directory;

        public DirectoryWatcher(DirectoryInfo directoryInfo)
        {
            directory = directoryInfo;

            watcher = new FileSystemWatcher(directoryInfo.FullName);

            watcher.NotifyFilter = NotifyFilters.Attributes |
                                   NotifyFilters.Security |
                                   NotifyFilters.DirectoryName |
                                   NotifyFilters.FileName;

            watcher.Changed += delegate { Changed?.Invoke(this, default); };
            watcher.Created += delegate { Changed?.Invoke(this, default); };
            watcher.Deleted += delegate { Changed?.Invoke(this, default); };
            watcher.Renamed += delegate { Changed?.Invoke(this, default); };

            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
        }
    }
}