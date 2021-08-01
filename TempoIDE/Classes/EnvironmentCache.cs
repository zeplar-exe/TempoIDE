using System.Collections.Generic;
using System.IO;
using TempoIDE.Classes.Types;

namespace TempoIDE.Classes
{
    public class EnvironmentCache
    {
        private readonly Dictionary<string, string> files = new Dictionary<string, string>();

        public string GetFile(string path)
        {
            return files.ContainsKey(path) ? files[path] : null;
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

                using var reader = new StreamReader(path.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                files[path.FullName] = reader.ReadToEnd();
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
                if (files.ContainsKey(path.FullName))
                    files.Remove(path.FullName);
            }
        }
    }
}