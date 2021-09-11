using Jammo.CsAnalysis.Compilation;
using TempoControls.Core.Types.Collections;

namespace TempoIDE.Core.Inspections
{
    public interface IFileInspector
    {
        public void Inspect(SyntaxCharCollection characters, CompilationWrapper compilation);

        public static IFileInspector FromExtension(string extension)
        {
            return extension.Replace(".", string.Empty) switch
            {
                "cs" => new CsFileInspector(),
                _ => new DefaultFileInspector()
            };
        }
    }

    public class DefaultFileInspector : IFileInspector
    {
        public void Inspect(SyntaxCharCollection characters, CompilationWrapper compilation)
        {
            
        }
    }
}