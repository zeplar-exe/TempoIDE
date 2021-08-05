using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Build.Construction;
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

        public static EnvironmentFilterMode FilterMode;
        public static FileSystemInfo EnvironmentPath;
        private static DirectoryWatcher directoryWatcher;
        

        public static bool IsCoreElementFocused()
        {
            if (MainWindow.Editor.SelectedEditor == null)
                return false;

            return MainWindow.Editor.Tabs.GetFocusedEditor() != null || MainWindow.Explorer.IsFocused;
        }
        
        public static void Compile()
        {
            if (FilterMode != EnvironmentFilterMode.Solution)
                return;

            var info = (FileInfo)EnvironmentPath;
            
            ConsoleManager.RunCommand("dotnet", info.DirectoryName, $"build {EnvironmentPath}");
            ConsoleManager.RunCommand("dotnet", info.DirectoryName, $"publish {EnvironmentPath}");
        }

        public static void CreateSolution(DirectoryInfo path, string name)
        {
            ConsoleManager.RunCommand("dotnet", path.FullName, $"new sln --force --name {name}");
            // Implicitly creates .sln file
            
            var solutionFile = path
                .GetFiles()
                .First(f => f.Name == name + ".sln");
            LoadEnvironment(solutionFile.FullName, EnvironmentFilterMode.Solution);
        }
        
        public static async void LoadEnvironment(string path, EnvironmentFilterMode mode)
        {
            MainWindow.Editor.Tabs.CloseAll();

            Cache = new EnvironmentCache();

            if (Directory.Exists(path))
            {
                var info = new DirectoryInfo(path);

                await Task.Run(delegate
                {
                    foreach (var file in info.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
                        Cache.AddFile(new FileInfo(file.FullName));
                });

                EnvironmentPath = info;
            }
            else if (File.Exists(path))
            {
                var info = new FileInfo(path);

                if (info.Extension == ".sln")
                {
                    mode = EnvironmentFilterMode.Solution;

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

                EnvironmentPath = info;
            }

            FilterMode = mode;

            LoadExplorer();
        }

        public static EnvironmentFilterMode GetPathType(string path)
        {
            var mode = EnvironmentFilterMode.None;
            
            if (Directory.Exists(path))
            {
                mode = EnvironmentFilterMode.Directory;
            }
            else if (File.Exists(path))
            {
                var file = new FileInfo(path);

                mode = file.Extension switch
                {
                    ".csproj" => EnvironmentFilterMode.File,
                    ".sln" => EnvironmentFilterMode.Solution,
                    _ => EnvironmentFilterMode.None
                };
            }

            return mode;
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
            
            AppDispatcher.Invoke(MainWindow.Editor.Tabs.Refresh);
        }

        private static void LoadExplorer()
        {
            MainWindow.Explorer.Clear();
                
            switch (FilterMode)
            {
                case EnvironmentFilterMode.Solution:
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
                case EnvironmentFilterMode.Directory:
                    var directory = (DirectoryInfo) EnvironmentPath;
                    
                    MainWindow.Explorer.AppendDirectory(directory);
                    
                    directoryWatcher = new DirectoryWatcher(directory);
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    break;
                case EnvironmentFilterMode.File:
                    if (EnvironmentPath.Extension == ".sln")
                        goto case EnvironmentFilterMode.Solution;

                    var info = (FileInfo) EnvironmentPath;

                    directoryWatcher = new DirectoryWatcher(info.Directory, info.Name);
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    MainWindow.Explorer.AppendElement(new ExplorerFileItem(info.FullName));
                    MainWindow.Editor.Tabs.Open(info);
                    break;
            }
        }
    }

    public enum EnvironmentFilterMode
    {
        None,
        Solution,
        Directory,
        File
    }
}