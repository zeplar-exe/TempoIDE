using System;
using System.IO;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Core.SettingsConfig.Directories
{
    public abstract class SettingDirectoryWrapper : IDisposable
    {
        public readonly DirectoryInfo Directory;

        protected SettingDirectoryWrapper(DirectoryInfo directory)
        {
            Directory = directory.CreateIfMissing();
        }
        
        public abstract void Write();

        public abstract void Dispose();
    }
}