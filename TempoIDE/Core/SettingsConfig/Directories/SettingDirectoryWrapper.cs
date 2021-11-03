using System.IO;
using TempoIDE.Core.Helpers;
using TempoIDE.Core.Interfaces;

namespace TempoIDE.Core.SettingsConfig.Directories
{
    public abstract class SettingDirectoryWrapper : IParseWriteStream
    {
        public readonly DirectoryInfo Directory;

        protected SettingDirectoryWrapper(DirectoryInfo directory)
        {
            Directory = directory.CreateIfMissing();
        }

        public abstract void Parse();
        public abstract void Write();
    }
}