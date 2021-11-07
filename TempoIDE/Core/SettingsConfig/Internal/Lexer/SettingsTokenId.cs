namespace TempoIDE.Core.SettingsConfig.Internal.Lexer
{
    public enum SettingsTokenId
    {
        Unknown = 0,
        
        Identifier,
        
        StringLiteral,
        NumericLiteral,
        
        Comment,
        
        BooleanTrue,
        BooleanFalse,
        
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
        OpenBracket,
        CloseBracket,
        OpenCurlyBracket,
        CloseCurlyBracket,
        Period,
        Equals,
        
        DoubleQuote
    }
}