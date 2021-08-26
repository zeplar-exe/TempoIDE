using System;
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

        private static MemoryCacheOptions DefaultCacheOptions => new()
        {
            ExpirationScanFrequency = new TimeSpan(0, 0, 5, 0),
            SizeLimit = 10000000024, // In bits, 1.25 gigabytes
            CompactionPercentage = 0.75
        };

        public EnvironmentCache()
        {
            ClearCache();
        }
        
        public void UpdateModels()
        {
            if (EnvironmentHelper.Mode == EnvironmentMode.Solution)
            {
                ClearCache();
                
                var solution = new CsSolutionFile(EnvironmentHelper.EnvironmentPath.FullName);
                
                foreach (var project in solution.Projects)
                    ProjectCompilations.Set(project.FilePath, new CachedProjectCompilation(project));
            }
        }

        private void ClearCache()
        {
            ProjectCompilations?.Dispose();
            ProjectCompilations = new MemoryCache(DefaultCacheOptions);
            
            FileData?.Dispose();
            FileData = new MemoryCache(DefaultCacheOptions);
        }

        public CachedFile GetFile(FileInfo file)
        {
            return FileData.GetOrCreate(file.FullName, entry =>
            {
                var cached = new CachedFile(file);
                entry.Size = cached.FileInfo.Length * 8;
                
                return cached;
            });
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
                        var newCached = new CachedFile(file);
                        var size = newCached.FileInfo.Length * 8; // Bytes to bits
                        
                        FileData.Set(file.FullName, newCached, new MemoryCacheEntryOptions().SetSize(size));
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