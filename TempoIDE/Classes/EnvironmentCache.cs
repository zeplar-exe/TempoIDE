using System;
using System.Collections.Generic;
using System.IO;

namespace TempoIDE.Classes
{
    public class EnvironmentCache
    {
        private readonly Dictionary<string, string> fileData = new Dictionary<string, string>();

        public string GetFile(FileInfo file)
        {
            return fileData.ContainsKey(file.FullName) ? fileData[file.FullName] : null;
        }

        public void AddFile(FileInfo path)
        {
            AddFile(new [] { path });
        }

        public void AddFile(FileInfo[] paths)
        {
            foreach (var path in paths)
            {
                if (!path.Exists)
                    continue;

                using var file = path.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var buffer = new BufferedStream(file);
                using var reader = new StreamReader(buffer);
                
                fileData[path.FullName] = reader.ReadToEnd();
            }
        }

        public void RemoveFile(FileInfo path)
        {
            RemoveFile(new [] { path });
        }
        
        public void RemoveFile(FileInfo[] paths)
        {
            foreach (var path in paths)
            {
                if (fileData.ContainsKey(path.FullName))
                    fileData.Remove(path.FullName);
            }
        }
    }
}