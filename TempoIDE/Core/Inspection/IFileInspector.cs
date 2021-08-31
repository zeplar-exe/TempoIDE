using JammaNalysis.Compilation;
using TempoControls.Core.Types.Collections;

namespace TempoIDE.Core.Inspection
{
    public interface IFileInspector
    {
        public void Inspect(SyntaxCharCollection characters, MergeableCompilation compilation);
    }

    public class DefaultFileInspector : IFileInspector
    {
        public void Inspect(SyntaxCharCollection characters, MergeableCompilation compilation)
        {
            
        }
    }
}