using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TempoIDE.ProgramData;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.ColorSchemes
{
    public class CsSyntaxScheme : IProgrammingLanguageColorScheme
    {
        public Brush Default => Brushes.White;
        public Brush Number => Brushes.LightCoral;
        public Brush Comment => Brushes.ForestGreen;
        public Brush Identifier => Brushes.CornflowerBlue;
        public Brush Type => Brushes.MediumPurple;
        public Brush Method => Brushes.LightGreen;
        public Brush Member => Brushes.CadetBlue;

        public void Highlight(SyntaxTextBox textBox)
        {
            int? wordStartIndex = null;
            string word = "";

            var xmlData = XmlLoader.Get("intellisense.cs");
            var keywords = new List<string>();

            foreach (var keyword in xmlData.Root.Element("keywords").Elements("kw"))
            {
                keywords.Add(keyword.Value);
            }

            var text = textBox.TextArea.Text;
            var charIndex = 0;
            
            SyntaxTree tree = CSharpSyntaxTree.ParseText(text);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            

            foreach (var character in text)
            {
                textBox.TextArea.UpdateIndex(charIndex, Default, new Typeface("Verdana"));

                if (char.IsNumber(character))
                {
                    textBox.TextArea.UpdateIndex(charIndex, new SyntaxChar(
                        character,
                        new CharDrawInfo(textBox.FontSize, new Typeface("Verdana"), textBox.GetDpi(), Number)
                    ));
                }
                else if (char.IsLetter(character))
                {
                    wordStartIndex ??= charIndex;

                    word += character;
                }
                else
                {
                    if (wordStartIndex == null)
                        continue;
                    
                    if (keywords.Contains(word))
                        textBox.TextArea.UpdateIndex(new IntRange(
                            (int)wordStartIndex, 
                            (int)wordStartIndex + word.Length), 
                            Identifier, new Typeface("Verdana"));

                    word = "";
                    wordStartIndex = null;
                }

                charIndex++;
            }
            
            if (wordStartIndex == null)
                return;
            
            if (keywords.Contains(word))
            {
                for (int index = (int) wordStartIndex; index < wordStartIndex + word.Length; index++)
                {
                    textBox.TextArea.UpdateIndex(index, Identifier, new Typeface("Verdana"));
                }
            }
        }

        public string[] GetAutoCompletions(SyntaxTextBox textBox)
        {
            var xmlData = XmlLoader.Get("intellisense.cs");
            var keywords = new List<string>();

            foreach (var keyword in xmlData.Root.Element("keywords").Elements("kw"))
            {
                keywords.Add(keyword.Value);
            }

            var typingWord = textBox.GetTypingWord();
            
            if (string.IsNullOrWhiteSpace(typingWord))
                return null;

            return keywords.Where(kw => kw.StartsWith(typingWord) && kw != typingWord).ToArray();
        }
    }
}