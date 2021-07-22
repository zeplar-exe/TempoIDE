using System;
using System.IO;

namespace TempoCompiler.Cs
{
    public class Compiler
    {
        public Compiler(FileInfo[] files)
        {
            foreach (var token in new Lexer(TestResources.TestInputFile))
                Console.WriteLine(token.Value);
        }
    }
}