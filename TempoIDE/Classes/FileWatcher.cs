using System.IO;

namespace TempoIDE.Classes
{
    public static class FileWatcher
    {
        public static FileSystemWatcher currentWatcher { get; private set; }

        public static FileSystemEventHandler DirectoryRenamed;
        public static FileSystemEventHandler SubDirectoryRenamed;
        public static FileSystemEventHandler FileRenamed;
        public static FileSystemEventHandler FileChanged;
        public static FileSystemEventHandler FileDeleted;

        public static void Start(DirectoryInfo directory)
        {
            currentWatcher?.Dispose();
            
            currentWatcher = new FileSystemWatcher
            {
                Path = directory.FullName,
                NotifyFilter =
                    NotifyFilters.DirectoryName |
                    NotifyFilters.LastWrite |
                    NotifyFilters.FileName,
                IncludeSubdirectories = true,
                Filter = "*.*",
                EnableRaisingEvents = true,
            };

            // TODO: Finish this if the 5 second cooldown goes south
        }
    }
}