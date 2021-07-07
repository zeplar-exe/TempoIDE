using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using TempoIDE.Classes;

namespace TempoIDE.UserControls
{
    public partial class SyntaxTextBox : UserControl
    {
        private void SyntaxTextBox_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            void FocusAction() => Focus();
            Dispatcher.BeginInvoke((Action)FocusAction, DispatcherPriority.ApplicationIdle);
        }

        private void SyntaxTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (IsReadOnly)
                return;
            
            caretThread = new Thread(CaretBlinkerThread);
            caretThread.Start();
        }

        private void SyntaxTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            caretThread.Interrupt();
            caretVisible = false;
            
            InvalidateVisual();
        }

        private void SyntaxTextBox_OnTextChanged(object sender, RoutedEventArgs e)
        {
            InvalidateVisual();
        }

        private void SyntaxTextBox_OnTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsReadOnly)
                return;
            
            // Backspace is a special case (no pun intended), so It'll be skipped here
            if (e.Text == "\b")
                return;

            AppendText(e.Text);
        }

        private void SyntaxTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (IsReadOnly)
                return;
            
            switch (e.Key)
            {
                case Key.Enter:
                {
                    AppendText(NewLine);

                    break;
                }
                case Key.Back:
                {
                    Backspace(1);

                    break;
                }
                case Key.Left:
                {
                    e.Handled = true;
                    var newPosition = CaretOffset + new IntVector(-1, 0);
                    
                    if (VerifyCaretOffset(newPosition))
                        CaretOffset = newPosition;

                    break;
                }
                case Key.Right:
                {
                    e.Handled = true;
                    var newPosition = CaretOffset + new IntVector(1, 0);

                    if (VerifyCaretOffset(newPosition))
                        CaretOffset = newPosition;

                    break;
                }
                case Key.Up:
                {
                    e.Handled = true;
                    var newPosition = CaretOffset + new IntVector(0, -1);

                    if (VerifyCaretOffset(newPosition))
                        CaretOffset = newPosition;

                    break;
                }
                case Key.Down:
                {
                    e.Handled = true;
                    var newPosition = CaretOffset + new IntVector(0, 1);

                    if (VerifyCaretOffset(newPosition))
                        CaretOffset = newPosition;

                    break;
                }
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var line = 0;
            var dpi = VisualTreeHelper.GetDpi(this).PixelsPerDip;

            var lineWidth = 0d;

            CaretPosition = new Rect(CaretPosition.X, CaretPosition.Y, 5, LineHeight);

            foreach (var character in characters)
            {
                if (character.Value is NewLine)
                {
                    line++;
                    lineWidth = 0d;
                    continue;
                }

                var charSize = character.Draw(drawingContext, new CharDrawInfo(
                    new Point(lineWidth, line * LineHeight),
                    FontSize,
                    new Typeface("Verdana"),
                    dpi
                ));

                lineWidth += charSize.Width;
            }

            if (caretVisible)
            {
                drawingContext.DrawRectangle(Brushes.White, null, CaretPosition);
            }
        }
    }
}