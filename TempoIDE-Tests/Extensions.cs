using System.IO;

namespace TempoIDE_Tests
{
    public static class Extensions
    {
        public static Stream CreateStream(this string s)
        {
            var stream = new MemoryStream();
            using var writer = new StreamWriter(stream, leaveOpen: true);
            
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            
            return stream;
        }
    }
}