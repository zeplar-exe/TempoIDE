using System.Collections.Generic;
using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser.Nodes
{
    public class IfStatement : HeaderedBlockNode
    {
        internal IfStatement(BlockNode block, StringContext context) : base(block, context)
        {
            
        }
    }
}