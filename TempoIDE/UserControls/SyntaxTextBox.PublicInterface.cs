using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using TempoIDE.Classes.Types;

namespace TempoIDE.UserControls
{
    public partial class SyntaxTextBox
    {
        public void MoveCaret(IntVector position, bool verifyNewline = false)
        {
            if (!VerifyOffset(position))
                return;
            
            AutoComplete.Disable();
            
            var newIndex = GetIndexAtOffset(position);
            var caretCharacter = newIndex - 1;

            if (verifyNewline && VerifyIndex(caretCharacter))
            {
                var nextLine = new IntVector(0, position.Y + 1);

                if (VerifyOffset(nextLine))
                    position = nextLine;
            }

            CaretOffset = position;
            CaretIndex = newIndex;
            CaretRect = GetCaretRectAtPosition(position);

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

        public Rect GetCaretRectAtPosition(IntVector position)
        {
            var rect = new Rect(0, 0, CaretWidth, CaretRect.Height);

            var line = TextArea.GetLines()[position.Y];
            
            for (var columnNo = 0; columnNo < position.X; columnNo++)
            {
                rect = Rect.Offset(rect, line[columnNo].Size.Width, 0);
            }
            
            rect = Rect.Offset(rect, 0, TextArea.LineHeight * position.Y);
            
            return rect;
        }

        #region AppendText

        public void AppendTextAtCaret(char character)
        {
            TextArea.AppendText(new SyntaxChar(character, GetDefaultDrawInfo()), CaretIndex);
            
            MoveCaret(character == ColoredLabel.NewLine ?
                new IntVector(0, CaretOffset.Y + 1) : 
                new IntVector(CaretOffset.X + 1, CaretOffset.Y));
        }

        public void AppendTextAtCaret(IEnumerable<char> characters)
        {
            foreach (var character in characters)
            {
                TextArea.Characters.Insert(CaretIndex, new SyntaxChar(character, GetDefaultDrawInfo()));

                MoveCaret(character == ColoredLabel.NewLine ?
                    new IntVector(0, CaretOffset.Y + 1) : 
                    new IntVector(CaretOffset.X + 1, CaretOffset.Y));
            }
            
            TextArea.InvalidateTextChanged();
        }
        
        public void AppendTextAtCaret(SyntaxChar character)
        {
            TextArea.AppendText(character, CaretIndex);
            
            MoveCaret(character.Value == ColoredLabel.NewLine ?
                new IntVector(0, CaretOffset.Y + 1) : 
                new IntVector(CaretOffset.X + 1, CaretOffset.Y));
        }

        public void AppendTextAtCaret(IEnumerable<SyntaxChar> syntaxChars)
        {
            foreach (var character in syntaxChars)
            {
                TextArea.Characters.Insert(CaretIndex, character);

                MoveCaret(character.Value == ColoredLabel.NewLine ?
                    new IntVector(0, CaretOffset.Y + 1) : 
                    new IntVector(CaretOffset.X + 1, CaretOffset.Y));
            }
            
            TextArea.InvalidateTextChanged();
        }
        
        public double GetDpi() => VisualTreeHelper.GetDpi(this).PixelsPerDip;

        private CharDrawInfo GetDefaultDrawInfo()
        {
            return new CharDrawInfo(TextArea.FontSize, new Typeface("Verdana"), GetDpi(), Brushes.White);
        }

        #endregion

        public void Backspace(int count)
        {
            if (TextArea.Characters.Count == 0)
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
                var character = TextArea.Characters[CaretIndex - 1];
                
                TextArea.RemoveIndex(CaretIndex - 1);
                
                if (character.Value == ColoredLabel.NewLine)
                {
                    var lines = TextArea.GetLines();
                    MoveCaret(new IntVector(lines[CaretOffset.Y - 1].Count, CaretOffset.Y - 1));
                }
                else
                {
                    MoveCaret(new IntVector(CaretOffset.X - 1, CaretOffset.Y));
                }

                lastRemovedCharacter = character.Value;
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
            
            if (CaretIndex == TextArea.Characters.Count)
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
            if (SelectionRange.Size == 0)
                return string.Empty;

            var builder = new StringBuilder();

            foreach (int index in SelectionRange.Arrange())
                builder.Append(TextArea.GetCharacterAtIndex(index).Value);

            return builder.ToString();
        }

        public string GetTypingWord(bool includeNumbers = false)
        {
            var index = CaretIndex - 1;
            var word = "";
            
            for (; index >= 0; index--)
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