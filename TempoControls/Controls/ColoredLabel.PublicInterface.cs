using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using TempoControls.CompletionProviders;
using TempoControls.Core.Types;
using TempoControls.Core.Types.Collections;
using TempoControls.SyntaxSchemes;

namespace TempoControls.Controls
{
    public partial class ColoredLabel
    {
        public void AppendText(char character, int index = -1)
        {
            if (index == -1)
            {
                index = Characters.Count;
            }
            
            AppendCharacter(new SyntaxChar(character, GetDefaultDrawInfo()), index);

            TextChanged?.Invoke(this, default);
        }
        
        public void AppendText(IEnumerable<char> text, int index = -1)
        {
            if (index == -1)
            {
                index = Characters.Count;
            }

            if (text.ToString() == Environment.NewLine)
            {
                AppendCharacter(new SyntaxChar('\0', GetDefaultDrawInfo()), index);
            }
            else
            {
                foreach (var character in text)
                    AppendCharacter(new SyntaxChar(character, GetDefaultDrawInfo()), index++);
            }

            InvalidateTextChanged();
        }

        public void AppendText(SyntaxChar character, int index = -1)
        {
            if (index == -1)
            {
                index = Characters.Count;
            }
            
            AppendCharacter(character, index);
            
            InvalidateTextChanged();
        }

        public void AppendText(IEnumerable<SyntaxChar> syntaxChars, int index = -1)
        {
            if (index == -1)
            {
                index = Characters.Count;
            }
            
            foreach (var character in syntaxChars)
                AppendCharacter(character, index++);
            
            InvalidateTextChanged();
        }

        public void InvalidateTextChanged()
        {
            TextChanged?.Invoke(this, default);
        }
        
        public DpiScale GetDpi() => VisualTreeHelper.GetDpi(this);
        public double GetTextDpi() => VisualTreeHelper.GetDpi(this).PixelsPerDip;
        
        public string GetCharactersFromIndex(int index, int places)
        {
            var range = new IntRange(index, index + places).Arrange();
            var characters = new StringBuilder();
            
            foreach (int n in range)
                if (n >= 0 && n < Characters.Count)
                    characters.Append(Characters[n].Value);

            return characters.ToString();
        }

        private CharDrawInfo GetDefaultDrawInfo()
        {
            return new CharDrawInfo(FontSize, new Typeface("Verdana"), GetTextDpi(), Brushes.White);
        }

        private void AppendCharacter(SyntaxChar character, int index)
        {
            if (index >= Characters.Count)
                Characters.Add(character);
            else
                Characters.Insert(index, character);
        }
        
        public void Clear()
        {
            Characters.Clear();

            TextChanged?.Invoke(this, default);
        }
        
        
        public void SetScheme(ISyntaxScheme scheme) => Scheme = scheme;
        public void SetScheme(string schemeExtension)
        {
            Scheme = SyntaxSchemeFactory.FromExtension(schemeExtension);
        }

        public void SetCompletionProvider(ICompletionProvider provider) => CompletionProvider = provider;
        public void SetCompletionProvider(string providerExtension)
        {
            CompletionProvider = CompletionProviderFactory.FromExtension(providerExtension);
        }

        public void UpdateIndex(int index, SyntaxChar newCharacter)
        {
            Characters[index] = newCharacter;
        }
        
        public void UpdateIndex(IntRange indices, SyntaxChar newCharacter)
        {
            foreach (int index in indices)
                UpdateIndex(index, newCharacter);
        }
        
        public void UpdateIndex(int index, Brush color, Typeface typeface)
        {
            Characters[index] = new SyntaxChar(Characters[index].Value,
                new CharDrawInfo(FontSize, typeface, GetTextDpi(), color));
        }
        
        public void UpdateIndex(IntRange indices, Brush color, Typeface typeface)
        {
            foreach (int index in indices)
                UpdateIndex(index, color, typeface);
        }

        public void RemoveIndex(int index)
        {
            Characters.RemoveAt(index);
            
            TextChanged?.Invoke(this, default);
        }
        
        public void RemoveIndex(IntRange indices)
        {
            foreach (int index in indices)
                Characters.RemoveAt(indices.Start);
            
            TextChanged?.Invoke(this, default);
        }

        public SyntaxChar GetCharacterAtIndex(int index)
        {
            return Characters[index];
        }

        public SyntaxCharCollection[] GetLines(bool omitNewLines = false)
        {
            var lines = new List<SyntaxCharCollection> { new() };
            var currentIndex = 0;

            foreach (var character in Characters)
            {
                if (character.Value == NewLine)
                {
                    if (!omitNewLines)
                        lines[currentIndex].Add(character);
                    
                    currentIndex++;

                    lines.Add(new SyntaxCharCollection());
                }
                else
                {
                    lines[currentIndex].Add(character);
                }
            }
            
            return lines.ToArray();
        }
    }
}