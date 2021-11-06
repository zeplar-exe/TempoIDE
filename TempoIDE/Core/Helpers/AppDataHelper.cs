using System.IO;

namespace TempoIDE.Core.Helpers
{
    public static class AppDataHelper
    {
        public static DirectoryInfo Directory => new(IOHelper.GetRelativePath(@"appdata\settings")); 
        public static bool Exists => Directory.Exists;
        
        public static FileStream GetFile(string path)
        {
            return File.OpenRead(Directory.ToFile(path).FullName);
        }
        
        public static bool TryGetDataDirectory(out DirectoryInfo info)
        {
            info = Directory;
            
            return Exists;
        }
    }
}