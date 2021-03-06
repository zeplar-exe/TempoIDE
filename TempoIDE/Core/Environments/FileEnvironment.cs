using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jammo.TextAnalysis;
using TempoIDE.Controls.Explorer;
using TempoIDE.Core.Associators;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.Wrappers;

namespace TempoIDE.Core.Environments
{
    public class FileEnvironment : DevelopmentEnvironment
    {
        public FileEnvironment(FileInfo path) : base(path, new DirectoryWatcher(path.Directory, path.Name))
        {
            
        }

        public override AnalysisCompilation GetRelevantCompilation(FileInfo file = null)
        {
            return ExtensionAssociator.AnalysisCompilationFromFile(file);
        }

        public override IEnumerable<Diagnostic> GetFileDiagnostics(FileInfo file = null)
        {
            return Enumerable.Empty<Diagnostic>(); // TODO
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

            explorer.Add(ExplorerFile.FromFile(info.FullName));
            ApplicationHelper.MainWindow.Editor.Tabs.OpenFile(info);
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
                    
                    break;
            }
        }
    }
}