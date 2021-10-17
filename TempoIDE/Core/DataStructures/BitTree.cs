using System;
using System.Collections.Generic;

namespace TempoIDE.Core.DataStructures
{
    public class BitTree
    {
        public readonly Bit Root;

        public BitTree(Bit root)
        {
            Root = root;
        }

        public static BitTree FromTree<TItem>(TItem root, Func<TItem, IEnumerable<TItem>> childGetter, Func<TItem, bool> flagGetter)
        {
            var tree = new BitTree(new Bit(flagGetter.Invoke(root)));

            foreach (var child in childGetter.Invoke(root))
                tree.Root.Children.Add(FromTree(child, childGetter, flagGetter).Root);

            return tree;
        }

        public IEnumerable<Bit> EnumerateTree()
        {
            yield return Root;
            
            foreach (var child in Root.EnumerateTree())
                yield return child;
        }
    }
}