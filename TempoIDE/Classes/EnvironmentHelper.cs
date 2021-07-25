using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.CodeAnalysis;
using TempoIDE.UserControls;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using TempoIDE.Windows;

namespace TempoIDE.Classes
{
    public static class EnvironmentHelper
    {
        public static MainWindow MainWindow => Application.Current.MainWindow as MainWindow;
        public static EnvironmentFilterMode FilterMode;
        public static string EnvironmentPath;
        private static DirectoryWatcher directoryWatcher;

        public static void Compile()
        {
            if (FilterMode != EnvironmentFilterMode.Solution)
                return;
            
            var process = new System.Diagnostics.Process();
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"cd {new FileInfo(EnvironmentPath).Directory} && dotnet build {EnvironmentPath} && dotnet publish {EnvironmentPath}"
            };

            process.StartInfo = startInfo;
            process.Start();
        }

        public static void CreateSolution(string path)
        {
            // MsBuild here
            
            LoadEnvironment(path, EnvironmentFilterMode.Solution);
        }
        
        public static void LoadEnvironment(string path, EnvironmentFilterMode mode)
        {
            MainWindow.Editor.CloseAll();

            if (new FileInfo(path).Extension == ".sln")
                mode = EnvironmentFilterMode.Solution;
            
            FilterMode = mode;
            EnvironmentPath = path;
            
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
            LoadExplorer();
        }

        private static void LoadExplorer()
        {
            MainWindow.Explorer.Clear();
            
            switch (FilterMode)
            {
                case EnvironmentFilterMode.None:
                    break;
                case EnvironmentFilterMode.Solution:
                    var topLevel = new ExplorerPanelElement(EnvironmentPath);
                    var slnDirectory = new FileInfo(EnvironmentPath).Directory;
                    
                    MainWindow.Explorer.AppendElement(EnvironmentPath);
                    MainWindow.Explorer.AppendDirectory(slnDirectory, topLevel);

                    directoryWatcher = new DirectoryWatcher(slnDirectory);
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    break;
                case EnvironmentFilterMode.Directory:
                    var directory = new DirectoryInfo(EnvironmentPath);
                    MainWindow.Explorer.AppendDirectory(directory);
                    
                    directoryWatcher = new DirectoryWatcher(directory);
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    break;
                case EnvironmentFilterMode.File:
                    var filePath = new FileInfo(EnvironmentPath);

                    if (filePath.Extension == ".sln")
                        goto case EnvironmentFilterMode.Solution;
                    
                    directoryWatcher = new DirectoryWatcher(filePath.Directory, Path.GetFileName(EnvironmentPath));
                    directoryWatcher.Changed += DirectoryChanged;
                    
                    MainWindow.Explorer.AppendElement(EnvironmentPath);
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