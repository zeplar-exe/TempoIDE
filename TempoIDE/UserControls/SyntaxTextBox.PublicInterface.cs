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
                characters.Add((SyntaxChar)character);

                CaretOffset = character is NewLine ?
                    new IntVector(0, CaretOffset.Y + 1) : 
                    new IntVector(CaretOffset.X + 1, CaretOffset.Y);
            }

            TextChanged?.Invoke(this, default);
        }

        public void AppendText(char character)
        {
            AppendText(character.ToString());
        }

        public void AppendText(SyntaxChar character)
        {
            characters.Add(character);

            CaretOffset = character.Value is NewLine ? 
                new IntVector(0, CaretOffset.Y + 1) : 
                new IntVector(CaretOffset.X + 1, CaretOffset.Y);
            
            TextChanged?.Invoke(this, default);
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

        public void Backspace(int count)
        {
            if (characters.Count == 0 || CaretIndex == 0)
                return;

            for (; count > 0; count--)
            {
                SyntaxChar character = characters[CaretIndex - 1];
                
                characters.RemoveAt(CaretIndex - 1);

                if (character.ToChar() is NewLine)
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

            //CaretIndex = 0;
            CaretOffset = new IntVector();

            TextChanged?.Invoke(this, default);
        }

        public void SetScheme(string schemeExtension)
        {
            scheme = ColorScheme.GetColorSchemeByExtension(schemeExtension);
        }

        public int GetLineCount()
        {
            return characters.Where(c => c.Value == NewLine).ToArray().Length;
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