using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TempoIDE.Classes;

namespace TempoIDE.UserControls
{
    public partial class SyntaxTextBox : UserControl
    {
        private void SyntaxTextBox_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Focus();
        }

        private void SyntaxTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
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
            // Backspace is a special case (no pun intended), so It'll be skipped here
            if (e.Text == "\b")
                return;

            AppendText(e.Text);
        }

        private void SyntaxTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                {
                    AppendText('\n');

                    break;
                }
                case Key.Back:
                {
                    Backspace(1);

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
                if (character.Value is '\n')
                {
                    line++;
                    lineWidth = 0d;
                }

                lineWidth += CharacterLeftRightMargin + character.Draw(drawingContext, new CharDrawInfo(
                    new Point(lineWidth, line * LineHeight),
                    FontSize,
                    new Typeface("Verdana"),
                    dpi
                ));
            }

            if (caretVisible)
            {
                drawingContext.DrawRectangle(Brushes.White, null, CaretPosition);
            }
        }
    }
}