using System.Collections.Generic;
using System.IO;
using Jammo.CsAnalysis.MsBuildAnalysis;
using TempoIDE.Core.Static;
using TempoIDE.Core.Types.Wrappers;

namespace TempoIDE.Core.Types
{
    public class EnvironmentCache
    {
        public readonly Dictionary<string, CachedProjectCompilation> ProjectCompilations = new();
        public readonly Dictionary<string, CachedFile> FileData = new();

        public EnvironmentCache()
        {
            Clear();
        }
        
        public void UpdateModels()
        {
            if (EnvironmentHelper.Mode == EnvironmentMode.Solution)
            {
                ProjectCompilations.Clear();

                var solution = new JSolutionFile(EnvironmentHelper.EnvironmentPath.FullName);

                foreach (var project in solution.ProjectFiles)
                {
                    ProjectCompilations[project.FileInfo.FullName] = new CachedProjectCompilation(project);
                }
            }
        }

        public void Clear()
        {
            FileData.Clear();
            ProjectCompilations.Clear();
        }

        public CachedFile GetFile(FileInfo file)
        {
            if (!FileData.TryGetValue(file.FullName, out var existing))
                return FileData[file.FullName] = new CachedFile(file);
            
            return existing;
        }

        public void AddFile(FileInfo file)
        {
            if (file.Exists)
            {
                try
                {
                    if (FileData.TryGetValue(file.FullName, out var cached))
                    {
                        cached.Update();
                    }
                    else
                    {
                        FileData[file.FullName] = new CachedFile(file);
                    }
                }
                catch (IOException)
                {
                    FileData.Remove(file.FullName);
                }
            }
        }

        public void RemoveFile(FileInfo file)
        {
            FileData.Remove(file.FullName);
        }
    }
}