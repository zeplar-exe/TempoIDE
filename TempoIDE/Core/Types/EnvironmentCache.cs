using System;
using System.IO;
using ByteSizeLib;
using Jammo.TextAnalysis.DotNet.MsBuild;
using TempoIDE.Core.Environments;
using TempoIDE.Core.Static;
using TempoIDE.Core.Types.Wrappers;

namespace TempoIDE.Core.Types
{
    public class EnvironmentCache
    {
        private static CacheOptions DefaultCacheOptions => new()
        {
            MaximumSize = (ulong)ByteSize.FromGigaBytes(6).Bits, MakeRoomOnSizeLimit = true,
        };
        
        public readonly SimpleCache<string, CachedFile> FileData;
        public readonly SimpleCache<string, CachedProjectCompilation> ProjectCompilations;

        public EnvironmentCache()
        {
            FileData = new SimpleCache<string, CachedFile>(DefaultCacheOptions);
            ProjectCompilations = new SimpleCache<string, CachedProjectCompilation>(DefaultCacheOptions);
        }
        
        public void UpdateModels()
        {
            ProjectCompilations.Clear();

            if (EnvironmentHelper.Current is SolutionEnvironment solutionEnv)
            { // TODO: Use compilation of all relevant files if false
                var solution = new JSolutionFile(solutionEnv.EnvironmentPath.FullName);

                foreach (var project in solution.ProjectFiles)
                {
                    ProjectCompilations.Set(project.FileInfo.FullName, new CachedProjectCompilation(project));
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
            return FileData.GetOrCreate(file.FullName, CacheItemFromFile(file));
        }

        public void AddFile(FileInfo file)
        {
            if (file.Exists)
            {
                FileData.GetOrCreate(file.FullName, CacheItemFromFile(file)).Update();
            }
        }

        public void RemoveFile(FileInfo file)
        {
            FileData.Remove(file.FullName);
        }

        private CacheItem<CachedFile> CacheItemFromFile(FileInfo file)
        {
            return new CacheItem<CachedFile>(
                new CachedFile(file), new CacheItemOptions
                {
                    NonAccessDeletionTime = TimeSpan.FromMinutes(15),
                    Size = (ulong)file.Length
                });
        }
    }
}