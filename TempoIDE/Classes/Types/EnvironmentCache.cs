using System;
using System.Collections.Generic;
using System.IO;

namespace TempoIDE.Classes.Types
{
    public class EnvironmentCache
    {
        private readonly Dictionary<string, CachedFile> fileData = new Dictionary<string, CachedFile>();

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