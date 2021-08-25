using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using TempoControls.Core.Static;
using TempoControls.Core.Types;

namespace TempoControls
{
    public partial class ColoredLabel
    {
        public void AppendText(char character, int index = -1)
        {
            if (index == -1)
            {
                index = TextBuilder.Length;
            }
            
            AppendCharacter(character, index);

            InvalidateTextChanged();
        }
        
        public void AppendText(IEnumerable<char> characters, int index = -1)
        {
            if (index == -1)
            {
                index = TextBuilder.Length;
            }
            
            foreach (var character in characters)
                AppendCharacter(character, index++);

            InvalidateTextChanged();
        }

        internal DrawInfo DefaultDrawInfo => new(FontSize, Typeface, TextDpi);

        private void AppendCharacter(char character, int index)
        {
            if (index >= TextBuilder.Length)
                TextBuilder.Append(character);
            else
                TextBuilder.Insert(index, character.ToString());
        }

        public void InvalidateTextChanged()
        {
            TextChanged?.Invoke(this, default);
        }
        
        public double TextDpi => VisualTreeHelper.GetDpi(this).PixelsPerDip;
        
        public string GetCharactersFromIndex(int index, int places)
        {
            var range = new IntRange(index, index + places).Arrange();
            var characters = new StringBuilder();
            
            foreach (int n in range)
                if (n >= 0 && n < TextBuilder.Length)
                    characters.Append(TextBuilder[n]);

            return characters.ToString();
        }

        public void Clear()
        {
            TextBuilder.SetString(string.Empty);
            
            TextChanged?.Invoke(this, default);
        }
        
        
        public void SetScheme(ISyntaxScheme scheme) => Scheme = scheme;

        public void SetCompletionProvider(ICompletionProvider provider) => CompletionProvider = provider;

        public void RemoveIndex(int index)
        {
            TextBuilder.Remove(index, 1);
            
            TextChanged?.Invoke(this, default);
        }
        
        public void RemoveIndex(IntRange indices)
        {
            TextBuilder.Remove(indices.Start, indices.Size);
            
            TextChanged?.Invoke(this, default);
        }

        public string[] GetLines(bool omitNewLines = false)
        {
            var lines = new List<string> { "" };
            var currentIndex = 0;

            foreach (var character in TextBuilder.ToString())
            {
                if (character == NewLine)
                {
                    if (!omitNewLines)
                        lines[currentIndex] += character;
                    
                    currentIndex++;

                    lines.Add(string.Empty);
                }
                else
                {
                    lines[currentIndex] += character;
                }
            }
            
            return lines.ToArray();
        }
    }
}