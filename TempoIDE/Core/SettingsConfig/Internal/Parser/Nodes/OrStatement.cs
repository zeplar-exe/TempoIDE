using System.Linq;
using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser.Nodes
{
    public class OrStatement : LogicalStatement
    {
        internal OrStatement(BlockNode block, StringContext context) : base(block, context)
        {
            
        }
        
        public override bool IsTruthy()
        {
            return Values.Any();
        }
    }
}