using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser
{
    public class SettingsToken : GenericLexerToken<SettingsTokenId>
    {
        public SettingsToken(string text, StringContext context, SettingsTokenId id) : base(text, context, id)
        {
            
        }

        public override string ToString()
        {
            return Text;
        }
    }
}