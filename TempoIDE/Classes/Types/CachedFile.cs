using System.IO;
using Microsoft.CodeAnalysis;

namespace TempoIDE.Classes.Types
{
    public class CachedFile
    {
        public readonly FileInfo File;
        public string Content { get; private set; }

        public CachedFile(FileInfo file)
        {
            File = file;
            
            Update();
        }

        public void Update()
        {
            File.Refresh();

            if (!File.Exists)
                return;
            
            using var file = File.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var buffer = new BufferedStream(file);
            using var reader = new StreamReader(buffer);
            
            Content = reader.ReadToEndAsync().Result;
        }
    }
}