using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jammo.TextAnalysis.DotNet.MsBuild;
using Jammo.TextAnalysis.DotNet.MsBuild.Solutions;
using TempoIDE.Controls.Panels;
using TempoIDE.Core.DataStructures;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.Wrappers;

namespace TempoIDE.Core.Environments
{
    public class SolutionEnvironment : DevelopmentEnvironment
    {
        public readonly SolutionStream Stream;

        public SolutionEnvironment(FileInfo file) : base(file, CreateWatcher(file.FullName))
        {
            Stream = SolutionParser.Parse( IOHelper.ReadFullStream(file.OpenRead()));
        }
        
        public SolutionEnvironment(SolutionStream stream) 
            : base(new FileInfo(stream.FilePath), CreateWatcher(stream.FilePath))
        {
            Stream = stream;
        }

        private static DirectoryWatcher CreateWatcher(string path)
        {
            var file = new FileInfo(path);
            
            return new DirectoryWatcher(file.Directory);
        }

        public static SolutionEnvironment CreateEmpty(string directory, string name)
        {
            var stream = new SolutionStream();
            stream.Version = new FormatVersion("12.00");

            var global = new GlobalDefinition();
            var configPlatforms = new GlobalSectionDefinition
            {
                ConfigurationType = "SolutionConfigurationPlatforms",
                RunTime = "preSolution"
            };
            
            configPlatforms.AddConfiguration(new GlobalConfiguration
            {
                Config = "Debug|AnyCpu", 
                AssignedConfig = "Debug|AnyCpu"
            });
            
            configPlatforms.AddConfiguration(new GlobalConfiguration
            {
                Config = "Release|AnyCpu", 
                AssignedConfig = "Release|AnyCpu"
            });
            
            global.AddSection(configPlatforms);
            stream.Globals.Add(global);

            var path = Path.Join(directory, name + ".sln");
            stream.WriteTo(path);
            
            return new SolutionEnvironment(stream);
        }

        public CachedProjectCompilation GetProjectOfFile(FileInfo file)
        {
            return Cache.ProjectCompilations.Values
                .FirstOrDefault(compilation => compilation.Project.FileSystem.EnumerateTree()
                    .Any(projectFile => projectFile.Info.FullName == file.FullName));
        }

        public override async void CacheFiles()
        {
            using var solution = new JSolutionFile(EnvironmentPath.FullName);

            await Task.Run(delegate
            {
                foreach (var project in solution.ProjectFiles)
                {
                    foreach (var file in project.FileSystem.EnumerateTree().OfType<ProjectFile>())
                    {
                        Cache.AddFile((FileInfo)file.Info);
                    }
                }
            });
        }

        public override void RefreshCache()
        {
            Cache.Clear();
            CacheFiles();
            Cache.UpdateModels();
        }

        public override void LoadExplorer(ExplorerView explorer)
        {
            var topLevel = new ExplorerFileSystemItem(EnvironmentPath.FullName) { IsExpanded = true };

            explorer.AppendElement(topLevel);

            using var solution = new JSolutionFile(EnvironmentPath.FullName);
                    
            foreach (var project in solution.ProjectFiles)
            {
                var projectFile = new FileInfo(project.FileInfo.FullName);
                var projectItem = new ExplorerFileSystemItem(projectFile.FullName);

                topLevel.AppendElement(projectItem);
                projectItem.AppendDirectory(projectFile.Directory, false);
            }
        }

        protected override void DirectoryChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                if (e.FullPath == EnvironmentPath.FullName)
                {
                    EnvironmentHelper.CloseEnvironment();
                    return;
                }
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
                    Cache.RemoveFile(new FileInfo(e.FullPath));
                    
                    break;
                case WatcherChangeTypes.Changed:
                    var info = new FileInfo(e.FullPath);
                    
                    Cache.AddFile(info);
                    
                    break;
            }
            
            RefreshCache();
        }
    }
}