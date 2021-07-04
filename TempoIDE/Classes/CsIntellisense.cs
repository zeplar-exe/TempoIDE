using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

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
            "void", "while", "where", "yield", "or", "and"
        };

        private static readonly string[] Operators =
        {
            "+", "-", "*", "/", "%", "&", "(", ")", "[", "]",
            "|", "^", "!", "~", "&&", "||", ",",
            "++", "--", "<<", ">>", "==", "!=", "<", ">", "<=",
            ">=", "=", "+=", "-=", "*=", "/=", "%=", "&=", "|=",
            "^=", "<<=", ">>=", ".", "[]", "()", "?:", "=>", "??"
        };
        
        private static readonly string[] Separator =
        {
            ";", "{", "}", "\r", "\n", "\r\n", " ",
            "+", "-", "*", "/", "%", "&", "(", ")", "[", "]",
            "|", "^", "!", "~", "&&", "||", ",",
            "++", "--", "<<", ">>", "==", "!=", "<", ">", "<=",
            ">=", "=", "+=", "-=", "*=", "/=", "%=", "&=", "|=",
            "^=", "<<=", ">>=", ".", "[]", "()", "?:", "=>", "??"
        };
        
        private static class ColorScheme
        {
            public static readonly Brush Default = Brushes.White;
            public static readonly Brush Number = Brushes.LightCoral;
            public static readonly Brush Comment = Brushes.ForestGreen;
            public static readonly Brush Identifier = Brushes.CornflowerBlue;
            public static readonly Brush Type = Brushes.MediumPurple;
            public static readonly Brush Method = Brushes.LightGreen;
            public static readonly Brush Member = Brushes.CadetBlue;
        }

        public static void Highlight(ref RichTextBox textBox)
        {
            var richText = textBox.GetPlainText();
            var readingWord = "";
            
            var startPoint = textBox.Document.ContentStart;
            var caretOffset = startPoint.GetOffsetToPosition(textBox.CaretPosition);

            textBox.Document.Blocks.Clear();
            
            foreach (var character in richText)
            {
                Console.WriteLine("--" + character + "--");
                if (char.IsLetter(character))
                {
                    readingWord += character;
                }
                else
                {
                    if (identifiers.Contains(readingWord))
                    {
                        var newRange = new TextRange(textBox.Document.ContentEnd, textBox.Document.ContentEnd);
                        newRange.Text = readingWord;
                        newRange.ApplyPropertyValue(TextElement.ForegroundProperty, ColorScheme.Identifier);
                    }
                    else if (char.IsNumber(character))
                    {
                        var newRange = new TextRange(textBox.Document.ContentEnd, textBox.Document.ContentEnd);
                        newRange.Text = character.ToString();
                        newRange.ApplyPropertyValue(TextElement.ForegroundProperty, ColorScheme.Number);
                    }
                    else
                    {
                        textBox.AppendText(readingWord);
                    }

                    readingWord = "";
                }
            }
            
            textBox.CaretPosition = startPoint.GetPositionAtOffset(caretOffset) ?? textBox.Document.ContentStart;
        }

        public static Tuple<string, List<string>> AutoCompleteSuggest(ref RichTextBox textBox)
        {
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

            return (word, identifiers.Where(id => id.StartsWith(word) && id != word).ToList()).ToTuple();
        }
    }
}