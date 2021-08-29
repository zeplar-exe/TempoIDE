using System;
using System.Collections.Generic;
using System.IO;
using JammaNalysis.MsBuildAnalysis;
using Microsoft.Extensions.Caching.Memory;
using TempoIDE.Core.Static;
using TempoIDE.Core.Types.Wrappers;

namespace TempoIDE.Core.Types
{
    public class EnvironmentCache
    {
        public MemoryCache ProjectCompilations;
        public MemoryCache FileData;

        public List<string> CompilationKeys = new();
        public List<string> FileKeys = new();

        private static MemoryCacheOptions DefaultCacheOptions => new()
        {
            ExpirationScanFrequency = new TimeSpan(0, 0, 5, 0),
            CompactionPercentage = 0.75
        };

        public EnvironmentCache()
        {
            Clear();
        }
        
        public void UpdateModels()
        {
            if (EnvironmentHelper.Mode == EnvironmentMode.Solution)
            {
                Clear();
                CompilationKeys.Clear();

                var solution = new CsSolutionFile(EnvironmentHelper.EnvironmentPath.FullName);

                foreach (var project in solution.Projects)
                {
                    ProjectCompilations.Set(project.FilePath, new CachedProjectCompilation(project));
                    CompilationKeys.Add(project.FilePath);
                }
            }
        }

        public void Clear()
        {
            ProjectCompilations?.Dispose();
            ProjectCompilations = new MemoryCache(DefaultCacheOptions);
            
            FileData?.Dispose();
            FileData = new MemoryCache(DefaultCacheOptions);
        }

        public CachedFile GetFile(FileInfo file)
        {
            return FileData.GetOrCreate(file.FullName, _ => new CachedFile(file));
        }

        public void AddFile(FileInfo file)
        {
            if (file.Exists)
            {
                try
                {
                    if (FileData.TryGetValue(file.FullName, out var cached))
                    {
                        ((CachedFile)cached).Update();
                    }
                    else
                    {
                        FileData.Set(file.FullName, new CachedFile(file));
                        FileKeys.Add(file.FullName);
                    }
                }
                catch (IOException)
                {
                    FileData.Remove(file.FullName);
                }
                catch (ObjectDisposedException)
                {
                    // This exception occurs infrequently and I don't know how to fix it
                }
            }
        }

        public void RemoveFile(FileInfo file)
        {
            FileData.Remove(file.FullName);
            FileKeys.Remove(file.FullName);
        }
    }
}