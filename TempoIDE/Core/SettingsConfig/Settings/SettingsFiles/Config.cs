using System;
using System.IO;
using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Settings.SettingsFiles
{
    public abstract class Config : IDisposable
    {
        protected readonly Stream Stream;

        protected Config(Stream stream)
        {
            Stream = stream;
        }

        protected Config(FileStream stream)
        {
            Stream = stream;
            FilePath = stream.Name;
        }
        
        public string FilePath { get; }
        public bool IsInitialized => Stream?.CanRead ?? false;

        public abstract void Parse();
        public abstract void Write();
        
        public void Dispose()
        {
            Stream.Dispose();
        }
    }
}