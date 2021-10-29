using System.Collections.Generic;
using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser.Nodes
{
    public class BlockNode : SettingsNode
    {
        internal BlockNode(StringContext context) : base(context)
        {
            
        }
        
        private readonly List<SettingsNode> nodes = new();

        public override IEnumerable<SettingsNode> Nodes => nodes;

        public void AddNode(SettingsNode node) => nodes.Add(node);
        public void RemoveNode(SettingsNode node) => nodes.Remove(node);
    }
}