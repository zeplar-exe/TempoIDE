using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Build.Construction;
using TempoIDE.UserControls;
using TempoIDE.Windows;

namespace TempoIDE.Classes
{
    public static class EnvironmentHelper
    {
        public static MainWindow MainWindow => Application.Current.MainWindow as MainWindow;
        public static Window ActiveWindow => Application.Current
            .Windows
            .OfType<Window>()
            .SingleOrDefault(x => x.IsActive);

        public static EnvironmentCache Cache;
        
        public static EnvironmentFilterMode FilterMode;
        public static FileInfo EnvironmentPath;
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
            
            ConsoleManager.RunCommand("dotnet", EnvironmentPath.DirectoryName, $"build {EnvironmentPath}");
            ConsoleManager.RunCommand("dotnet", EnvironmentPath.DirectoryName, $"publish {EnvironmentPath}");
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
        
        public static void LoadEnvironment(string path, EnvironmentFilterMode mode)
        {
            MainWindow.Editor.Tabs.CloseAll();

            Cache = new EnvironmentCache();
            
            if (Directory.Exists(path))
            {
                foreach (var file in new DirectoryInfo(path).EnumerateFileSystemInfos())
                    Cache.AddFile(new FileInfo(file.FullName));
            }
            else if (File.Exists(path))
            {
                Cache.AddFile(new FileInfo(path));
            }

            EnvironmentPath = new FileInfo(path);

            if (EnvironmentPath.Extension == ".sln")
                mode = EnvironmentFilterMode.Solution;
            
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
                    Cache.RemoveFile(new FileInfo(e.FullPath));
                    Cache.AddFile(new FileInfo(e.FullPath));
                    
                    break;
            }

            LoadExplorer();
            MainWindow.Editor.Tabs.Refresh();
        }

        private static void LoadExplorer()
        {
            MainWindow.Explorer.Clear();
            
            switch (FilterMode)
            {
                case EnvironmentFilterMode.None:
                    break;
                case EnvironmentFilterMode.Solution:
                    var solution = SolutionFile.Parse(EnvironmentPath.FullName);
                    var solutionDirectory = new FileInfo(EnvironmentPath.FullName).Directory;
                    var topLevel = new ExplorerFileItem(EnvironmentPath.FullName) { IsExpanded = true };
                    
                    MainWindow.Explorer.AppendElement(topLevel);
                    
                    foreach (var project in solution.ProjectsInOrder)
                    {
                        var file = new FileInfo(project.AbsolutePath);
                        var projectItem = MainWindow.Explorer.AppendElement(new ExplorerFileItem(file.FullName), topLevel);
                        
                        MainWindow.Explorer.AppendDirectory(file.Directory, projectItem);
                    }

                    directoryWatcher = new DirectoryWatcher(solutionDirectory);
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    break;
                case EnvironmentFilterMode.Directory:
                    var directory = new DirectoryInfo(EnvironmentPath.FullName);
                    MainWindow.Explorer.AppendDirectory(directory);
                    
                    directoryWatcher = new DirectoryWatcher(directory);
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    break;
                case EnvironmentFilterMode.File:
                    if (EnvironmentPath.Extension == ".sln")
                        goto case EnvironmentFilterMode.Solution;
                    
                    directoryWatcher = new DirectoryWatcher(EnvironmentPath.Directory, EnvironmentPath.FullName);
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    MainWindow.Explorer.AppendElement(new ExplorerFileItem(EnvironmentPath.FullName));
                    MainWindow.Editor.Tabs.Open(EnvironmentPath);
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