using System.IO;
using System.Threading.Tasks;
using Jammo.ParserTools;
using TempoPlugins.Internal.Lexer;
using TempoPlugins.Internal.Syntax;
using TempoPlugins.Internal.Syntax.Nodes;
using TempoPlugins.Internal.Syntax.Nodes.Expressions;

namespace TempoPlugins.Internal.Parser
{
    public class TelParser
    {
        private readonly EnumerableNavigator<TelToken> navigator;
        private TelSyntaxTree tree;

        public TelParser(string text)
        {
            navigator = new TelLexer(text).Lex().ToNavigator();
            // TODO: Record errors
        }

        public TelParser(Stream stream)
        {
            var reader = new StreamReader(stream);

            navigator = new TelLexer(reader.ReadToEnd()).Lex().ToNavigator();
        }

        public TelSyntaxTree Parse()
        {
            var root = new TelCompilationRoot();
            tree = new TelSyntaxTree(root);
            
            foreach (var token in navigator.EnumerateFromIndex())
            {
                switch (token.Id)
                {
                    case TelTokenId.DefineInstruction:
                    {
                        root.AddNode(ParseDefineSyntax());

                        break;
                    }
                    default:
                    {
                        ReportError("Unexpected token.");
                        break;
                    }
                }
            }

            return tree;
        }

        private DefineSyntax ParseDefineSyntax()
        {
            var syntax = new DefineSyntax { DefineToken = navigator.Current };

            foreach (var token in navigator.EnumerateFromIndex())
            {
                switch (token.Id)
                {
                    case TelTokenId.Identifier:
                        syntax.Identifier = new TelIdentifier { Name = token.ToString() };

                        if (navigator.TakeIf(t => t.Is(TelTokenId.AsInstruction), out _))
                            syntax.Initializer = ParseAsSyntax();

                        return syntax;
                    default:
                        ReportError("Expected an identifier.");
                        break;
                }
            }

            return syntax;
        }

        private AsSyntax ParseAsSyntax()
        {
            var syntax = new AsSyntax { AsToken = navigator.Current };

            if (!navigator.TryMoveNext(out _))
                return syntax;

            syntax.Value = ParseExpressionSyntax();

            return syntax;
        }

        private ExpressionSyntax ParseExpressionSyntax()
        {
            switch (navigator.Current.Id)
            {
                case TelTokenId.StringLiteral:
                    return StringExpressionSyntax.Parse(navigator);
                case TelTokenId.NumericLiteral:
                    return NumericExpressionSyntax.Parse(navigator);
                default:
                    return new UnknownExpressionSyntax(navigator.Current);
            }
        }

        private void ReportError(string message)
        {
            tree.ReportError(message, navigator.Current.Context);
        }
    }
}