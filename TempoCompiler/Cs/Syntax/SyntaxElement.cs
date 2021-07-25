using System.Collections;
using System.Collections.Generic;

namespace TempoCompiler.Cs.Syntax
{
    public class SyntaxElement : IEnumerable<SyntaxElement>
    {
        public SyntaxElement Parent;
        public List<SyntaxElement> Children = new List<SyntaxElement>(50);
        public IEnumerator<SyntaxElement> GetEnumerator()
        {
            foreach (var child in Children)
            {
                yield return child;
                
                foreach (var grandChild in child.Children)
                    yield return grandChild;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}