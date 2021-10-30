namespace TempoIDE.Core.SettingsConfig.Internal.Parser
{
    public enum SettingsTokenId
    {
        Unknown = 0,
        
        Identifier,
        
        StringLiteral,
        NumericLiteral,
        
        Comment,
        
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
        Equals,
        
        DoubleQuote
    }
}