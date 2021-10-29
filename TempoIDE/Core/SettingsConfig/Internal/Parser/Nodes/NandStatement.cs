using System.Linq;
using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser.Nodes
{
    public class NandStatement : LogicalStatement
    {
        internal NandStatement(BlockNode block, StringContext context) : base(block, context)
        {
            
        }
        
        public override bool IsTruthy()
        {
            return !Values.All(t => t);
        }
    }
}