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

        public bool TryGetNamespace(string name, out CompilationNamespace result)
        {
            if (!Name.Contains(name))
            {
                result = null;
                return false;
            }

            CompilationNamespace currentSearch = this;
            
            foreach (var namePoint in name.Substring(0, Name.Length).Split('.'))
            {
                var matched = false;
                
                foreach (var ns in Namespaces)
                {
                    if (ns.Name == currentSearch.Name + "." + namePoint)
                    {
                        currentSearch = ns;
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    result = null;
                    return false;
                }
            }

            result = currentSearch;
            return true;
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