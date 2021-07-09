using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Xml;
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

            foreach (var charPair in textBox.EnumerateCharacters())
            {
                textBox.UpdateIndex(charPair.index, Default, new Typeface("Verdana"));
                
                var rawChar = charPair.character.Value;
                
                if (char.IsNumber(rawChar))
                {
                    textBox.UpdateIndex(charPair.index, new SyntaxChar(
                        rawChar,
                        new CharDrawInfo(textBox.FontSize, new Typeface("Verdana"), textBox.GetDpi(), Number)
                    ));
                }
                else if (char.IsLetter(rawChar))
                {
                    wordStartIndex ??= charPair.index;

                    word += rawChar;
                }
                else
                {
                    if (wordStartIndex == null)
                        continue;
                    
                    if (keywords.Contains(word))
                        for (int index = (int) wordStartIndex; index < wordStartIndex + word.Length; index++)
                        {
                            textBox.UpdateIndex(index, Identifier, new Typeface("Verdana"));
                        }

                    word = "";
                    wordStartIndex = null;
                }
            }
            
            if (wordStartIndex == null)
                return;
            
            if (keywords.Contains(word))
            {
                for (int index = (int) wordStartIndex; index < wordStartIndex + word.Length; index++)
                {
                    textBox.UpdateIndex(index, Identifier, new Typeface("Verdana"));
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