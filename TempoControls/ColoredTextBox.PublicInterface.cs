using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using TempoControls.Core.Types;

namespace TempoControls
{
    public partial class ColoredTextBox
    {
        public void MoveCaret(IntVector position)
        {
            if (!VerifyOffset(position))
                return;
            
            AutoComplete.Disable();
            
            var newIndex = GetIndexAtOffset(position);
            var caretCharacter = newIndex - 1;

            if (position.X > 0 && VerifyIndex(caretCharacter))
            {
                if (TextArea.TextBuilder[caretCharacter] == ColoredLabel.NewLine)
                {
                    var newPos = GetOffsetAtIndex(caretCharacter);
                    
                    if (newPos.Y != position.Y)
                        return;

                    position = new IntVector(position.X - 1, position.Y);
                }
            }

            CaretOffset = position;

            TextArea.InvalidateVisual();
        }

        public void Select(IntRange range)
        {
            SelectionRange = range;
            
            TextArea.InvalidateVisual();
        }
        
        public void ClearSelection()
        {
            Select(new IntRange(CaretIndex, CaretIndex));
        }

        #region AppendText

        public void AppendTextAtCaret(char character)
        {
            TextArea.AppendText(character, CaretIndex);
            
            MoveCaret(character == ColoredLabel.NewLine ?
                new IntVector(0, CaretOffset.Y + 1) : 
                new IntVector(CaretOffset.X + 1, CaretOffset.Y));
        }

        public void AppendTextAtCaret(IEnumerable<char> characters)
        {
            foreach (var character in characters)
            {
                TextArea.TextBuilder.Insert(CaretIndex, character.ToString());
                
                MoveCaret(character == ColoredLabel.NewLine ?
                    new IntVector(0, CaretOffset.Y + 1) : 
                    new IntVector(CaretOffset.X + 1, CaretOffset.Y));
            }

            TextArea.InvalidateTextChanged();
        }

        public void AppendTextAtCaret(IEnumerable<SyntaxChar> syntaxChars)
        {
            foreach (var character in syntaxChars)
            {
                TextArea.TextBuilder.Insert(CaretIndex, character);

                MoveCaret(character.Value == ColoredLabel.NewLine ?
                    new IntVector(0, CaretOffset.Y + 1) : 
                    new IntVector(CaretOffset.X + 1, CaretOffset.Y));
            }
            
            TextArea.InvalidateTextChanged();
        }
        
        public double GetDpi() => VisualTreeHelper.GetDpi(this).PixelsPerDip;

        #endregion

        public void Backspace(int count)
        {
            if (TextArea.TextBuilder.Length == 0)
                return;

            var range = SelectionRange.Arrange();

            if (range.Size > 0)
            {
                MoveCaret(GetOffsetAtIndex(range.Start));
                
                TextArea.RemoveIndex(range);
                ClearSelection();
                
                UpdateAutoCompletion();
                
                return;
            }
            
            if (CaretIndex == 0)
                return;

            var lastRemovedCharacter = new char();

            for (; count > 0; count--)
            {
                var character = TextArea.TextBuilder[CaretIndex - 1];
                
                TextArea.RemoveIndex(CaretIndex - 1);
                
                if (character == ColoredLabel.NewLine)
                {
                    var lines = TextArea.GetLines();
                    MoveCaret(new IntVector(lines[CaretOffset.Y - 1].Length, CaretOffset.Y - 1));
                }
                else
                {
                    MoveCaret(new IntVector(CaretOffset.X - 1, CaretOffset.Y));
                }

                lastRemovedCharacter = character;
            }
            
            if (string.IsNullOrWhiteSpace(lastRemovedCharacter.ToString()))
                UpdateAutoCompletion();
        }
        
        public void Frontspace(int count)
        {
            if (SelectionRange.Size > 0)
            {
                MoveCaret(GetOffsetAtIndex(SelectionRange.Start));
                
                TextArea.RemoveIndex(SelectionRange);
                ClearSelection();
                
                UpdateAutoCompletion();
                
                return;
            }
            
            if (CaretIndex == TextArea.TextBuilder.Length)
                return;

            TextArea.RemoveIndex(new IntRange(CaretIndex, CaretIndex + count));
            
            UpdateAutoCompletion();
        }

        public void Clear()
        {
            TextArea.Clear();
            ClearSelection();
            MoveCaret(new IntVector(0, 0));
        }

        public string GetSelectedText()
        {
            var text = new StringBuilder();
            
            if (SelectionRange.Size == 0)
                return string.Empty;

            foreach (int index in SelectionRange.Arrange())
                text.Append(TextArea.TextBuilder[index]);

            return text.ToString();
        }

        public string GetTypingWord(bool includeNumbers = false)
        {
            var index = CaretIndex - 1;
            var word = "";
            
            for (; index >= 0; index--)
            {
                var selected = TextArea.TextBuilder[index];

                if (char.IsLetter(selected))
                {
                    word = word.Insert(0, selected.ToString());
                }
                else if (includeNumbers && char.IsNumber(selected))
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