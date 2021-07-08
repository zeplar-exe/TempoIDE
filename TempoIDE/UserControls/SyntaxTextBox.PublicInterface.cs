using System;
using System.Collections.Generic;
using System.Linq;
using TempoIDE.Classes;
using TempoIDE.Classes.ColorSchemes;

namespace TempoIDE.UserControls
{
    public partial class SyntaxTextBox
    {
        public void AppendText(string text)
        {
            foreach (var character in text)
            {
                AppendCharacter((SyntaxChar)character);
            }
        }

        public void AppendText(char character)
        {
            AppendCharacter((SyntaxChar)character);
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
                    CaretOffset = new IntVector(lines[CaretOffset.Y - 1].Length, CaretOffset.Y - 1);
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
            CaretOffset = new IntVector();

            TextChanged?.Invoke(this, default);
        }

        public void SetScheme(string schemeExtension)
        {
            scheme = ColorScheme.GetColorSchemeByExtension(schemeExtension);
        }

        public IEnumerator<SyntaxChar> EnumerateCharacters()
        {
            foreach (var character in characters)
                yield return character;
        }
        
        public void Highlight()
        {
            var stb = this;
            scheme?.Highlight(ref stb);
        }
    }
}