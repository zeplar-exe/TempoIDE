using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using TempoControls.Core.Types;
using TempoControls.Core.Types.Collections;

namespace TempoControls
{
    public partial class ColoredTextBox
    {
        public void MoveCaret(IntVector position)
        {
            if (!VerifyOffset(position))
                return;
            
            // TODO: Why in the living fuck is this causing a loss of focus
            
            AutoComplete.Disable();
            
            var newIndex = GetIndexAtOffset(position);
            var caretCharacter = newIndex - 1;

            if (position.X > 0 && VerifyIndex(caretCharacter))
            {
                if (TextArea.Characters[caretCharacter].Value == ColoredLabel.NewLine)
                {
                    var newPos = GetOffsetAtIndex(caretCharacter);
                    
                    if (newPos.Y != position.Y)
                        return;

                    position = new IntVector(position.X - 1, position.Y);
                }
            }

            CaretOffset = position;
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
            TextArea.AppendText(new SyntaxChar(character, TextArea.DefaultDrawInfo), CaretIndex);
            
            MoveCaret(character == ColoredLabel.NewLine ?
                new IntVector(0, CaretOffset.Y + 1) : 
                new IntVector(CaretOffset.X + 1, CaretOffset.Y));
        }

        public void AppendTextAtCaret(IEnumerable<char> characters)
        {
            foreach (var character in characters)
            {
                TextArea.Characters.Insert(CaretIndex, new SyntaxChar(character, TextArea.DefaultDrawInfo));

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
                // TODO: For whatever stupid reason, backspace will randomly delete an irrelevant character
                
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

        public SyntaxCharCollection GetSelectedText()
        {
            var collection = new SyntaxCharCollection();
            
            if (SelectionRange.Size == 0)
                return collection;

            foreach (int index in SelectionRange.Arrange())
                collection.Add(TextArea.GetCharacterAtIndex(index));

            return collection;
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