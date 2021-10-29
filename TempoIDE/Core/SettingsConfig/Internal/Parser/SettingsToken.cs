using Jammo.ParserTools;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser
{
    public class SettingsToken : GenericLexerToken<SettingsTokenId>
    {
        public SettingsToken(string text, StringContext context, SettingsTokenId id) : base(text, context, id)
        {
            
        }
    }

    public static class SettingTokenExtensions
    {
        public static bool IsKeyword(this SettingsToken token)
        {
            return (int)token.Id > 2 && (int)token.Id < 11;
        }
    }
}