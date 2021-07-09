using System;
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
            if (IsReadOnly)
                return;
            
            Focus();
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
            caretThread?.Interrupt();
            caretVisible = false;
            
            InvalidateVisual();
        }

        private void SyntaxTextBox_OnTextChanged(object sender, RoutedEventArgs e)
        {
            Highlight();

            var autoCompletions = scheme?.GetAutoCompletions(this);

            if (autoCompletions != null && autoCompletions.Length != 0)
            {
                AutoComplete.Visibility = Visibility.Visible;

                AutoComplete.Translate.X = CaretRect.Right;
                AutoComplete.Translate.Y = CaretRect.Bottom;
    
                selectedAutoComplete = autoCompletions[0];
    
                AutoComplete.Words.Children.Clear();
    
                foreach (string word in autoCompletions)
                {
                    AutoComplete.Words.Children.Add(new TextBlock { Text = word });
                }
            }
            else
            {
                selectedAutoComplete = null;
                AutoComplete.Visibility = Visibility.Collapsed;
            }

            InvalidateVisual();
        }

        private void SyntaxTextBox_OnTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsReadOnly)
                return;

            AppendTextAtCaret(e.Text);
        }

        private void SyntaxTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (IsReadOnly)
                return;
            
            switch (e.Key)
            {
                #region Special
                
                case Key.Enter:
                {
                    e.Handled = true;
                    AppendTextAtCaret(NewLine);

                    break;
                }
                case Key.Back:
                {
                    e.Handled = true;
                    Backspace(1);

                    break;
                }
                case Key.Tab:
                {
                    e.Handled = true;

                    if (selectedAutoComplete == null)
                    {
                        AppendTextAtCaret('\t');
                    }
                    else
                    {
                        AppendTextAtCaret(selectedAutoComplete.Replace(GetTypingWord(), "") + " ");
                        selectedAutoComplete = null;
                    }

                    break;
                }
                
                #endregion

                #region Arrow Keys
                
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
                
                #endregion

                default:
                {
                    // Handle command cases
                    
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

            foreach (var character in characters)
            {
                if (character.Value == NewLine)
                {
                    line++;
                    lineWidth = 0d;
                    continue;
                }

                var charSize = character.Draw(drawingContext, new Point(lineWidth, line * LineHeight));

                lineWidth += charSize.Width;
            }

            if (caretVisible)
            {
                drawingContext.DrawRectangle(Brushes.White, null, CaretRect);
            }
        }
    }
}