using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace TempoIDE.Classes
{
    public static class ResourceCache
    {
        private static readonly Dictionary<string, string> LoadedText = new Dictionary<string, string>();
        private static readonly Dictionary<string, XDocument> LoadedXml = new Dictionary<string, XDocument>();

        private static readonly Dictionary<string, string> RequiredFiles = new Dictionary<string, string>
        {
            {"app.commands", "TempoIDE.Resources.Configs.app-commands.xml"},
            
            {"intellisense.cs", "TempoIDE.Resources.Configs.intellisense-cs.xml"},
        };

        public static void Load()
        {
            LoadedXml.Clear();
            
            var assembly = Assembly.GetExecutingAssembly();
            
            foreach (var (name, path) in RequiredFiles)
            {
                using var stream = assembly.GetManifestResourceStream(path);
                using var reader = new StreamReader(stream);

                var text = reader.ReadToEnd();
                
                switch (Path.GetExtension(path))
                {
                    case ".xml":
                        LoadedXml.Add(name, XDocument.Parse(text));
                        break;
                    default:
                        LoadedText.Add(name, text);
                        break;
                }
            }

            foreach (var (key, document) in LoadedXml)
                if (document.Root is null)
                    throw new Exception($"Xml document '{key}' does not have a root.");
        }

        public static XDocument GetXml(string name)
        {
            return LoadedXml[name];
        }

        public static string GetText(string name)
        {
            return LoadedText[name];
        }
    }
}