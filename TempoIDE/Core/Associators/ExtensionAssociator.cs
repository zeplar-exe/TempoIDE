using System.IO;
using Jammo.TextAnalysis;
using Jammo.TextAnalysis.DotNet.CSharp;
using Jammo.TextAnalysis.Xml;

namespace TempoIDE.Core.Associators
{
    public static class ExtensionAssociator
    {
        public static AnalysisCompilation AnalysisCompilationFromFile(FileInfo file)
        {
            switch (file.Extension)
            {
                case ".cs":
                {
                    return CSharpAnalysisCompilationHelper.Create(file.FullName, AnalysisType.File);
                }
                case ".xml":
                {
                    var comp = new XmlAnalysisCompilation();
                    comp.AppendFile(file);
                    comp.GenerateCompilation();

                    return comp;
                }
                default:
                    return null;
            }
        }
    }
}