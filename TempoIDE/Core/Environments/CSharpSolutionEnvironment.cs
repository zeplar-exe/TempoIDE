using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jammo.TextAnalysis;
using Jammo.TextAnalysis.DotNet.CSharp;
using Jammo.TextAnalysis.DotNet.MsBuild;
using Jammo.TextAnalysis.DotNet.MsBuild.Solutions;
using TempoIDE.Controls.Panels;
using TempoIDE.Core.Associators;
using TempoIDE.Core.DataStructures;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.Wrappers;

namespace TempoIDE.Core.Environments;

public class CSharpSolutionEnvironment : DevelopmentEnvironment
{
    private readonly Dictionary<string, CachedProjectCompilation> projectCompilations = new();

    public readonly JSolutionFile SolutionFile;

    public CSharpSolutionEnvironment(FileInfo file) : base(file, CreateWatcher(file.FullName))
    {
        SolutionFile = new JSolutionFile(file.FullName);

        foreach (var project in SolutionFile.ProjectFiles)
            projectCompilations.Add(project.FileInfo.Name, new CachedProjectCompilation(project));
    }
        
    public CSharpSolutionEnvironment(JSolutionFile solutionFile) 
        : base(new FileInfo(solutionFile.Stream.FilePath), CreateWatcher(solutionFile.Stream.FilePath))
    {
        SolutionFile = solutionFile;
    }

    private static DirectoryWatcher CreateWatcher(string path)
    {
        var file = new FileInfo(path);
            
        return new DirectoryWatcher(file.Directory!);
    }

    public static CSharpSolutionEnvironment CreateEmpty(string directory, string name)
    {
        var path = Path.Join(directory, name + ".sln");

        var file = new JSolutionFile(path);
        var stream = file.Stream;
            
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
            
        stream.Write();
        file.UpdateProjects();
            
        return new CSharpSolutionEnvironment(file);
    }

    public override AnalysisCompilation? GetRelevantCompilation(FileInfo? file = null)
    {
        if (file == null)
            return null;

        if (file.Extension == ".cs")
        {
            if (Cache.FileData.Get(file.FullName) is not CachedProjectFile cachedFile)
                return ExtensionAssociator.AnalysisCompilationFromFile(file);

            foreach (var (path, compilation) in projectCompilations)
            {
                if (path == cachedFile.FileInfo.FullName)
                    return compilation.Compilation;
            }
        }
            
        return ExtensionAssociator.AnalysisCompilationFromFile(file);
    }

    public override IEnumerable<Diagnostic> GetFileDiagnostics(FileInfo? file = null)
    { // TODO: Aight, we seriously need to move away from Jammo.TextAnalysis
        if (file == null)
            return Enumerable.Empty<Diagnostic>();

        if (GetRelevantCompilation() is not CSharpProjectAnalysisCompilation compilation)
            return Enumerable.Empty<Diagnostic>(); // TODO

        foreach (var document in compilation.Documents)
        {
            if (document.File.FullName != file.FullName)
                return document.Diagnostics;
        }
            
        return Enumerable.Empty<Diagnostic>();
    }

    public override async void CacheFiles()
    {
        await Task.Run(delegate
        {
            foreach (var project in SolutionFile.ProjectFiles)
            {
                foreach (var file in project.FileSystem.EnumerateTree().OfType<ProjectFile>())
                    Cache.FileData.Set(file.Info.FullName, new CachedProjectFile((FileInfo)file.Info, project));
            }
        });
    }

    public override void RefreshCache()
    {
        Cache.Clear();
        CacheFiles();
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
            
        projectCompilations.Clear();
            
        foreach (var project in SolutionFile.ProjectFiles)
            projectCompilations.Add(project.FileInfo.Name, new CachedProjectCompilation(project));
    }
}