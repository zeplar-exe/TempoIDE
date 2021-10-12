namespace TempoPlugins
{
    public enum TelTokenId
    {
        Unknown = 0,
            
        Newline,
            
        Identifier,
            
        StringLiteral,
        NumericLiteral,
            
        DefineInstruction,
        AsInstruction,
        ImportInstruction,
        FromInstruction,
            
        ProtocolKeyword,
            
        DelegateOperator,
        Plus, Minus, Multiply, Divide,
        LessThan, LessThanOrEqual,
        GreaterThan, GreaterThanOrEqual,
        Assignment, Equals, Not, NotEqual,
        Period, Comma,
        Or, And,

        OpenParenthesis,
        CloseParenthesis,
        OpenBracket,
        CloseBracket,
        OpenCurlyBracket,
        CloseCurlyBracket,
    }
}