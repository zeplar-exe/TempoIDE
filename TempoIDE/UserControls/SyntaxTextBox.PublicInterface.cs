using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TempoIDE.Classes;
using TempoIDE.Classes.ColorSchemes;

namespace TempoIDE.UserControls
{
    public partial class SyntaxTextBox : UserControl
    {
        public string GetSelectedText()
        {
            if (SelectionRange.Size == 0)
                return string.Empty;

            StringBuilder builder = new StringBuilder();

            foreach (int index in SelectionRange)
                builder.Append(TextArea.GetCharacterAtIndex(index).Value);

            return builder.ToString();
        }
        
        public void AppendTextAtCaret(string text)
        {
            foreach (var character in text)
                CaretAppendWrapper(new SyntaxChar(character, GetDefaultDrawInfo()));
        }

        public void AppendTextAtCaret(char character)
        {
            CaretAppendWrapper(new SyntaxChar(character, GetDefaultDrawInfo()));
        }

        public void AppendTextAtCaret(SyntaxChar character)
        {
            CaretAppendWrapper(character);
        }
        
        public void AppendTextAtCaret(IEnumerable<char> characters)
        {
            foreach (var character in characters)
                CaretAppendWrapper(new SyntaxChar(character, GetDefaultDrawInfo()));
        }

        public void AppendTextAtCaret(IEnumerable<SyntaxChar> syntaxChars)
        {
            foreach (var character in syntaxChars)
                CaretAppendWrapper(character);
        }
        
        public double GetDpi() => VisualTreeHelper.GetDpi(this).PixelsPerDip;

        private CharDrawInfo GetDefaultDrawInfo()
        {
            return new CharDrawInfo(FontSize, new Typeface("Verdana"), GetDpi(), Brushes.White);
        }

        private void CaretAppendWrapper(SyntaxChar character)
        {
            if (CaretIndex >= TextArea.Characters.Count)
                TextArea.AppendText(character);
            else
                TextArea.AppendText(character, CaretIndex);

            CaretOffset = character.Value == ColoredLabel.NewLine ?
                new IntVector(0, CaretOffset.Y + 1) : 
                new IntVector(CaretOffset.X + 1, CaretOffset.Y);
        }

        public void ClearSelection()
        {
            SelectionRange = new IntRange(CaretIndex, CaretIndex);
            InvalidateVisual();
        }

        public void Backspace(int count)
        {
            if (TextArea.Characters.Count == 0 || CaretIndex == 0)
                return;

            if (SelectionRange.Size > 0)
            {
                CaretOffset = GetCaretOffsetAtIndex(SelectionRange.Start);
                
                TextArea.RemoveIndex(SelectionRange);
                ClearSelection();
                
                return;
            }

            for (; count > 0; count--)
            {
                SyntaxChar character = TextArea.Characters[CaretIndex - 1];
                
                TextArea.RemoveIndex(CaretIndex - 1);
                
                if (character.Value == ColoredLabel.NewLine)
                {
                    var lines = GetLines();
                    CaretOffset = new IntVector(lines[CaretOffset.Y - 1].Count, CaretOffset.Y - 1);
                }
                else
                {
                    CaretOffset = new IntVector(CaretOffset.X - 1, CaretOffset.Y);
                }
            }
        }

        public void Clear()
        {
            TextArea.Clear();
            CaretOffset = new IntVector(0, 0);
        }

        public string GetTypingWord(bool includeNumbers = false)
        {
            string word = "";
            
            for (var index = CaretIndex - 1; index >= 0; index--)
            {
                var selected = TextArea.Characters[index];

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
    }
}