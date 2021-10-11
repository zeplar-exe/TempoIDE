using System.Collections.Generic;
using TempoPlugins.Syntax.Nodes;

namespace TempoPlugins.Syntax
{
    public class TelSyntaxTree
    {
        public readonly TelSyntaxNode Root;

        public TelSyntaxTree(TelSyntaxNode root)
        {
            Root = root;
        }

        public IEnumerable<TelSyntaxNode> Enumerate()
        {
            yield return Root;

            foreach (var child in Root.Descendents())
                yield return child;
        }
    }
}