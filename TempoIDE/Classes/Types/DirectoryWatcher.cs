using System.IO;

namespace TempoIDE.Classes.Types
{
    public class DirectoryWatcher
    {
        public event FileSystemEventHandler Changed;
        
        private FileSystemWatcher watcher;

        public DirectoryWatcher(DirectoryInfo directory, string filter = "*")
        {
            watcher = new FileSystemWatcher(directory.FullName);

            watcher.NotifyFilter = NotifyFilters.Attributes |
                                   NotifyFilters.Security |
                                   NotifyFilters.DirectoryName |
                                   NotifyFilters.FileName |
                                   NotifyFilters.LastWrite | 
                                   NotifyFilters.CreationTime;
            
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