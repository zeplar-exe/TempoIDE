using System.IO;

namespace TempoIDE.Core.Helpers
{
    // ReSharper disable once InconsistentNaming
    public static class IOExtensions
    {
        public static DirectoryInfo ToNestedDirectory(this DirectoryInfo directoryInfo, string relative)
        {
            return new DirectoryInfo(Path.Join(directoryInfo.FullName, relative));
        }
        
        public static FileInfo ToFile(this DirectoryInfo directoryInfo, string relative)
        {
            return new FileInfo(Path.Join(directoryInfo.FullName, relative));
        }

        public static FileStream OpenOrCreate(this FileInfo file, FileMode mode, FileAccess access, FileShare share)
        {
            if (!file.Exists)
            {
                file.Create().Dispose();
                file.Refresh();
            }
            
            return file.Open(mode, access, share);
        }
    }
}