using System;
using System.Linq;
using System.Reflection;

namespace TempoPlugins
{
    public abstract class Plugin
    {
        public static Plugin LoadPluginFromAssembly(string path)
        {
            var assembly = Assembly.Load(path);
            var attachers = assembly.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(PluginAttacherAttribute), false).Length > 0)
                .ToArray();

            if (attachers.Length == 0)
            {
                throw new NoAttacherException();
            }
            
            if (attachers.Length > 1)
            {
                throw new TooManyAttachersException();
            }

            var attacher = attachers.First();
            var method = attacher.GetMethod("Attach", BindingFlags.Static | BindingFlags.DeclaredOnly);

            if (method == null)
            {
                throw new NoAttachMethodException();
            }

            var plugin = method.Invoke(attacher, null) as Plugin;

            if (plugin == null)
            {
                throw new InvalidAttacherReturnException();
            }

            return plugin;
        }
    }
    
    public class NoAttacherException : Exception { }
    public class TooManyAttachersException : Exception { }
    public class NoAttachMethodException : Exception { }
    public class InvalidAttacherReturnException : Exception { }
}