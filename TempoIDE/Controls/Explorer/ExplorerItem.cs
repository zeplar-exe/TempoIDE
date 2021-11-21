using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace TempoIDE.Controls.Explorer
{
    public abstract class ExplorerItem : TreeViewItem
    {
        public void Add(ExplorerItem item)
        {
            AddChild(item);
        }

        public void Add(IEnumerable<ExplorerItem> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public IEnumerable<TExplorerItem> EnumerateTree<TExplorerItem>() where TExplorerItem : ExplorerItem
        {
            foreach (var item in Items.OfType<TExplorerItem>())
            {
                yield return item;

                foreach (var nested in item.EnumerateTree<TExplorerItem>())
                    yield return nested;
            }
        }
    }
}