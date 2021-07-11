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
            overrideCaretVisibility = true;

            TextChanged?.Invoke(sender, e);
        }

        private void SyntaxTextBox_OnTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsReadOnly)
                return;
            
            if (SelectionRange.Size > 0)
            {
                CaretOffset = GetCaretOffsetAtIndex(SelectionRange.Start);
            
                TextArea.RemoveIndex(SelectionRange);
                ClearSelection();
            }

            AppendTextAtCaret(e.Text);
            HandleAutoCompletion();
        }
        
        private void SyntaxTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (IsReadOnly)
                return;
            
            foreach (var command in Commands)
            {
                if (command.Keybinds.All(Keyboard.IsKeyDown))
                {
                    command.Execute(this);
                    e.Handled = true;
                }
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
                case Key.Delete:
                    Frontspace(1);
                    
                    break;
                case Key.Tab:
                {
                    e.Handled = true;

                    if (selectedAutoComplete == null)
                    {
                        AppendTextAtCaret('\t');
                    }
                    else
                    {
                        AppendTextAtCaret(selectedAutoComplete.Replace(GetTypingWordAtIndex(CaretIndex - 1), ""));
                        selectedAutoComplete = null;
                    }
                    
                    HandleAutoCompletion();

                    break;
                }
                
                #endregion

                #region Arrow Keys
                
                case Key.Left:
                {
                    e.Handled = true;
                    overrideCaretVisibility = true;
                    var newPosition = CaretOffset + new IntVector(-1, 0);
                    
                    if (VerifyCaretOffset(newPosition))
                        CaretOffset = newPosition;

                    break;
                }
                case Key.Right:
                {
                    e.Handled = true;
                    overrideCaretVisibility = true;
                    var newPosition = CaretOffset + new IntVector(1, 0);

                    if (VerifyCaretOffset(newPosition))
                        CaretOffset = newPosition;

                    break;
                }
                case Key.Up:
                {
                    e.Handled = true;
                    overrideCaretVisibility = true;
                    var newPosition = CaretOffset + new IntVector(0, -1);

                    if (VerifyCaretOffset(newPosition))
                        CaretOffset = newPosition;

                    break;
                }
                case Key.Down:
                {
                    e.Handled = true;
                    overrideCaretVisibility = true;
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
            if (overrideCaretVisibility)
            {
                context.DrawRectangle(Brushes.White, null, CaretRect);
                overrideCaretVisibility = false;
                
                return;
            }
            
            if (caretVisible)
            {
                context.DrawRectangle(Brushes.White, null, CaretRect);
            }
        }
    }
}