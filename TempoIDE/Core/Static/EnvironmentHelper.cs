using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using JammaNalysis.MsBuildAnalysis;
using Microsoft.Extensions.Caching.Memory;
using TempoIDE.Core.Types;
using TempoIDE.Core.Types.Wrappers;
using TempoIDE.UserControls.Panels;
using TempoIDE.Windows;

namespace TempoIDE.Core.Static
{
    public static class EnvironmentHelper
    {
        public static MainWindow MainWindow
        {
            get
            {
                MainWindow window = null;

                AppDispatcher.Invoke(delegate
                {
                    window = Application.Current.MainWindow as MainWindow;
                });

                return window;
            }
        }

        public static Dispatcher AppDispatcher => Application.Current.Dispatcher;

        public static Window ActiveWindow => Application.Current
            .Windows
            .OfType<Window>()
            .SingleOrDefault(x => x.IsActive);

        public static EnvironmentCache Cache;
        public static CsSolutionFile Solution;

        public static EnvironmentMode Mode;
        public static FileSystemInfo EnvironmentPath;
        private static DirectoryWatcher directoryWatcher;
        
        public static void Compile()
        {
            if (Mode != EnvironmentMode.Solution)
                return;

            var info = (FileInfo)EnvironmentPath;
            
            ConsoleManager.RunCommand("dotnet", info.DirectoryName, $"build {EnvironmentPath}");
            ConsoleManager.RunCommand("dotnet", info.DirectoryName, $"publish {EnvironmentPath}");
        }

        public static void CreateSolution(DirectoryInfo directory, string name, string template = "sln")
        {
            CreateTemplate(directory, name, template);
            // Implicitly creates .sln file
            
            var solutionFile = directory
                .GetFiles()
                .First(f => f.Name == name + ".sln");
            LoadEnvironment(solutionFile.FullName);
        }

        public static void CreateProject(DirectoryInfo directory, string name, string template)
        {
            CreateTemplate(directory, name, template);
        }

        private static void CreateTemplate(DirectoryInfo directory, string name, string template)
        {
            ConsoleManager.RunCommand("dotnet", directory.FullName, $"new {template} --force --name {name}");
        }
        
        public static void CloseEnvironment()
        {
            Mode = EnvironmentMode.None;
            
            Cache.Clear();

            AppDispatcher.Invoke(MainWindow.Editor.Tabs.CloseAll);
            AppDispatcher.Invoke(LoadExplorer);
        }
        
        public static CachedProjectCompilation GetProjectOfFile(FileInfo file)
        {
            if (Mode != EnvironmentMode.Solution)
                return null;
            
            foreach (var projectFilePath in Cache.CompilationKeys)
            {
                var compilation = (CachedProjectCompilation)Cache.ProjectCompilations.Get(projectFilePath);

                if (compilation.Project.FileSystem.EnumerateTree().Any(projectFile => projectFile.Info.FullName == file.FullName))
                    return compilation;
            }

            return null;
        }
        
        public static void LoadEnvironment(string path)
        {
            directoryWatcher?.Dispose();
            MainWindow.Editor.Tabs.CloseAll();
            
            Cache = new EnvironmentCache();

            var progressDialog = new ProgressDialog
            {
                Title = "Preparing workspace",
                Owner = MainWindow
            };

            progressDialog.Tasks.Enqueue(new ProgressTask(
                "Caching files", delegate { CacheFilesInPath(path); }));
            
            progressDialog.Tasks.Enqueue(new ProgressTask(
                "Reading semantics", Cache.UpdateModels));
            
            progressDialog.Tasks.Enqueue(new ProgressTask(
                "Loading files", delegate { AppDispatcher.Invoke(LoadExplorer); }));

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
                    Solution = new CsSolutionFile(info.FullName);
                    Mode = EnvironmentMode.Solution;

                    await Task.Run(delegate
                    {
                        foreach (var project in Solution.Projects)
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
            MainWindow.Explorer.Clear();
                
            switch (Mode)
            {
                case EnvironmentMode.File:
                    if (EnvironmentPath.Extension == ".sln")
                        goto case EnvironmentMode.Solution;

                    var info = (FileInfo) EnvironmentPath;

                    directoryWatcher = new DirectoryWatcher(info.Directory, info.Name);
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    MainWindow.Explorer.AppendElement(new ExplorerFileItem(info.FullName));
                    MainWindow.Editor.Tabs.Open(info);
                    
                    break;
                case EnvironmentMode.Solution:
                    var solutionDirectory = new FileInfo(EnvironmentPath.FullName).Directory;
                    var topLevel = new ExplorerFileItem(EnvironmentPath.FullName) { IsExpanded = true };
                    
                    MainWindow.Explorer.AppendElement(topLevel);
                    
                    foreach (var project in Solution.Projects)
                    {
                        var projectFile = new FileInfo(project.FilePath);
                        var projectItem = new ExplorerFileItem(projectFile.FullName);

                        topLevel.AppendElement(projectItem);
                        projectItem.AppendDirectory(projectFile.Directory, false);
                    }

                    directoryWatcher = new DirectoryWatcher(solutionDirectory);
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    break;
                case EnvironmentMode.Directory:
                    var directory = (DirectoryInfo) EnvironmentPath;
                    
                    MainWindow.Explorer.AppendDirectory(directory);
                    
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

                    AppDispatcher.Invoke(LoadExplorer);
                    AppDispatcher.Invoke(MainWindow.Editor.Tabs.Refresh);
                    
                    break;
                case WatcherChangeTypes.Renamed:
                    var renamedArgs = (RenamedEventArgs) e;
                    
                    Cache.RemoveFile(new FileInfo(renamedArgs.OldFullPath));
                    Cache.AddFile(new FileInfo(renamedArgs.FullPath));
                    
                    AppDispatcher.Invoke(LoadExplorer);
                    AppDispatcher.Invoke(MainWindow.Editor.Tabs.Refresh);
                    
                    break;
                case WatcherChangeTypes.Deleted:
                    Cache.RemoveFile(new FileInfo(e.FullPath));

                    AppDispatcher.Invoke(LoadExplorer);
                    AppDispatcher.Invoke(MainWindow.Editor.Tabs.Refresh);
                    
                    break;
                case WatcherChangeTypes.Changed:
                    Cache.AddFile(new FileInfo(e.FullPath));

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