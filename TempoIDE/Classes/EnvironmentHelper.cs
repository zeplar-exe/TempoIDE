using System.IO;
using System.Windows;
using TempoIDE.UserControls;
using TempoIDE.Windows;

namespace TempoIDE.Classes
{
    public static class EnvironmentHelper
    {
        public static MainWindow MainWindow => Application.Current.MainWindow as MainWindow;
        public static EnvironmentFilterMode FilterMode;
        private static DirectoryWatcher directoryWatcher;

        public static void CreateSolution(string path)
        {
            // MsBuild here
            
            LoadEnvironment(path, EnvironmentFilterMode.Solution);
        }
        
        public static void LoadEnvironment(string path, EnvironmentFilterMode mode)
        {
            MainWindow.Explorer.Clear();
            MainWindow.Editor.CloseAll();
            
            switch (mode)
            {
                case EnvironmentFilterMode.None:
                    break;
                case EnvironmentFilterMode.Solution:
                    var topLevel = new ExplorerPanelExpander
                    {
                        Element = { FilePath = path },
                        ElementExpander = { IsExpanded = true }
                    };

                    var slnDirectory = new FileInfo(path).Directory;
                    MainWindow.Explorer.AppendExpander(topLevel);
                    MainWindow.Explorer.AppendDirectory(slnDirectory, topLevel);

                    directoryWatcher = new DirectoryWatcher(slnDirectory);
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    break;
                case EnvironmentFilterMode.Directory:
                    var directory = new DirectoryInfo(path);
                    MainWindow.Explorer.AppendDirectory(directory);
                    
                    directoryWatcher = new DirectoryWatcher(directory);
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    break;
                case EnvironmentFilterMode.File:
                    var filePath = new FileInfo(path);

                    if (filePath.Extension == ".sln")
                        goto case EnvironmentFilterMode.Solution;

                    var element = new ExplorerPanelElement
                    {
                        FilePath = path,
                    };
                    
                    directoryWatcher = new DirectoryWatcher(filePath.Directory, Path.GetFileName(path));
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    MainWindow.Explorer.AppendElement(element);
                    break;
            }
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