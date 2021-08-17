namespace TempoCompiler
{
    public enum LexerTokenType
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
        PartialModifier,
        
        AwaitKeyword,
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
        RecordKeyword,
        
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
        
        
        AddContextKeyword,
        AndContextKeyword,
        AliasContextKeyword,
        AscendingContextKeyword,
        ByContextKeywordContextKeyword,
        DescendingContextKeyword,
        EqualsContextKeyword,
        FromContextKeyword,
        GetContextKeyword,
        GlobalContextKeyword,
        GroupContextKeyword,
        InitContextKeyword,
        IntoContextKeyword,
        JoinContextKeyword,
        LetContextKeyword,
        Not,
        NInt,
        NuIntContextKeyword,
        NotNullContextKeyword,
        OnContextKeyword,
        OrContextKeyword,
        OrderbyContextKeyword,
        RemoveContextKeyword,
        SelectContextKeyword,
        SetContextKeyword,
        ValueContextKeyword,
        WhenContextKeyword,
        WhereContextKeyword,
        WithContextKeyword,
        YieldContextKeyword,
        

        Period, Comma, Colon, Semicolon,
        LeftBrace, RightBrace, 
        LeftBracket, RightBracket, 
        LeftParenthesis, RightParenthesis
    }
}