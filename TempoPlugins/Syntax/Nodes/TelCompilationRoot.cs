using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TempoPlugins.Syntax.Nodes
{
    public class TelCompilationRoot : TelSyntaxNode
    {
        public IEnumerable<ParserError> GetAllErrors()
        {
            foreach (var error in Errors)
                yield return error;

            foreach (var descendent in Descendents())
            {
                foreach (var nestedError in descendent.Errors)
                    yield return nestedError;
            }
        }
    }
}