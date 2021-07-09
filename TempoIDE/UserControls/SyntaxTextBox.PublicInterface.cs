using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using TempoIDE.Classes;
using TempoIDE.Classes.ColorSchemes;

namespace TempoIDE.UserControls
{
    public partial class SyntaxTextBox : UserControl
    {
        public void AppendText(string text)
        {
            foreach (var character in text)
            {
                AppendCharacter(new SyntaxChar(character, GetDefaultDrawInfo()));
            }
        }

        public void AppendText(char character)
        {
            AppendCharacter(new SyntaxChar(character, GetDefaultDrawInfo()));
        }

        public void AppendText(SyntaxChar character)
        {
            AppendCharacter(character);
        }

        [Obsolete("This overload has a O(n^2) time complexity, it is recommended that you pass a string instead.")]
        public void AppendText(IEnumerable<char> syntaxChars)
        {
            foreach (var character in syntaxChars)
                AppendText(character.ToString());
        }

        public void AppendText(IEnumerable<SyntaxChar> syntaxChars)
        {
            foreach (var character in syntaxChars)
                AppendText(character);
        }
        
        public double GetDpi()
        {
            return VisualTreeHelper.GetDpi(this).PixelsPerDip;
        }

        private CharDrawInfo GetDefaultDrawInfo()
        {
            return new CharDrawInfo(FontSize, new Typeface("Verdana"), GetDpi(), Brushes.White);
        }

        private void AppendCharacter(SyntaxChar character)
        {
            if (CaretIndex >= characters.Count)
                characters.Add(character);
            else
                characters.Insert(CaretIndex, character);

            CaretOffset = character.Value == NewLine ?
                new IntVector(0, CaretOffset.Y + 1) : 
                new IntVector(CaretOffset.X + 1, CaretOffset.Y);
            
            TextChanged?.Invoke(this, default);
        }

        public void Backspace(int count)
        {
            if (characters.Count == 0 || CaretIndex == 0)
                return;

            for (; count > 0; count--)
            {
                SyntaxChar character = characters[CaretIndex - 1];
                
                characters.RemoveAt(CaretIndex - 1);
                
                if (character.Value == NewLine)
                {
                    var lines = GetLines();
                    CaretOffset = new IntVector(lines[CaretOffset.Y - 1].Count, CaretOffset.Y - 1);
                }
                else
                {
                    CaretOffset = new IntVector(CaretOffset.X - 1, CaretOffset.Y);
                }
            }

            TextChanged?.Invoke(this, default);
        }

        public void Clear()
        {
            characters.Clear();
            CaretOffset = new IntVector(0, 0);

            TextChanged?.Invoke(this, default);
        }

        public void SetScheme(string schemeExtension)
        {
            scheme = schemeExtension == null ? null : ColorScheme.GetColorSchemeByExtension(schemeExtension);
        }

        public void UpdateIndex(int index, SyntaxChar newCharacter)
        {
            characters[index] = newCharacter;
        }
        
        public void UpdateIndex(int index, Brush color, Typeface typeface)
        {
            characters[index] = new SyntaxChar(characters[index].Value,
                new CharDrawInfo(FontSize, typeface, GetDpi(), color));
        }

        public SyntaxChar GetCharacterAtIndex(int index)
        {
            return characters[index];
        }

        public IEnumerable<(int index, SyntaxChar character)> EnumerateCharacters()
        {
            for (var index = 0; index < characters.Count; index++)
            {
                yield return (index, characters[index]);
            }
        }
        
        public void Highlight()
        {
            var stb = this;
            scheme?.Highlight(ref stb);
        }
    }
}