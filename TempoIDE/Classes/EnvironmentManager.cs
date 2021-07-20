using System;
using System.IO;
using System.Windows;
using TempoIDE.UserControls;
using TempoIDE.Windows;

namespace TempoIDE.Classes
{
    public static class EnvironmentManager
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
                    MainWindow.Explorer.AppendExpander(topLevel, 0);
                    MainWindow.Explorer.AppendDirectory(slnDirectory, 0, topLevel);

                    directoryWatcher = new DirectoryWatcher(slnDirectory);
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    break;
                case EnvironmentFilterMode.Directory:
                    var directory = new DirectoryInfo(path);
                    MainWindow.Explorer.AppendDirectory(directory, 0);
                    
                    directoryWatcher = new DirectoryWatcher(directory);
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    break;
                case EnvironmentFilterMode.File:
                    var fileDirectory = new DirectoryInfo(path);
                    var element = new ExplorerPanelElement
                    {
                        FilePath = path,
                    };
                    
                    directoryWatcher = new DirectoryWatcher(fileDirectory, Path.GetFileName(path));
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    MainWindow.Explorer.AppendElement(element, 0);
                    break;
            }
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