using TempoIDE.Core.SettingsConfig.Internal.Lexer;

namespace TempoIDE.Core.SettingsConfig.Internal.Parser
{
    public static class SettingTokenExtensions
    {
        public static bool IsLiteral(this SettingsToken token)
        {
            return token.Id is
                SettingsTokenId.StringLiteral or 
                SettingsTokenId.NumericLiteral or
                SettingsTokenId.BooleanTrue or
                SettingsTokenId.BooleanFalse;
        }
        
        public static bool IsKeyword(this SettingsToken token)
        {
            return IsControl(token) || IsLogical(token);
        }
        
        public static bool IsControl(this SettingsToken token)
        {
            return token.Id is SettingsTokenId.IfKeyword;
        }

        public static bool IsLogical(this SettingsToken token)
        {
            return token.Id is
                SettingsTokenId.OrKeyword or SettingsTokenId.AndKeyword or
                SettingsTokenId.NorKeyword or SettingsTokenId.NandKeyword or
                SettingsTokenId.XorKeyword or SettingsTokenId.XandKeyword;
        }
    }
}