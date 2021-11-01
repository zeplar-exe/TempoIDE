using System;
using System.IO;
using TempoIDE.Core.Helpers;

namespace TempoIDE.Core.SettingsConfig
{
    public abstract class SettingDirectoryWrapper
    {
        protected readonly DirectoryInfo Directory;

        protected SettingDirectoryWrapper(DirectoryInfo directory)
        {
            Directory = directory.CreateIfMissing();
        }

        public abstract void Parse();
        public abstract void Write();
    }
}