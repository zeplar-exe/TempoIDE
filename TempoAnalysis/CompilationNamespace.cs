using System.Collections.Generic;

namespace TempoAnalysis
{
    public class CompilationNamespace
    {
        public string Name;

        public readonly List<CompilationNamespaceType> Types = new();
        public readonly List<CompilationNamespace> Namespaces = new();

        public CompilationNamespace(string name)
        {
            Name = name;
        }

        public IEnumerable<CompilationNamespace> EnumerateTree()
        {
            foreach (var comp in Namespaces)
            {
                yield return comp;
                
                foreach (var nestedComp in comp.EnumerateTree())
                    yield return nestedComp;
            }
        }
    }
}