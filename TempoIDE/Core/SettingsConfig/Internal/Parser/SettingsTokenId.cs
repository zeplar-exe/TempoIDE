namespace TempoIDE.Core.SettingsConfig.Internal.Parser
{
    public enum SettingsTokenId
    {
        Unknown = 0,
        
        Identifier,
        
        IsOperator,
        IfKeyword,
        OrKeyword,
        AndKeyword,
        NorKeyword,
        NandKeyword,
        XorKeyword,
        XandKeyword,
        
        OpenParenthesis, 
        CloseParenthesis,
        OpenCurlyBracket,
        CloseCurlyBracket,
        Period,
        Equals
    }
}