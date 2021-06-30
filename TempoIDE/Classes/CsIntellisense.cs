using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CSharp;

namespace TempoIDE.Classes
{
    public static class CsIntellisense
    {
        private static readonly string[] Comments = {"//.*\n", @"/\**.\*/"};

        private static readonly string[] Keywords =
        {
            "abstract", "as", "base", "bool", "break", "by",
            "byte", "case", "catch", "char", "checked", "class", "const",
            "continue", "decimal", "default", "delegate", "do", "double",
            "descending", "explicit", "event", "extern", "else", "enum",
            "false", "finally", "fixed", "float", "for", "foreach", "from",
            "goto", "group", "if", "implicit", "in", "int", "interface",
            "internal", "into", "is", "lock", "long", "new", "null", "namespace",
            "object", "operator", "out", "override", "orderby", "params",
            "private", "protected", "public", "readonly", "ref", "return",
            "switch", "struct", "sbyte", "sealed", "short", "sizeof",
            "stackalloc", "static", "string", "select", "this",
            "throw", "true", "try", "typeof", "uint", "ulong", "unchecked",
            "unsafe", "ushort", "using", "var", "virtual", "volatile",
            "void", "while", "where", "yield"
        };

        private static readonly string[] Operators =
        {
            "+", "-", "*", "/", "%", "&", "(", ")", "[", "]",
            "|", "^", "!", "~", "&&", "||", ",",
            "++", "--", "<<", ">>", "==", "!=", "<", ">", "<=",
            ">=", "=", "+=", "-=", "*=", "/=", "%=", "&=", "|=",
            "^=", "<<=", ">>=", ".", "[]", "()", "?:", "=>", "??"
        };
        
        private static readonly string[] Separator = {";", "{", "}", "\r", "\n", "\r\n"};

        private static Stopwatch debounceTimer = new Stopwatch();

        private static class ColorScheme
        {
            public static readonly Color Comment = Color.FromRgb(73, 138, 72);
            public static readonly Color Keyword = Color.FromRgb(96, 146, 189);
        }

        public static void Highlight(ref RichTextBox textBox)
        {
            return; // TODO: This.
            
            if (debounceTimer.Elapsed < TimeSpan.FromSeconds(5) && debounceTimer.IsRunning)
            {
                debounceTimer.Start();
                return;
            }

            debounceTimer.Restart();
            
            var richText = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd).Text;
            textBox.Document.Blocks.Clear();

            foreach (string word in richText.Split(' '))
            {
                Console.WriteLine(word);
                Console.WriteLine(Keywords.Contains(word));
                if (Keywords.Contains(word))
                    textBox.AppendColoredText(word + " ", "CornflowerBlue");
                else
                    textBox.AppendText(word + " ");
            }
        }
    }
}