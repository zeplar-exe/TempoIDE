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
        public void AppendTextAtCaret(string text)
        {
            foreach (var character in text)
                AppendCharacter(new SyntaxChar(character, GetDefaultDrawInfo()));

            TextChanged?.Invoke(this, default);
        }

        public void AppendTextAtCaret(char character)
        {
            AppendCharacter(new SyntaxChar(character, GetDefaultDrawInfo()));
            
            TextChanged?.Invoke(this, default);
        }

        public void AppendTextAtCaret(SyntaxChar character)
        {
            AppendCharacter(character);
            
            TextChanged?.Invoke(this, default);
        }

        [Obsolete("This overload has a O(n^2) time complexity, it is recommended that you pass a string instead.")]
        public void AppendTextAtCaret(IEnumerable<char> syntaxChars)
        {
            foreach (var character in syntaxChars)
                AppendCharacter(new SyntaxChar(character, GetDefaultDrawInfo()));
            
            TextChanged?.Invoke(this, default);
        }

        public void AppendTextAtCaret(IEnumerable<SyntaxChar> syntaxChars)
        {
            foreach (var character in syntaxChars)
                AppendCharacter(character);
            
            TextChanged?.Invoke(this, default);
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

        public string GetTypingWord(bool includeNumbers = false)
        {
            string word = "";
            
            for (var index = CaretIndex - 1; index >= 0; index--)
            {
                var selected = characters[index];

                if (char.IsLetter(selected.Value))
                {
                    word = word.Insert(0, selected.ToString());
                }
                else if (includeNumbers && char.IsNumber(selected.Value))
                {
                    word = word.Insert(0, selected.ToString());
                }
                else
                {
                    break;
                }
            }

            return word;
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
            scheme?.Highlight(this);
        }
    }
}