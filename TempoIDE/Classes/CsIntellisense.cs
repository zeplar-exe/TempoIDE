using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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
        private static string[] identifiers =
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
        private static class ColorScheme
        {
            public static readonly Color Comment = Color.FromRgb(73, 138, 72);
            public static readonly Color Keyword = Color.FromRgb(96, 146, 189);
        }

        public static void Highlight(ref RichTextBox textBox)
        {
            
        }

        public static List<string> Suggest(ref RichTextBox textBox)
        {
            var range = new TextRange(textBox.Document.ContentStart, textBox.CaretPosition);
            var caretIndex = range.Text.Length - 1;
            // I don't really know why subtracting 1 works, but it does ok
            var richText = textBox.GetPlainText();

            if (richText.Length == 0)
                return null;
            
            var word = "";

            while (caretIndex >= 0 && (char.IsLetter(richText[caretIndex]) || char.IsNumber(richText[caretIndex])))
            {
                word += richText[caretIndex--];
            }

            var charArray = word.ToCharArray();
            Array.Reverse(charArray);
            word = new string(charArray);

            if (string.IsNullOrEmpty(word))
                return null;
            
            return identifiers.Where(id => id.StartsWith(word) && id != word).ToList();
        }
    }
}