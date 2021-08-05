using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Build.Construction;
using Microsoft.CodeAnalysis;
using TempoIDE.Classes.Types;
using TempoIDE.UserControls;
using TempoIDE.Windows;

namespace TempoIDE.Classes
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

        public static void CreateSolution(DirectoryInfo directory, string name)
        {
            ConsoleManager.RunCommand("dotnet", directory.FullName, $"new sln --force --name {name}");
            // Implicitly creates .sln file
            
            var solutionFile = directory
                .GetFiles()
                .First(f => f.Name == name + ".sln");
            LoadEnvironment(solutionFile.FullName);
        }

        public static void CreateProject(DirectoryInfo directory, string name)
        {
            // TODO: This
        }
        
        public static void LoadEnvironment(string path)
        {
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

            progressDialog.Show();
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
                    foreach (var file in info.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
                        Cache.AddFile(new FileInfo(file.FullName));
                });
            }
            else if (File.Exists(path))
            {
                var info = new FileInfo(path);
                
                Mode = EnvironmentMode.File;
                EnvironmentPath = info;

                if (info.Extension == ".sln")
                {
                    Mode = EnvironmentMode.Solution;

                    await Task.Run(delegate
                    {
                        foreach (var file in info.Directory.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
                            Cache.AddFile(new FileInfo(file.FullName));
                    });
                }
                else
                {
                    Cache.AddFile(new FileInfo(path));
                }
            }
        }
        
        private static void DirectoryChanged(object sender, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    Cache.AddFile(new FileInfo(e.FullPath));

                    AppDispatcher.Invoke(LoadExplorer);
                    
                    break;
                case WatcherChangeTypes.Renamed:
                    var renamedArgs = (RenamedEventArgs) e;
                    
                    Cache.RemoveFile(new FileInfo(renamedArgs.OldFullPath));
                    Cache.AddFile(new FileInfo(renamedArgs.FullPath));
                    
                    AppDispatcher.Invoke(LoadExplorer);
                    
                    break;
                case WatcherChangeTypes.Deleted:
                    Cache.RemoveFile(new FileInfo(e.FullPath));

                    AppDispatcher.Invoke(LoadExplorer);
                    
                    break;
                case WatcherChangeTypes.Changed:
                    Cache.AddFile(new FileInfo(e.FullPath));

                    break;
            }
            
            //TODO: Cache.UpdateModels();
            AppDispatcher.Invoke(MainWindow.Editor.Tabs.Refresh);
        }

        private static void LoadExplorer()
        {
            MainWindow.Explorer.Clear();
                
            switch (Mode)
            {
                case EnvironmentMode.Solution:
                    var solution = SolutionFile.Parse(EnvironmentPath.FullName);
                    var solutionDirectory = new FileInfo(EnvironmentPath.FullName).Directory;
                    var topLevel = new ExplorerFileItem(EnvironmentPath.FullName) { IsExpanded = true };
                    
                    MainWindow.Explorer.AppendElement(topLevel);
                    
                    foreach (var project in solution.ProjectsInOrder)
                    {
                        var file = new FileInfo(project.AbsolutePath);
                        var projectItem = new ExplorerFileItem(file.FullName);
                        
                        topLevel.AppendElement(projectItem);
                        projectItem.AppendDirectory(file.Directory, false);
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
                case EnvironmentMode.File:
                    if (EnvironmentPath.Extension == ".sln")
                        goto case EnvironmentMode.Solution;

                    var info = (FileInfo) EnvironmentPath;

                    directoryWatcher = new DirectoryWatcher(info.Directory, info.Name);
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    MainWindow.Explorer.AppendElement(new ExplorerFileItem(info.FullName));
                    MainWindow.Editor.Tabs.Open(info);
                    break;
            }
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