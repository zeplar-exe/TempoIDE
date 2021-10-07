using System;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using TempoControls.Core.IntTypes;
using TempoControls.Core.Types;
using TempoControls.Core.Types.Collections;

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
                MoveCaret(GetOffsetAtIndex(TextArea.TextBuilder.Length));
            
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
                        AppendTextAtCaret(ColoredLabel.LineBreak);
                    
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
                    
                    if (CaretOffset.X == 0)
                    {
                        if (CaretOffset.Y == 0)
                            break;
                        
                        MoveCaret(new IntVector(TextArea.GetLines()[CaretOffset.Y - 1].Length, CaretOffset.Y - 1));
                    }
                    else
                    {
                        MoveCaret(new IntVector(CaretOffset.X - 1, CaretOffset.Y));
                    }

                    break;
                }
                case Key.Right:
                {
                    e.Handled = true;
                    overrideCaretVisibility = true;
                    
                    var lines = TextArea.GetLines();

                    if (CaretOffset.X == lines[CaretOffset.Y].Length)
                    {
                        if (CaretOffset.Y == lines.Length - 1)
                            break;
                        
                        MoveCaret(new IntVector(lines[CaretOffset.Y + 1].Length, CaretOffset.Y + 1));
                    }
                    else
                    {
                        MoveCaret(new IntVector(CaretOffset.X + 1, CaretOffset.Y));
                    }

                    break;
                }
                case Key.Up:
                {
                    e.Handled = true;
                    overrideCaretVisibility = true;

                    if (AutoComplete.Enabled)
                    {
                        AutoComplete.MoveSelectedIndex(LogicalDirection.Backward);
                    }
                    else
                    {
                        if (CaretOffset.Y == 0)
                            break;
                        
                        var newOffset = new IntVector(CaretOffset.X, CaretOffset.Y - 1);

                        if (VerifyOffset(newOffset))
                            MoveCaret(newOffset);
                        else if (newOffset.Y < 0)
                            MoveCaret(new IntVector(TextArea.GetLines()[CaretOffset.Y - 1].Length, CaretOffset.Y - 1));
                    }

                    break;
                }
                case Key.Down:
                {
                    e.Handled = true;
                    overrideCaretVisibility = true;

                    if (AutoComplete.Enabled)
                    {
                        AutoComplete.MoveSelectedIndex(LogicalDirection.Forward);
                    }
                    else
                    {
                        var lines = TextArea.GetLines();
                        
                        if (CaretOffset.Y >= lines.Length - 1)
                            break;
                        
                        var newOffset = new IntVector(CaretOffset.X, CaretOffset.Y + 1);

                        if (VerifyOffset(newOffset))
                            MoveCaret(newOffset);
                        else if (newOffset.Y < TextArea.LineCount)
                            MoveCaret(new IntVector(TextArea.GetLines()[CaretOffset.Y + 1].Length, CaretOffset.Y + 1));
                    }

                    break;
                }
                
                #endregion
            }
            
            HandledKeyPress?.Invoke(this, e);
        }

        private void TextArea_OnBeforeCharacterRead(DrawingContext context, SyntaxChar character, Rect charRect, int index)
        {
            var range = SelectionRange.Arrange();
            
            if (range.Size > 0 && range.Contains(index))
            {
                context.DrawRectangle(Brushes.DarkCyan, null, charRect);
            }
        }
        
        private void TextArea_OnAfterHighlight(SyntaxCharCollection characters)
        {
            var rect = new Rect(0, 0, CaretWidth, TextArea.LineHeight);
            var lineCount = 0;
            var columnIndex = 0;
            var line = new SyntaxCharCollection();

            foreach (var character in characters)
            {
                if (lineCount > CaretOffset.Y)
                    break;

                if (lineCount == CaretOffset.Y ||
                    lineCount == CaretOffset.Y && character.Value == ColoredLabel.LineBreak)
                {
                    columnIndex++;

                    if (columnIndex > CaretOffset.X)
                        break;
                    
                    line.Add(character);
                }
                else if (character.Value == ColoredLabel.LineBreak)
                {
                    lineCount++;
                }
            }
            
            for (var columnNo = 0; columnNo < CaretOffset.X; columnNo++)
            {
                rect = Rect.Offset(rect, line[columnNo].Size.Width, 0);
            }
            
            rect = Rect.Offset(rect, 0, TextArea.LineHeight * CaretOffset.Y);

            CaretRect = rect;
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