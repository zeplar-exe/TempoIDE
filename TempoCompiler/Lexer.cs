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
        ConstModifier,
        ReservedIdentifierModifier,
        BuiltinType,

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

        OverrideModifier,
        SealedModifier,
        ReadonlyModifier,
        ExternModifier,
        AbstractModifier,
        VirtualModifier,
        AsyncModifier,
        StaticModifier,
        UnsafeModifier,
        VolatileModifier,
        
        FixedKeyword,
        StackAllocKeyword,
        
        ThisLiteral,
        TrueLiteral,
        FalseLiteral,
        NullLiteral,

        NewKeyword,
        AsKeyword,
        IsKeyword,
        
        SizeofKeyword,
        TypeofKeyword,
        
        NamespaceKeyword,
        ClassKeyword,
        StructKeyword,
        EnumKeyword,
        DelegateKeyword,
        
        OperatorKeyword,
        
        InKeyword,
        OutKeyword,
        RefKeyword,

        ForKeyword,
        ForeachKeyword,
        WhileKeyword,
        DoKeyword,
        
        ContinueKeyword,
        BreakKeyword,
        ReturnKeyword,
        
        IfKeyword,
        ElseKeyword,
        
        SwitchKeyword,
        CaseKeyword,
        GotoKeyword,
        
        DefaultKeyword,
        BaseKeyword,
        
        ThrowKeyword,
        CatchKeyword,
        FinallyKeyword,
        UsingKeyword,
        
        UncheckedKeyword,
        CheckedKeyword,

        Period, Comma, Colon, Semicolon,
        LeftBrace, RightBrace, 
        LeftBracket, RightBracket, 
        LeftParenthesis, RightParenthesis
    }
}