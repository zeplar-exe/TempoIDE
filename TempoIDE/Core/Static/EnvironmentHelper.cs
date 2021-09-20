using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jammo.TextAnalysis.DotNet.MsBuild;
using Jammo.TextAnalysis.DotNet.MsBuild.Solutions;
using TempoIDE.Core.Types;
using TempoIDE.Core.Types.Wrappers;
using TempoIDE.UserControls.Panels;
using TempoIDE.Windows;

namespace TempoIDE.Core.Static
{
    public static class EnvironmentHelper
    {
        public static EnvironmentCache Cache;
        public static JSolutionFile Solution;

        public static EnvironmentMode Mode;
        public static FileSystemInfo EnvironmentPath;
        private static DirectoryWatcher directoryWatcher;
        
        public static void Build()
        {
            if (Mode != EnvironmentMode.Solution)
                return;

            var info = (FileInfo)EnvironmentPath;
            
            ConsoleHelper.RunCommand("dotnet", info.DirectoryName, $"build {EnvironmentPath}");
        }

        public static SolutionStream CreateEmptySolution(string directory, string name)
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
            
            LoadEnvironment(path);
            
            return stream;
        }
        
        public static void CloseEnvironment()
        {
            Mode = EnvironmentMode.None;
            
            Cache.Clear();

            ApplicationHelper.AppDispatcher.Invoke(ApplicationHelper.MainWindow.Editor.Tabs.CloseAll);
            ApplicationHelper.AppDispatcher.Invoke(LoadExplorer);
        }
        
        public static CachedProjectCompilation GetProjectOfFile(FileInfo file)
        {
            if (Mode != EnvironmentMode.Solution)
                return null;

            return Cache.ProjectCompilations.Values
                .FirstOrDefault(compilation => compilation.Project.FileSystem.EnumerateTree()
                    .Any(projectFile => projectFile.Info.FullName == file.FullName));
        }
        
        public static void LoadEnvironment(string path)
        {
            directoryWatcher?.Dispose();
            ApplicationHelper.MainWindow.Editor.Tabs.CloseAll();
            
            Cache = new EnvironmentCache();

            var progressDialog = new ProgressDialog
            {
                Title = "Preparing workspace",
                Owner = ApplicationHelper.MainWindow
            };

            progressDialog.Tasks.Enqueue(new ProgressTask(
                "Caching files", delegate { CacheFilesInPath(path); }));
            
            progressDialog.Tasks.Enqueue(new ProgressTask(
                "Reading semantics", Cache.UpdateModels));
            
            progressDialog.Tasks.Enqueue(new ProgressTask(
                "Loading files", delegate { ApplicationHelper.AppDispatcher.Invoke(LoadExplorer); }));

            progressDialog.Completed += delegate { progressDialog.Close(); };

            progressDialog.StartAsync();
        }

        private static async void CacheFilesInPath(string path)
        {
            if (Directory.Exists(path))
            {
                var info = new DirectoryInfo(path);

                Mode = EnvironmentMode.Directory;
                EnvironmentPath = info;

                await Task.Run(delegate
                {
                    foreach (var file in info.EnumerateFiles("*", SearchOption.AllDirectories))
                        Cache.AddFile(file);
                });
            }
            else if (File.Exists(path))
            {
                var info = new FileInfo(path);
                
                Mode = EnvironmentMode.File;
                EnvironmentPath = info;

                if (info.Extension == ".sln")
                {
                    Solution = new JSolutionFile(info.FullName);
                    Mode = EnvironmentMode.Solution;

                    await Task.Run(delegate
                    {
                        foreach (var project in Solution.ProjectFiles)
                        {
                            foreach (var file in project.FileSystem.EnumerateTree().OfType<ProjectFile>())
                            {
                                Cache.AddFile((FileInfo)file.Info);
                            }
                        }
                    });
                }
                else
                {
                    Cache.AddFile(new FileInfo(path));
                }
            }
        }

        private static void LoadExplorer()
        {
            ApplicationHelper.MainWindow.Explorer.Clear();
                
            switch (Mode)
            {
                case EnvironmentMode.File:
                    if (EnvironmentPath.Extension == ".sln")
                        goto case EnvironmentMode.Solution;

                    var info = (FileInfo) EnvironmentPath;

                    directoryWatcher = new DirectoryWatcher(info.Directory, info.Name);
                    directoryWatcher.Changed += DirectoryChanged;

                    ApplicationHelper.MainWindow.Explorer.AppendElement(new ExplorerFileItem(info.FullName));
                    ApplicationHelper.MainWindow.Editor.Tabs.Open(info);
                    
                    break;
                case EnvironmentMode.Solution:
                    var solutionDirectory = new FileInfo(EnvironmentPath.FullName).Directory;
                    var topLevel = new ExplorerFileItem(EnvironmentPath.FullName) { IsExpanded = true };

                    ApplicationHelper.MainWindow.Explorer.AppendElement(topLevel);
                    
                    foreach (var project in Solution.ProjectFiles)
                    {
                        var projectFile = new FileInfo(project.FileInfo.FullName);
                        var projectItem = new ExplorerFileItem(projectFile.FullName);

                        topLevel.AppendElement(projectItem);
                        projectItem.AppendDirectory(projectFile.Directory, false);
                    }

                    directoryWatcher = new DirectoryWatcher(solutionDirectory);
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    break;
                case EnvironmentMode.Directory:
                    var directory = (DirectoryInfo) EnvironmentPath;

                    ApplicationHelper.MainWindow.Explorer.AppendDirectory(directory);
                    
                    directoryWatcher = new DirectoryWatcher(directory);
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    break;
            }
        }
        
        private static void DirectoryChanged(object sender, FileSystemEventArgs e)
        {
            if (Mode == EnvironmentMode.Solution)
                if (!File.Exists(EnvironmentPath.FullName))
                    CloseEnvironment();
            
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    Cache.AddFile(new FileInfo(e.FullPath));

                    ApplicationHelper.AppDispatcher.Invoke(LoadExplorer);
                    ApplicationHelper.AppDispatcher.Invoke(ApplicationHelper.MainWindow.Editor.Tabs.Refresh);
                    
                    break;
                case WatcherChangeTypes.Renamed:
                    var renamedArgs = (RenamedEventArgs) e;
                    
                    Cache.RemoveFile(new FileInfo(renamedArgs.OldFullPath));
                    Cache.AddFile(new FileInfo(renamedArgs.FullPath));

                    ApplicationHelper.AppDispatcher.Invoke(LoadExplorer);
                    ApplicationHelper.AppDispatcher.Invoke(ApplicationHelper.MainWindow.Editor.Tabs.Refresh);
                    
                    break;
                case WatcherChangeTypes.Deleted:
                    Cache.RemoveFile(new FileInfo(e.FullPath));

                    ApplicationHelper.AppDispatcher.Invoke(LoadExplorer);
                    ApplicationHelper.AppDispatcher.Invoke(ApplicationHelper.MainWindow.Editor.Tabs.Refresh);
                    
                    break;
                case WatcherChangeTypes.Changed:
                    var info = new FileInfo(e.FullPath);
                    
                    Cache.AddFile(info);
                    
                    break;
            }
            
            Cache.UpdateModels();
        }
    }

    public enum EnvironmentMode
    {
        None,
        Solution,
        Directory,
        File
    }
}