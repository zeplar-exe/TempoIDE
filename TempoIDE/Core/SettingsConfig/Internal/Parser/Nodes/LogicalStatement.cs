using System.Collections.Generic;
using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser.Nodes
{
    public abstract class LogicalStatement : HeaderedBlockNode, ITruthy
    {
        public readonly IEnumerable<bool> Values;
        
        protected LogicalStatement(BlockNode block, StringContext context) : base(block, context)
        {
            var values = new List<bool>();
            
            foreach (var node in block.Nodes)
            {
                if (node is not ITruthy truthy)
                {
                    ReportError("Expected an expression that evaluates to truthy or falsy.");
                    continue;
                }
                
                values.Add(truthy.IsTruthy());
            }

            Values = values;
        }

        public abstract bool IsTruthy();
    }
}