using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Construction;
using Microsoft.CodeAnalysis;
using TempoAnalysis;

namespace TempoIDE.Classes.Types
{
    public class EnvironmentCache
    {
        public Dictionary<string, CachedProjectCompilation> Compilations = new();
        private readonly Dictionary<string, CachedFile> fileData = new();
        
        public void UpdateModels()
        {
            Compilations.Clear();
            
            if (EnvironmentHelper.Mode == EnvironmentMode.Solution)
            {
                var solution = SolutionFile.Parse(EnvironmentHelper.EnvironmentPath.FullName);
                
                foreach (var project in solution.ProjectsInOrder)
                    Compilations[project.ProjectGuid] = new CachedProjectCompilation(project);
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