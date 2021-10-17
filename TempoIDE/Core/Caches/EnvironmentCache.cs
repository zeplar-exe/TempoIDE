using System;
using System.IO;
using ByteSizeLib;
using TempoIDE.Core.DataStructures;

namespace TempoIDE.Core.Caches
{
    public class EnvironmentCache
    {
        private static CacheOptions DefaultCacheOptions => new()
        {
            MaximumSize = (ulong)ByteSize.FromGigaBytes(6).Bits, MakeRoomOnSizeLimit = true,
        };
        
        public readonly SimpleCache<string, CachedFile> FileData;

        public EnvironmentCache()
        {
            FileData = new SimpleCache<string, CachedFile>(DefaultCacheOptions);
        }
        

        public void Clear()
        {
            FileData.Clear();
        }

        public CachedFile GetFile(FileInfo file)
        {
            return FileData.GetOrCreate(file.FullName, CacheItemFromFile(file));
        }

        public void AddFile(FileInfo file)
        {
            if (file.Exists)
            {
                if (FileData.KeyExists(file.FullName))
                    FileData.Get(file.FullName).Update();
                else
                    FileData.Set(file.FullName, CacheItemFromFile(file));
            }
        }

        public void RemoveFile(FileInfo file)
        {
            FileData.Remove(file.FullName);
        }

        private static CacheItem<CachedFile> CacheItemFromFile(FileInfo file)
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