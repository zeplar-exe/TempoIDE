using JammaNalysis.Compilation;
using TempoControls.Core.Types.Collections;

namespace TempoIDE.Core.Inspection
{
    public interface IInspector
    {
        public void Inspect(SyntaxCharCollection characters, MergeableCompilation compilation);
    }

    public class DefaultInspector : IInspector
    {
        public void Inspect(SyntaxCharCollection characters, MergeableCompilation compilation)
        {
            
        }
    }
}