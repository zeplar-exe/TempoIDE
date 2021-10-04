using System;
using System.IO;
using Jammo.ParserTools;

namespace TempoPlugins
{
    public class TelStream : IParserStream
    {
        private FileStream stream;

        public bool IsInitialized => stream == null;
        public string FilePath => stream.Name;

        public TelStream(FileStream stream = null)
        {
            this.stream = stream;
        }

        public void Parse()
        {
            throw new NotImplementedException();
        }

        public void Write()
        {
            throw new NotImplementedException();
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