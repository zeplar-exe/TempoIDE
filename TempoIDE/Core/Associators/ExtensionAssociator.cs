using System.IO;
using Jammo.TextAnalysis;
using Jammo.TextAnalysis.DotNet.CSharp;
using Jammo.TextAnalysis.Xml;
using TempoControls.Core.CompletionProviders;
using TempoControls.Core.SyntaxSchemes;

namespace TempoIDE.Core.Associators
{
    public static class ExtensionAssociator
    {
        public static ISyntaxScheme SchemeFromExtension(string extension)
        {
            return extension.Replace(".", string.Empty) switch
            {
                "cs" => new CsSyntaxScheme(),
                _ => new DefaultSyntaxScheme()
            };
        }
        
        public static ICompletionProvider CompletionProviderFromExtension(string extension)
        {
            return extension.Replace(".", string.Empty) switch
            {
                "cs" => new CsCompletionProvider(),
                _ => new DefaultCompletionProvider()
            };
        }

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