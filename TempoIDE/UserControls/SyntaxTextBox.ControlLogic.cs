using System;
using System.Linq;
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
                        break; // TODO: Narrowed down the issue to here, basically, the mouse doesn't move fast enough to register as a character backwards
                    }
                }
                else
                {
                    if (totalWidth > clickPos.X)
                    {
                        column--;
                        break; // TODO: Narrowed down the issue to here, basically, the mouse doesn't move fast enough to register as a character backwards
                    }
                }

                totalWidth += character.Size.Width;
                column++;
            }

            return new IntVector(column, line);
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
            
            TextArea.InvalidateVisual();
        }

        private void SyntaxTextBox_OnTextChanged(object sender, RoutedEventArgs e)
        {
            TextChanged?.Invoke(sender, e);
            
            var autoCompletions = TextArea.Scheme?.GetAutoCompletions(this);

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
            
            foreach (var command in Commands)
            {
                if (command.Keybinds.All(Keyboard.IsKeyDown))
                    command.Execute(this);
            }
            
            switch (e.Key)
            {
                #region Special
                
                case Key.Enter:
                {
                    e.Handled = true;
                    AppendTextAtCaret(ColoredLabel.NewLine);

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
                        AppendTextAtCaret(selectedAutoComplete.Replace(GetTypingWord(), ""));
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
            }
        }

        private void TextArea_OnBeforeCharacterRender(DrawingContext context, Rect charRect, int index)
        {
            var range = SelectionRange.Arrange();
            
            if (range.Size > 0 && range.Contains(index))
            {
                context.DrawRectangle(Brushes.DarkCyan, null, charRect);
            }
        }

        private void TextArea_OnAfterRender(DrawingContext context)
        {
            if (caretVisible)
            {
                context.DrawRectangle(Brushes.White, null, CaretRect);
            }
        }
    }
}