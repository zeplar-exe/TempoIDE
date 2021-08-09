using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace TempoIDE.Classes.Types
{
    public class EnvironmentCache
    {
        public Dictionary<ProjectId, CachedCompilation> Compilations = new Dictionary<ProjectId, CachedCompilation>();
        private readonly Dictionary<string, CachedFile> fileData = new Dictionary<string, CachedFile>();
        
        public void UpdateModels()
        {
            Compilations.Clear();
            
            if (EnvironmentHelper.Mode == EnvironmentMode.Solution)
            {
                var workspace = MSBuildWorkspace.Create();
                var solution = workspace.OpenSolutionAsync(EnvironmentHelper.EnvironmentPath.FullName).Result;
                
                foreach (var project in solution.Projects)
                {
                    try
                    {
                        Compilations[project.Id] = new CachedCompilation(project);
                    }
                    catch (ArgumentException e) { /* Project is invalid */  }
                }
            }
        }

        public CachedFile GetFile(FileInfo file)
        {
            return fileData.TryGetValue(file.FullName, out var cached) ? cached : null;
        }

        public void AddFile(FileInfo file)
        {
            if (file.Exists)
            {
                if (fileData.TryGetValue(file.FullName, out var cached))
                    cached.Update();
                else
                    fileData.Add(file.FullName, new CachedFile(file));
            }
        }

        public void RemoveFile(FileInfo file)
        {
            if (fileData.ContainsKey(file.FullName))
                fileData.Remove(file.FullName);
        }
    }
}