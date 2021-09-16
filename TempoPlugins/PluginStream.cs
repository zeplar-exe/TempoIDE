using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Jammo.ParserTools;
using Newtonsoft.Json.Linq;

namespace TempoPlugins
{
    public class PluginStream : IParserStream
    {
        private FileStream stream;

        public bool IsInitialized => stream == null;
        public string FilePath => stream.Name;
        
        public string Version = "";
        public readonly Dictionary<string, string> Metadata = new();

        public PluginStream(FileStream stream = null)
        {
            this.stream = stream;
        }

        public void Parse()
        {
            throw new NotImplementedException();
        }

        public void Write()
        {
            if (stream == null)
            {
                var working = Directory.GetCurrentDirectory();

                Console.WriteLine("The current stream is null, a new file will be created in the working directory." +
                                  $"Current working directory: {working}");
                
                stream = File.Create(Path.Join(Directory.GetCurrentDirectory(), "Jammo_SolutionStream.sln"));
            }
            
            var writer = new StreamWriter(stream);
            stream.SetLength(0);
            
            writer.WriteLine(Version);
            writer.WriteLine();
            writer.WriteLine(Metadata.ToString());
        }

        public void WriteTo(string path)
        {
            using var file = File.Create(path);
            using var writer = new StreamWriter(file);
            
            file.SetLength(0);
            writer.Write(ToString());
        }

        public void Dispose()
        {
            stream?.Dispose();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine(Version);
            builder.AppendLine();
            builder.AppendLine(Metadata.ToString());

            return builder.ToString();
        }
    }
}