using System.IO;

namespace TempoIDE.Core.Helpers
{
    public static class AppDataHelper
    {
        public static DirectoryInfo Directory => new DirectoryInfo(IOHelper.GetRelativePath(@"appdata")).CreateIfMissing();
    }
}