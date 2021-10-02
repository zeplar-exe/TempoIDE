using System.IO;
using Jammo.TextAnalysis.DotNet.CSharp;
using TempoIDE.Controls.Panels;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.Wrappers;

namespace TempoIDE.Core.Environments
{
    public class FileEnvironment : DevelopmentEnvironment
    {
        private CSharpAnalysisCompilation compilation;
        
        public FileEnvironment(FileInfo path) : base(path, new DirectoryWatcher(path.Directory, path.Name))
        {
            compilation = CSharpAnalysisCompilationHelper.Create(path.FullName, AnalysisType.File);;
        }

        public override CSharpAnalysisCompilation GetRelevantCompilation(FileInfo file = null)
        {
            return compilation;
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
                    if (e.FullPath != EnvironmentPath.FullName)
                        break;
                    
                    Cache.AddFile(new FileInfo(e.FullPath));
                    compilation = CSharpAnalysisCompilationHelper.Create(EnvironmentPath.FullName, AnalysisType.File);
                    
                    break;
            }
        }
    }
}