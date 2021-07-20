namespace TempoCompiler.Cs
{
    public enum TokenType
    {
        Unknown = 0,
        
        Abstract, As, Async, 
        Base, Bool, Break, Byte,
        Case, Catch, Char, Checked, Class, Const, Continue,
        Decimal, Default, Delegate, Do, Double, 
        Else, Enum, Event, Explicit, Extern,
        False, Finally, Fixed, Float, For, Foreach, 
        Goto, 
        If, 
        Implicit, In, Int, Interface, Internal, Is,
        Lock, Long, 
        Namespace, New, 
        Null, Object, Operator, Out, Override,
        Params, Private, Protected, Public,
        Readonly, Ref, Return,
        Sbyte, Sealed, Short, Sizeof, Stackalloc, Static, String, Struct, Switch, 
        This, Throw, True, Try, Typeof, 
        Uint, Ulong, Unchecked, Unsafe, Ushort, Using, 
        Virtual, Void, Volatile, 
        While, Where,
        
        Identifier, Number,
        Period, Comma, DoubleQuote, SingleQuote,
        LeftParen, RightParen,
        LeftBracket, RightBracket,
        BlockOpen, BlockClose,
        
        Whitespace,
        EndOfFile
    }
}