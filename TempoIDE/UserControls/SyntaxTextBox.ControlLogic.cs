using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TempoIDE.Classes.Types;

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
            
            Backspace(0);

            AppendTextAtCaret(e.Text);
            UpdateAutoCompletion();
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

                    if (autoCompletions.Selected == null)
                    {
                        AppendTextAtCaret(ColoredLabel.NewLine);
                    }
                    else
                    {
                        autoCompletions.Selected.Execute(this);
                    }

                    UpdateAutoCompletion();

                    break;
                }
                case Key.Back:
                {
                    e.Handled = true;

                    var preceding = GetCharactersFromIndex(CaretIndex, -TabSize);
                    
                    if (string.IsNullOrWhiteSpace(preceding) && preceding != null)
                        Backspace(preceding.Length);
                    else
                        Backspace(1);

                    break;
                }
                case Key.Delete:
                    e.Handled = true;
                    
                    Frontspace(1);
                    
                    break;
                case Key.Tab:
                {
                    e.Handled = true;

                    if (autoCompletions.Selected == null)
                    {
                        var mod = CaretOffset.X % 4;

                        if (mod == 0)
                            mod = TabSize;
                        else
                            mod = Math.Abs(mod - TabSize);

                        AppendTextAtCaret(string.Concat(Enumerable.Repeat(" ", mod)));
                    }
                    else
                    {
                        autoCompletions.Selected?.Execute(this);
                    }

                    UpdateAutoCompletion();
                    
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
        
        private void AutoComplete_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            autoCompletions.Index = AutoComplete.SelectedIndex == -1 ? 0 : AutoComplete.SelectedIndex;
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