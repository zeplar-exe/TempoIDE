using System;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using TempoControls.Core.Types;

namespace TempoControls
{
    public partial class ColoredTextBox
    {
        private void ColoredTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (IsReadOnly)
                return;

            caretThread = new Thread(CaretBlinkerThread);
            caretThread.Start();
        }

        private void ColoredTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            caretThread?.Interrupt();
            caretVisible = false;

            AutoComplete.Disable();

            TextArea.InvalidateVisual();
        }

        private void SyntaxTextBox_OnTextChanged(object sender, RoutedEventArgs e)
        {
            overrideCaretVisibility = true;
            
            if (!VerifyOffset(CaretOffset))
                MoveCaret(GetOffsetAtIndex(TextArea.Characters.Count));
            
            TextChanged?.Invoke(sender, e);
        }

        private void ColoredTextBox_OnTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsReadOnly)
                return;
            
            Backspace(0);

            AppendTextAtCaret(e.Text);
            UpdateAutoCompletion();
        }
        
        private void ColoredTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (IsReadOnly)
                return;

            switch (e.Key)
            {
                #region Special
                
                case Key.Enter:
                {
                    e.Handled = true;

                    if (AutoComplete.Enabled)
                        AutoComplete.Selected?.Execute(this);
                    else
                        AppendTextAtCaret(ColoredLabel.NewLine);
                    
                    UpdateAutoCompletion();
                    
                    break;
                }
                case Key.Back:
                {
                    e.Handled = true;

                    var preceding = TextArea.GetCharactersFromIndex(CaretIndex, -TabSize);
                    
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

                    if (!AutoComplete.Enabled)
                    {
                        var mod = CaretOffset.X % 4;

                        mod = mod == 0 ? TabSize : Math.Abs(mod - TabSize);

                        if (!string.IsNullOrWhiteSpace(TextArea.GetCharactersFromIndex(CaretIndex, mod)))
                            mod = TabSize;
                            
                        AppendTextAtCaret(new string(' ', mod));
                    }
                    else
                    {
                        AutoComplete.Selected?.Execute(this);
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

                    MoveCaret(new IntVector(CaretOffset.X - 1, CaretOffset.Y));

                    break;
                }
                case Key.Right:
                {
                    e.Handled = true;
                    overrideCaretVisibility = true;
                    
                    MoveCaret(new IntVector(CaretOffset.X + 1, CaretOffset.Y));
                    
                    break;
                }
                case Key.Up:
                {
                    e.Handled = true;
                    overrideCaretVisibility = true;

                    if (AutoComplete.Enabled)
                        AutoComplete.MoveSelectedIndex(LogicalDirection.Backward);
                    else
                        MoveCaret(new IntVector(CaretOffset.X, CaretOffset.Y - 1));

                    break;
                }
                case Key.Down:
                {
                    e.Handled = true;
                    overrideCaretVisibility = true;

                    if (AutoComplete.Enabled)
                        AutoComplete.MoveSelectedIndex(LogicalDirection.Forward);
                    else
                        MoveCaret(new IntVector(CaretOffset.X, CaretOffset.Y + 1));

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
            }
            else if (caretVisible)
            {
                context.DrawRectangle(Brushes.White, null, CaretRect);
            }
        }
    }
}