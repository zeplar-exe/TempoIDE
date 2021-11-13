namespace TempoPlugins.Syntax
{
    public static class TokenExtensions
    {
        public static bool IsIdentifier(this TelToken token)
        {
            return !token.IsKeyword() && !token.IsBinary();
        }
        
        public static bool IsKeyword(this TelToken token)
        {
            switch (token.Id)
            {
                case TelTokenId.ProtocolKeyword:
                    return true;
                default:
                    return token.IsInstruction();
            }
        }
        
        public static bool IsInstruction(this TelToken token)
        {
            switch (token.Id)
            {
                case TelTokenId.DefineInstruction:
                case TelTokenId.AsInstruction:
                case TelTokenId.ImportInstruction:
                case TelTokenId.FromInstruction:
                    return true;
                default:
                    return false;
            }
        }
        
        public static bool IsBinary(this TelToken token)
        {
            switch (token.Id)
            {
                case TelTokenId.Assignment:
                    return true;
                default:
                    return token.IsConditional() || token.IsArithmetic();
            }
        }

        public static bool IsArithmetic(this TelToken token)
        {
            switch (token.Id)
            {
                case TelTokenId.Plus:
                case TelTokenId.Minus:
                case TelTokenId.Multiply:
                case TelTokenId.Divide:
                    return true;
                default:
                    return false;
            }
        }
        
        public static bool IsConditional(this TelToken token)
        {
            switch (token.Id)
            {
                case TelTokenId.And:
                case TelTokenId.Or:
                case TelTokenId.LessThan:
                case TelTokenId.LessThanOrEqual: 
                case TelTokenId.GreaterThan: 
                case TelTokenId.GreaterThanOrEqual: 
                case TelTokenId.Not: 
                case TelTokenId.Equals: 
                case TelTokenId.Assignment: 
                case TelTokenId.NotEqual:
                    return true;
                default:
                    return false;
            }
        }
    }
}