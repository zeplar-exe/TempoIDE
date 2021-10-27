namespace TempoIDE.Core.SettingsConfig.Internal.Parser
{
    public enum SettingsTokenId
    {
        Identifier,
        
        IsOperator,
        IfKeyword,
        OrKeyword,
        AndKeyword,
        NorKeyword,
        NandKeyword,
        XorKeyword,
        XandKeyword,
        NotKeyword,
        
        OpenParenthesis, 
        CloseParenthesis,
        OpenCurlyBracket,
        CloseCurlyBracket,
        Period,
        Equals
    }
}