using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Xml.Linq;
using TempoIDE.UserControls;

namespace TempoIDE.Classes
{
    public static class ResourceCache
    {
        private static readonly Dictionary<string, XDocument> LoadedXml = new Dictionary<string, XDocument>();

        private static readonly Dictionary<string, string> RequiredFiles = new Dictionary<string, string>
        {
            {"app.commands", "TempoIDE.Resources.app-commands.xml"},
            
            {"intellisense.cs", "TempoIDE.Resources.intellisense-cs.xml"},
        };

        public static void Load()
        {
            LoadedXml.Clear();
            
            var assembly = Assembly.GetExecutingAssembly();
            
            foreach (var (name, path) in RequiredFiles)
            {
                using var stream = assembly.GetManifestResourceStream(path);
                using var reader = new StreamReader(stream);
                
                LoadedXml.Add(name, XDocument.Parse(reader.ReadToEnd()));
            }

            foreach (var (key, document) in LoadedXml)
                if (document.Root is null)
                    throw new Exception($"Xml document '{key}' does not have a root.");
        }

        public static XDocument Get(string name)
        {
            return LoadedXml[name];
        }
    }
}