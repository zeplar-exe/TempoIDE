using System.Collections.Generic;
using Jammo.ParserTools;
using TempoPlugins.Internal.Syntax.Nodes;

namespace TempoPlugins.Internal.Syntax
{
    public class TelSyntaxTree
    {
        private readonly List<ParserError> errors = new();

        public IReadOnlyCollection<ParserError> Errors => errors.AsReadOnly();

        public readonly TelSyntaxNode Root;

        public TelSyntaxTree(TelSyntaxNode root)
        {
            Root = root;
        }

        public IEnumerable<TelSyntaxNode> Enumerate()
        {
            yield return Root;

            foreach (var child in Root.Descendents())
                yield return child;
        }

        internal void ReportError(string message, StringContext context)
        {
            errors.Add(new ParserError(message, context));
        }
    }
}