using System.IO;
using System.Linq;
using System.Reflection;

namespace TempoIDE.Core.Helpers
{
    public static class ResourceHelper
    {
        public static Stream GetResourceFile(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var file = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name));
            
            return assembly.GetManifestResourceStream(file);
        }
    }
}