using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TempoIDE.Plugins.Core
{
    public abstract class Plugin
    {
        public static Plugin ReflectPluginFromAssembly(string path)
        {
            var assembly = Assembly.Load(File.ReadAllBytes(path));
            Type[] attachers;
            
            try
            {
                attachers = assembly.GetTypes()
                    .Where(t => t.GetCustomAttributes(typeof(PluginAttacherAttribute), false).Length > 0)
                    .ToArray();
            }
            catch (ReflectionTypeLoadException)
            {
                throw new InvalidPluginPackageException();
            }
            
            switch (attachers.Length)
            {
                case 0:
                    throw new NoAttacherException();
                case > 1:
                    throw new TooManyAttachersException();
            }

            var attacher = attachers.First();
            var method = attacher.GetMethod("Attach");

            if (method == null)
            {
                throw new NoAttachMethodException();
            }

            if (method.Invoke(attacher, null) is not Plugin plugin)
            {
                throw new InvalidAttacherReturnException();
            }

            return plugin;
        }
    }
    
    public class InvalidPluginPackageException : Exception { }
    public class NoAttacherException : Exception { }
    public class TooManyAttachersException : Exception { }
    public class NoAttachMethodException : Exception { }
    public class InvalidAttacherReturnException : Exception { }
}