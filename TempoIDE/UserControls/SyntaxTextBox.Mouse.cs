using System;
using System.Windows;
using System.Windows.Input;
using TempoIDE.Classes;

namespace TempoIDE.UserControls
{
    public partial class SyntaxTextBox
    {
        private double selectStartXPosition;

        private void SyntaxTextBox_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsReadOnly)
                return;

            selectStartXPosition = e.GetPosition(this).X;

            Focus();
            CaretOffset = GetCaretOffsetByClick(e);

            isSelecting = true;
            SelectionRange = new IntRange(CaretIndex, CaretIndex);
        }

        private void SyntaxTextBox_OnMouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            isSelecting = false;
        }

        private void SyntaxTextBox_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isSelecting)
            {
                CaretOffset = GetCaretOffsetByClick(e);
                SelectionRange = new IntRange(SelectionRange.Start, CaretIndex);
            }
        }

        private IntVector GetCaretOffsetByClick(MouseEventArgs mouse)
        {
            var clickPos = mouse.GetPosition(this);
            var lines = GetLines();

            var line = Math.Clamp(
                (int) Math.Floor(clickPos.Y / LineHeight),
                0,
                GetLineCount() - 1
            );

            var column = 0;
            var totalWidth = 0d;

            foreach (var character in lines[line])
            {
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

                totalWidth += character.Size.Width;
                column++;
            }

            return new IntVector(column, line);
        }
    }
}