using System.IO;
using TempoIDE.Core.Static;
using TempoIDE.Core.Types;
using TempoIDE.UserControls.Panels;

namespace TempoIDE.Core.Environments
{
    public class FileEnvironment : DevelopmentEnvironment
    {
        public FileEnvironment(FileInfo path) : base(path, new DirectoryWatcher(path.Directory, path.Name))
        {
            
        }

        public override void CacheFiles()
        {
            Cache.AddFile((FileInfo)EnvironmentPath);
        }

        public override void RefreshCache()
        {
            Cache.Clear();
            CacheFiles();
        }

        public override void LoadExplorer(ExplorerView explorer)
        {
            var info = (FileInfo) EnvironmentPath;

            explorer.AppendElement(new ExplorerFileSystemItem(info.FullName));
            ApplicationHelper.MainWindow.Editor.Tabs.Open(info);
        }

        protected override void DirectoryChanged(object sender, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Deleted:
                    if (e.FullPath == EnvironmentPath.FullName)
                        EnvironmentHelper.CloseEnvironment();
                    
                    break;
                case WatcherChangeTypes.Changed:
                    if (e.FullPath == EnvironmentPath.FullName)
                        Cache.AddFile(new FileInfo(e.FullPath));
                    
                    break;
            }
        }
    }
}