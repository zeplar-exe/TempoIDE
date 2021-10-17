using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TempoControls.Core.IntTypes;
using TempoControls.Core.Types.Collections;

namespace TempoControls
{
    public partial class ColoredTextBox
    {
        private double selectStartXPosition;

        private void ColoredTextBox_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsReadOnly)
                return;
            
            var oldPosition = CaretOffset;
            
            MoveCaret(GetCaretOffsetByClick(e));

            if (e.ClickCount == 2)
            {
                if (oldPosition == CaretOffset)
                    if (TrySelectWord())
                        return;
            }
            
            selectStartXPosition = e.GetPosition(this).X;

            Focus();
            AutoComplete.Disable();

            isSelecting = true;
            Select(new IntRange(CaretIndex, CaretIndex));
        }

        private bool TrySelectWord()
        {
            var index = CaretIndex;
            var start = index;
            var end = start;

            if (!Text.Any())
                return false;

            if (index == Text.Length)
                return false;

            while (VerifyIndex(++index))
            {
                end++;
                
                if (!char.IsLetter(Text[index]))
                    break;
            }
            
            Select(new IntRange(start, end));
            
            return true;
        }

        private void ColoredTextBox_OnPreviewMouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            isSelecting = false;
        }
        
        private void ColoredTextBox_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;
            
            UpdateSelection(e);
        }
        
        private void ColoredTextBox_OnMouseLeave(object sender, MouseEventArgs e)
        {
            UpdateSelection(e);
        }

        private void ColoredTextBox_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            UpdateSelection(e);
        }

        private void UpdateSelection(MouseEventArgs e)
        {
            if (!isSelecting)
                return;
            
            MoveCaret(GetCaretOffsetByClick(e));
            Select(new IntRange(SelectionRange.Start, CaretIndex));
        }

        private IntVector GetCaretOffsetByClick(MouseEventArgs mouse)
        {
            var clickPos = mouse.GetPosition(this);
            var lines = TextArea.GetLines();

            var line = Math.Clamp(
                (int) Math.Floor(clickPos.Y / TextArea.LineHeight),
                0,
                TextArea.LineCount - 1
            );

            var column = 0;
            var totalWidth = 0d;

            if (clickPos.X > 0)
            {
                var collection = SyntaxCharCollection.FromString(lines[line], TextArea.DefaultDrawInfo);
                
                if (clickPos.X > collection.TotalWidth)
                {
                    column = collection.Count;
                }
                else
                {
                    foreach (var character in collection)
                    {
                        totalWidth += character.Size.Width;
                        column++;

                        if (character.Value == ColoredLabel.LineBreak)
                            column--;

                        if (clickPos.X > selectStartXPosition)
                        {
                            if (totalWidth >= clickPos.X)
                            {
                                break;
                            }
                        }
                        else if (totalWidth > clickPos.X)
                        {
                            var oldWidth = totalWidth - character.Size.Width;

                            if (clickPos.X - oldWidth > character.Size.Width / 2)
                            {
                                break;
                            }
                            else
                            {
                                column--;
                                break;
                            }
                        }
                    }
                }
            }

            if (column < 0)
                column = 0;

            return new IntVector(column, line);
        }
    }
}