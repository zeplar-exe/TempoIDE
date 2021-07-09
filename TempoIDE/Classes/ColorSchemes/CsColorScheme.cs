using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Xml;
using TempoIDE.ProgramData;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.ColorSchemes
{
    public class CsColorScheme : IProgrammingLanguageColorScheme
    {
        public Brush Default => Brushes.White;
        public Brush Number => Brushes.LightCoral;
        public Brush Comment => Brushes.ForestGreen;
        public Brush Identifier => Brushes.CornflowerBlue;
        public Brush Type => Brushes.MediumPurple;
        public Brush Method => Brushes.LightGreen;
        public Brush Member => Brushes.CadetBlue;

        public void Highlight(ref SyntaxTextBox textBox)
        {
            int? wordStartIndex = null;
            string word = "";
            
            var xmlData = IColorScheme.GetXDocumentFromString(ProgramFiles.intellisense_cs);
            var keywords = new List<string>();

            if (xmlData.Root is null)
                throw new Exception("CS intellisense xml document is invalid.");

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
                    if (wordStartIndex == null) // always true?
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
            
            if (wordStartIndex == null) // always true?
                return;
            
            if (keywords.Contains(word))
                for (int index = (int) wordStartIndex; index < wordStartIndex + word.Length; index++)
                {
                    textBox.UpdateIndex(index, Identifier, new Typeface("Verdana"));
                }
        }
    }
}