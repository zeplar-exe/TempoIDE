using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using TempoIDE.UserControls;

namespace TempoIDE.Classes
{
    public static class CsIntellisense
    {
        public static readonly string[] Keywords =
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
            "void", "while", "where", "yield", "or", "and", "dynamic"
        };

        public static Tuple<string, List<string>> AutoCompleteSuggest(ref SyntaxTextBox textBox)
        {
            return null; // Disabled for now
            
            var range = new TextRange(textBox.Document.ContentStart, textBox.CaretPosition);
            var caretIndex = range.Text.Length - 1;
            // I don't really know why subtracting 1 works, but it does ok
            var richText = textBox.GetPlainText();

            if (string.IsNullOrWhiteSpace(richText))
                return null;
            
            var word = "";

            while (caretIndex >= 0 && (char.IsLetter(richText[caretIndex]) || char.IsNumber(richText[caretIndex])))
            {
                word += richText[caretIndex--];
            }

            var charArray = word.ToCharArray();
            Array.Reverse(charArray);
            word = new string(charArray);
            
            if (string.IsNullOrWhiteSpace(word))
                return null;

            return (word, Keywords.Where(id => id.StartsWith(word) && id != word).ToList()).ToTuple();
        }
    }
}