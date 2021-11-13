using System.Linq;
using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser.Nodes
{
    public class XandStatement : LogicalStatement
    {
        internal XandStatement(BlockNode block, StringContext context) : base(block, context)
        {
            
        }

        public override bool IsTruthy()
        {
            return Values.All(t => t);
        }
    }
}