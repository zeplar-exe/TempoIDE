using System;
using System.Collections.Generic;
using System.Windows.Media;
using TempoIDE.Classes;
using TempoIDE.Classes.ColorSchemes;

namespace TempoIDE.UserControls
{
    public partial class ColoredLabel
    {
        public void AppendText(string text, int index = -1)
        {
            if (index < 0)
            {
                index = Characters.Count;
            }
            
            foreach (var character in text)
                AppendCharacter(new SyntaxChar(character, GetDefaultDrawInfo()), index++);
            
            TextChanged?.Invoke(this, default);
        }
        
        public void AppendText(IEnumerable<char> text, int index = -1)
        {
            if (index < 0)
            {
                index = Characters.Count;
            }
            
            foreach (var character in text)
                AppendCharacter(new SyntaxChar(character, GetDefaultDrawInfo()), index++);
            
            TextChanged?.Invoke(this, default);
        }
        
        public void AppendText(char character, int index = -1)
        {
            if (index < 0)
            {
                index = Characters.Count;
            }
            
            AppendCharacter(new SyntaxChar(character, GetDefaultDrawInfo()), index);

            TextChanged?.Invoke(this, default);
        }

        public void AppendText(SyntaxChar character, int index = -1)
        {
            if (index < 0)
            {
                index = Characters.Count;
            }
            
            AppendCharacter(character, index);
            
            TextChanged?.Invoke(this, default);
        }

        public void AppendText(IEnumerable<SyntaxChar> syntaxChars, int index = -1)
        {
            if (index < 0)
            {
                index = Characters.Count;
            }
            
            foreach (var character in syntaxChars)
                AppendCharacter(character, index++);
            
            TextChanged?.Invoke(this, default);
        }
        
        public double GetDpi() => VisualTreeHelper.GetDpi(this).PixelsPerDip;

        private CharDrawInfo GetDefaultDrawInfo()
        {
            return new CharDrawInfo(FontSize, new Typeface("Verdana"), GetDpi(), Brushes.White);
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
        
        public void SetScheme(string schemeExtension)
        {
            Scheme = schemeExtension == null ? null : ColorScheme.GetColorSchemeByExtension(schemeExtension);
        }

        public void UpdateIndex(int index, SyntaxChar newCharacter)
        {
            Characters[index] = newCharacter;
        }
        
        public void UpdateIndex(IntRange indices, SyntaxChar newCharacter)
        {
            foreach (int index in indices)
                Characters[index] = newCharacter;
        }
        
        public void UpdateIndex(int index, Brush color, Typeface typeface)
        {
            Characters[index] = new SyntaxChar(Characters[index].Value,
                new CharDrawInfo(FontSize, typeface, GetDpi(), color));
        }
        
        public void UpdateIndex(IntRange indices, Brush color, Typeface typeface)
        {
            foreach (int index in indices)
                Characters[index] = new SyntaxChar(Characters[index].Value,
                    new CharDrawInfo(FontSize, typeface, GetDpi(), color));
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

        public IEnumerable<(int index, SyntaxChar character)> EnumerateCharacters()
        {
            for (var index = 0; index < Characters.Count; index++)
            {
                yield return (index, Characters[index]);
            }
        }
    }
}