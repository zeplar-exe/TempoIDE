using System.IO;
using TempoIDE.Core.Static;
using TempoIDE.Core.Types;
using TempoIDE.UserControls.Panels;

namespace TempoIDE.Core.Environments
{
    public abstract class DevelopmentEnvironment
    {
        public readonly EnvironmentCache Cache = new();

        public readonly FileSystemInfo EnvironmentPath;
        public readonly DirectoryWatcher DirectoryWatcher;
        
        protected const string ConfigDirectoryName = ".TempoConfig";

        private TempoConfigStream configStream;
        public TempoConfigStream ConfigStream
        {
            get
            {
                if (configStream != null)
                    return configStream;

                var currentPath = EnvironmentPath is DirectoryInfo ? 
                    EnvironmentPath.FullName : 
                    ((FileInfo)EnvironmentPath).Directory.FullName;

                var directoryPath = Path.Join(currentPath, ConfigDirectoryName);

                if (Directory.Exists(directoryPath))
                {
                    var existingStream = new TempoConfigStream(directoryPath);
                    existingStream.Parse();

                    return configStream = existingStream;
                }

                var directory = Directory.CreateDirectory(directoryPath);

                var stream = new TempoConfigStream(directory.FullName);
                stream.Write();

                return configStream = stream;
            }
        }

        public DevelopmentEnvironment(FileSystemInfo path, DirectoryWatcher watcher)
        {
            EnvironmentPath = path;
            DirectoryWatcher = watcher;

            DirectoryWatcher.Changed += DirectoryChanged;
        }

        public abstract void CacheFiles();
        public abstract void RefreshCache();
        public abstract void LoadExplorer(ExplorerView explorer);
        protected abstract void DirectoryChanged(object sender, FileSystemEventArgs e);

        public virtual void Close()
        {
            DirectoryWatcher.Dispose();
            Cache.Clear();
        }
    }
}