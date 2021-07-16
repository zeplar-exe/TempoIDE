using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TempoIDE.ProgramData;

namespace TempoIDE.Classes
{
    public static class XmlLoader
    {
        public readonly static Dictionary<string, XDocument> LoadedXml = new Dictionary<string, XDocument>();

        public static void Load()
        {
            LoadedXml.Clear();
            
            
            LoadedXml.Add("intellisense.cs", XDocument.Parse(ProgramFiles.intellisense_cs));
            
            LoadedXml.Add("app.commands", XDocument.Parse(ProgramFiles.app_commands));

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