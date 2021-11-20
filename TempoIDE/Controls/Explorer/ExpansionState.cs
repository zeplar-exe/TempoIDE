using System.Collections.Generic;

namespace TempoIDE.Controls.Explorer
{
    public class ExpansionState
    {
        public HashSet<string> ExpandedFiles { get; }

        public static ExpansionState FromExplorer(ExplorerView explorer)
        {
            var set = new HashSet<string>();
            
            foreach (var item in explorer.EnumerateTree<TitledExplorerItem>())
            {
                if (item.IsExpanded)
                    set.Add(item.Uid);
            }
            
            return new ExpansionState(set);
        }

        private ExpansionState(HashSet<string> expandedFiles)
        {
            ExpandedFiles = expandedFiles;
        }
    }
}