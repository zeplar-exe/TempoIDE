using System.IO;
using System.Threading.Tasks;
using TempoIDE.Controls.Panels;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.Wrappers;

namespace TempoIDE.Core.Environments
{
    public class DirectoryEnvironment : DevelopmentEnvironment
    {
        public DirectoryEnvironment(DirectoryInfo directory) : base(directory, new DirectoryWatcher(directory))
        {
            
        }

        public override async void CacheFiles()
        {
            var info = (DirectoryInfo)EnvironmentPath;

            await Task.Run(delegate
            {
                foreach (var file in info.EnumerateFiles("*", SearchOption.AllDirectories))
                    Cache.AddFile(file);
            });
        }

        public override void RefreshCache()
        {
            Cache.Clear();
            CacheFiles();
        }

        public override void LoadExplorer(ExplorerView explorer)
        {
            explorer.AppendDirectory((DirectoryInfo)EnvironmentPath);
        }

        protected override void DirectoryChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                
            }
            
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    Cache.AddFile(new FileInfo(e.FullPath));
                    
                    break;
                case WatcherChangeTypes.Renamed:
                    var renamedArgs = (RenamedEventArgs) e;
                    
                    Cache.RemoveFile(new FileInfo(renamedArgs.OldFullPath));
                    Cache.AddFile(new FileInfo(renamedArgs.FullPath));
                    
                    break;
                case WatcherChangeTypes.Deleted:
                    if (e.FullPath == EnvironmentPath.FullName)
                        EnvironmentHelper.CloseEnvironment();
                    else
                        Cache.RemoveFile(new FileInfo(e.FullPath));

                    break;
                case WatcherChangeTypes.Changed:
                    Cache.AddFile(new FileInfo(e.FullPath));
                    
                    break;
            }
        }
    }
}