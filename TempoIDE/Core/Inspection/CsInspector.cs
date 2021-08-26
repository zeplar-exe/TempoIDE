using System;
using JammaNalysis.Compilation;
using TempoControls.Core.Types.Collections;

namespace TempoIDE.Core.Inspection
{
    public class CsInspector : IInspector
    {
        public void Inspect(SyntaxCharCollection characters, MergeableCompilation compilation)
        {
            Console.WriteLine("Inspection successful");
        }
    }
}