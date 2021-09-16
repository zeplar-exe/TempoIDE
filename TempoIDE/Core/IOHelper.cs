using System;
using System.IO;

namespace TempoIDE.Core
{
    // ReSharper disable once InconsistentNaming
    public static class IOHelper
    {
        public static bool TryOpenRelativeFile(
            string path,
            out FileStream result,
            out Exception exception)
        {
            result = null;
            exception = null;
            
            try
            {
                result = File.OpenRead(GetRelativePath(path));
            }
            catch (FileNotFoundException e)
            {
                exception = e;
                
                return false;
            }
            catch (DirectoryNotFoundException e)
            {
                exception = e;
                
                return false;
            }

            return true;
        }

        public static string ReadFullStream(FileStream stream)
        {
            using var reader = new StreamReader(stream);

            return reader.ReadToEndAsync().Result;
        }

        public static string GetRelativePath(string path)
        {
            return Path.Join(Directory.GetCurrentDirectory(), path);
        }
        
        public static bool RelativeFileExists(string path)
        {
            return new FileInfo(GetRelativePath(path)).Exists;
        }

        public static bool RelativeDirectoryExists(string path)
        {
            return new DirectoryInfo(GetRelativePath(path)).Exists;
        }
    }
}