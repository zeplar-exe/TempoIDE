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

            foreach (var pair in LoadedXml)
                if (pair.Value.Root is null)
                    throw new Exception($"Xml document '{pair.Key}' does not have a root.");
        }

        public static XDocument Get(string name)
        {
            return LoadedXml[name];
        }
    }
}