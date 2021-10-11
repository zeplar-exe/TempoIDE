using System;
using Jammo.ParserTools;

namespace TempoPlugins.Syntax.Nodes.Expressions
{
    public abstract class ExpressionSyntax : TelSyntaxNode
    {
        public static ExpressionSyntax Parse(EnumerableNavigator<TelToken> navigator)
        {
            var token = navigator.Current;

            do
            {
                switch (token.Id)
                {
                    case TelTokenId.Whitespace:
                        continue;
                    case TelTokenId.StringLiteral:
                        return new StringExpressionSyntax { Literal = token };
                    case TelTokenId.NumericLiteral:
                        return new NumericExpressionSyntax { Literal = token };
                    default:
                        return new UnknownExpressionSyntax { Token = token };
                        
                }
            } while (navigator.TryMoveNext(out token));
            
            return null;
        }
    }
}