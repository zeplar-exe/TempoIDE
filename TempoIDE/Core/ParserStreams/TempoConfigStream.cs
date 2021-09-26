using System;
using System.IO;
using Jammo.ParserTools;
using TempoIDE.Core.Wrappers;

namespace TempoIDE.Core.ParserStreams
{
    public class TempoConfigStream : IParserStream
    {
        private readonly DirectoryInfo directory;

        public bool IsInitialized => true;
        public string FilePath => directory.FullName;

        public readonly DistinctCollection<string> Excluded = new();

        public TempoConfigStream(string directory)
        {
            this.directory = new DirectoryInfo(directory);

            if (!this.directory.Exists)
                throw new ArgumentException("Expected a directory.");
        }

        public bool QueryExcluded(string path)
        {
            return Excluded.Contains(path);
        }

        public void Exclude(string path)
        {
            Excluded.Add(path);
        }

        public void Restore(string path)
        {
            Excluded.Remove(path);
        }

        public void Parse()
        {
            Excluded.Clear();
            
            foreach (var excludeFile in directory.GetFiles("exclude.txt"))
            {
                using var file = excludeFile.OpenRead();
                using var reader = new StreamReader(file);
                
                while (!reader.EndOfStream)
                    Excluded.Add(reader.ReadLineAsync().Result);
            }
        }

        public void Write()
        {
            File.WriteAllLines(Path.Join(directory.FullName, "exclude.txt"), Excluded);
        }

        public void WriteTo(string path)
        {
            throw new NotImplementedException();
        }
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}