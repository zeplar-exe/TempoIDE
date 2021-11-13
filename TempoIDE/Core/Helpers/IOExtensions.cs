using System.IO;

namespace TempoIDE.Core.Helpers
{
    // ReSharper disable once InconsistentNaming
    public static class IOExtensions
    {
        public static DirectoryInfo ToRelativeDirectory(this DirectoryInfo directoryInfo, string relative)
        {
            return new DirectoryInfo(Path.Join(directoryInfo.FullName, relative));
        }

        public static DirectoryInfo CreateIfMissing(this DirectoryInfo directoryInfo)
        {
            if (!directoryInfo.Exists)
                directoryInfo.Create();

            return directoryInfo;
        }
        
        public static FileInfo CreateIfMissing(this FileInfo fileInfo)
        {
            if (!fileInfo.Exists)
                fileInfo.Create().Dispose();

            return fileInfo;
        }
        
        public static FileInfo ToFile(this DirectoryInfo directoryInfo, string name)
        {
            return new FileInfo(Path.Join(directoryInfo.FullName, name));
        }

        public static FileStream OpenOrCreate(this FileInfo file, FileMode mode, FileAccess access, FileShare share)
        {
            if (!file.Exists)
            {
                Directory.CreateDirectory(file.DirectoryName);
                
                file.Create().Dispose();
                file.Refresh();
            }
            
            return file.Open(mode, access, share);
        }
    }
}