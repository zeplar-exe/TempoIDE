using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Construction;
using Microsoft.Extensions.Caching.Memory;
using TempoIDE.Core.Static;
using TempoIDE.Core.Types.Wrappers;

namespace TempoIDE.Core.Types
{
    public class EnvironmentCache
    {
        public MemoryCache Compilations;
        private readonly Dictionary<string, CachedFile> fileData = new();

        public EnvironmentCache()
        {
            RefreshCache();
        }
        
        public void UpdateModels()
        {
            if (EnvironmentHelper.Mode == EnvironmentMode.Solution)
            {
                RefreshCache();
                
                var solution = SolutionFile.Parse(EnvironmentHelper.EnvironmentPath.FullName);
                
                foreach (var project in solution.ProjectsInOrder)
                    Compilations.Set(project.ProjectGuid, new CachedProjectCompilation(project));
            }
        }

        private void RefreshCache()
        {
            Compilations?.Dispose();
            Compilations = new MemoryCache(new MemoryCacheOptions
            {
                ExpirationScanFrequency = new TimeSpan(0, 5, 0, 0)
            });
        }

        public CachedFile GetFile(FileInfo file)
        {
            return fileData.TryGetValue(file.FullName, out var cached) ? cached : null;
        }

        public void AddFile(FileInfo file)
        {
            if (file.Exists)
            {
                try
                {
                    if (fileData.TryGetValue(file.FullName, out var cached))
                        cached.Update();
                    else
                        fileData.Add(file.FullName, new CachedFile(file));
                }
                catch (IOException)
                {
                    fileData.Remove(file.FullName);
                }
            }
        }

        public void RemoveFile(FileInfo file)
        {
            if (fileData.ContainsKey(file.FullName))
                fileData.Remove(file.FullName);
        }
    }
}