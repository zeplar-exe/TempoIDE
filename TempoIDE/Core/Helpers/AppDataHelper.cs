using System.IO;

namespace TempoIDE.Core.Helpers;

public static class AppDataHelper
{
    public static DirectoryInfo Directory => new(IOHelper.GetRelativePath("data")); 
    //TODO: Use Users/AppData
        
    public static FileStream GetFile(string path)
    {
        return File.OpenRead(Path.Join(Directory.FullName, path));
    }
}