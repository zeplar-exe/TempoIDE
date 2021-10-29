using System.Linq;
using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser.Nodes
{
    public class XorStatement : LogicalStatement
    {
        internal XorStatement(BlockNode block, StringContext context) : base(block, context)
        {
            
        }

        public override bool IsTruthy()
        {
            return Values.Distinct().Count() == Values.Count();
        }
    }
}