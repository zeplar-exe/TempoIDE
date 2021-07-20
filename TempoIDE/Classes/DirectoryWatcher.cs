using System;
using System.IO;
using System.Windows;

namespace TempoIDE.Classes
{
    public class DirectoryWatcher
    {
        public event FileSystemEventHandler Changed;
        
        private FileSystemWatcher watcher;
        private DirectoryInfo directory;

        public DirectoryWatcher(DirectoryInfo directoryInfo, string filter = "")
        {
            directory = directoryInfo;

            watcher = new FileSystemWatcher(directoryInfo.FullName);

            watcher.NotifyFilter = NotifyFilters.Attributes |
                                   NotifyFilters.Security |
                                   NotifyFilters.DirectoryName |
                                   NotifyFilters.FileName;

            watcher.Changed += (sender, e) => Changed?.Invoke(sender, e);
            watcher.Created += (sender, e) => Changed?.Invoke(sender, e);
            watcher.Deleted += (sender, e) => Changed?.Invoke(sender, e);
            watcher.Renamed += (sender, e) => Changed?.Invoke(sender, e);

            watcher.Filter = filter;
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
        }
    }
}