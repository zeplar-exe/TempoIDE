namespace TempoCompiler
{
    public class Lexer
    {
        // TODO: Work on this to allow easy access to file elements
    }

    public enum TokenKind
    {
        Unknown = 0,
        
        Identifier,
        ReservedIdentifierModifier,

        Numeric,
        NumericModifier,
        
        Character,
        String,
        VerbatimStringModifier,
        InterpolationStringModifier,
        
        
        PlusOperator, MinusOperator, MultiplyOperator, DivideOperator, ExponentOperator, ModuloOperator,
        OrOperator, AndOperator, BitwiseOrOperator, BitwiseAndOperator,
        EqualsOperator, DoesNotEqualOperator, NotOperator,
        LessThanOperator, LessThanOrEqualToOperator, MoreThanOperatorOperator, MoreThanOrEqualToOperator,
        NullCoalesceOperator, NullCoalesceAssignmentOperator, TernaryOperator,
        
        
        PublicModifier,
        PrivateModifier,
        InternalModifier,
        ProtectedModifier,
        
        AbstractModifier,
        VirtualModifier,
        AsyncModifier,
        StaticModifier,
        
        NamespaceKeyword,
        
        NewKeyword,
        AsKeyword,
        CaseKeyword,
        ClassKeyword,
        DefaultKeyword,
        DelegateKeyword,

        ForKeyword,
        ForeachKeyword,
        WhileKeyword,
        DoKeyword,
        ContinueKeyword,
        
        IfKeyword,
        ElseKeyword,
        

        Period, Comma, Colon, Semicolon,
        LeftBrace, RightBrace, 
        LeftBracket, RightBracket, 
        LeftParenthesis, RightParenthesis
    }
}