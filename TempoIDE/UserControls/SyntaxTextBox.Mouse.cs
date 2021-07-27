using System;
using System.Windows;
using System.Windows.Input;
using TempoIDE.Classes.Types;

namespace TempoIDE.UserControls
{
    public partial class SyntaxTextBox
    {
        private double selectStartXPosition;

        private void SyntaxTextBox_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsReadOnly)
                return;

            selectStartXPosition = e.GetPosition(this).X;

            Focus();
            MoveCaret(GetCaretOffsetByClick(e));

            isSelecting = true;
            Select(new IntRange(CaretIndex, CaretIndex));
        }

        private void SyntaxTextBox_OnPreviewMouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            isSelecting = false;
        }
        
        private void SyntaxTextBox_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (isSelecting)
            {
                MoveCaret(GetCaretOffsetByClick(e));
                Select(new IntRange(SelectionRange.Start, CaretIndex));
            }
            
            isSelecting = false;
        }

        private void SyntaxTextBox_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (isSelecting)
            {
                MoveCaret(GetCaretOffsetByClick(e));
                Select(new IntRange(SelectionRange.Start, CaretIndex));
            }
        }

        private IntVector GetCaretOffsetByClick(MouseEventArgs mouse)
        {
            var clickPos = mouse.GetPosition(this);
            var lines = TextArea.GetLines();

            var line = Math.Clamp(
                (int) Math.Floor(clickPos.Y / LineHeight),
                0,
                TextArea.GetLineCount() - 1
            );

            var column = 0;
            var totalWidth = 0d;

            if (clickPos.X > 0)
            {
                foreach (var character in lines[line])
                {
                    totalWidth += character.Size.Width;
                    column++;

                    if (character.Value == ColoredLabel.NewLine)
                        column--;
                    
                    if (clickPos.X > selectStartXPosition)
                    {
                        if (totalWidth > clickPos.X)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (totalWidth > clickPos.X)
                        {
                            column--;
                            break;
                        }
                    }
                }
            }
            
            column = Math.Clamp(column, 0, int.MaxValue);
            
            return new IntVector(column, line);
        }
    }
}