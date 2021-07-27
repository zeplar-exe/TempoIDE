using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using TempoIDE.Classes.Types;

namespace TempoIDE.UserControls
{
    public partial class SyntaxTextBox
    {
        public void MoveCaret(IntVector position)
        {
            if (!VerifyCaretOffset(position))
                return;
            
            AutoComplete.Disable();

            CaretOffset = position;
            CaretIndex = GetCaretIndexAtOffset(position);
            
            CaretRect = new Rect(0, 0, CaretRect.Width, CaretRect.Height);

            var line = TextArea.GetLines()[position.Y];
            
            for (var columnNo = 0; columnNo < position.X; columnNo++)
            {
                CaretRect = Rect.Offset(CaretRect, line[columnNo].Size.Width, 0);
            }
            
            CaretRect = Rect.Offset(CaretRect, 0, LineHeight * position.Y);
            
            TextArea.InvalidateVisual();
        }

        public void Select(IntRange range)
        {
            SelectionRange = range;
            
            TextArea.InvalidateVisual();
        }
        
        public void ClearSelection()
        {
            Select(new IntRange(CaretIndex, CaretIndex));
        }

        #region AppendText
        
        public void AppendTextAtCaret(string text)
        {
            foreach (var character in text)
                CaretAppendWrapper(new SyntaxChar(character, GetDefaultDrawInfo()));
        }

        public void AppendTextAtCaret(char character)
        {
            CaretAppendWrapper(new SyntaxChar(character, GetDefaultDrawInfo()));
        }

        public void AppendTextAtCaret(SyntaxChar character)
        {
            CaretAppendWrapper(character);
        }
        
        public void AppendTextAtCaret(IEnumerable<char> characters)
        {
            foreach (var character in characters)
                CaretAppendWrapper(new SyntaxChar(character, GetDefaultDrawInfo()));
        }

        public void AppendTextAtCaret(IEnumerable<SyntaxChar> syntaxChars)
        {
            foreach (var character in syntaxChars)
                CaretAppendWrapper(character);
        }
        
        public double GetDpi() => VisualTreeHelper.GetDpi(this).PixelsPerDip;

        private CharDrawInfo GetDefaultDrawInfo()
        {
            return new CharDrawInfo(FontSize, new Typeface("Verdana"), GetDpi(), Brushes.White);
        }

        private void CaretAppendWrapper(SyntaxChar character)
        {
            if (CaretIndex >= TextArea.Characters.Count)
                TextArea.AppendText(character);
            else
                TextArea.AppendText(character, CaretIndex);

            MoveCaret(character.Value == ColoredLabel.NewLine ?
                new IntVector(0, CaretOffset.Y + 1) : 
                new IntVector(CaretOffset.X + 1, CaretOffset.Y));
        }
        
        #endregion

        public void Backspace(int count)
        {
            if (TextArea.Characters.Count == 0)
                return;

            var range = SelectionRange.Arrange();

            if (range.Size > 0)
            {
                MoveCaret(GetCaretOffsetAtIndex(range.Start));
                
                TextArea.RemoveIndex(range);
                ClearSelection();
                
                UpdateAutoCompletion();
                
                return;
            }
            
            if (CaretIndex == 0)
                return;

            for (; count > 0; count--)
            {
                var character = TextArea.Characters[CaretIndex - 1];
                
                TextArea.RemoveIndex(CaretIndex - 1);
                
                if (character.Value == ColoredLabel.NewLine)
                {
                    var lines = TextArea.GetLines();
                    MoveCaret(new IntVector(lines[CaretOffset.Y - 1].Count, CaretOffset.Y - 1));
                }
                else
                {
                    MoveCaret(new IntVector(CaretOffset.X - 1, CaretOffset.Y));
                }
            }
            
            UpdateAutoCompletion();
        }
        
        public void Frontspace(int count)
        {
            if (SelectionRange.Size > 0)
            {
                MoveCaret(GetCaretOffsetAtIndex(SelectionRange.Start));
                
                TextArea.RemoveIndex(SelectionRange);
                ClearSelection();
                
                UpdateAutoCompletion();
                
                return;
            }
            
            if (CaretIndex == TextArea.Characters.Count)
                return;

            TextArea.RemoveIndex(new IntRange(CaretIndex, CaretIndex + count));
            
            UpdateAutoCompletion();
        }

        public void Clear()
        {
            TextArea.Clear();
            ClearSelection();
            MoveCaret(new IntVector(0, 0));
        }

        public string GetCharactersFromIndex(int index, int places)
        {
            var range = new IntRange(index, index + places).Arrange();
            var characters = new StringBuilder();
            
            foreach (int n in range)
                if (n >= 0 && n < TextArea.Characters.Count)
                    characters.Append(TextArea.Characters[n].Value);

            return characters.ToString();
        }
        
        public string GetSelectedText()
        {
            if (SelectionRange.Size == 0)
                return string.Empty;

            var builder = new StringBuilder();

            foreach (int index in SelectionRange.Arrange())
                builder.Append(TextArea.GetCharacterAtIndex(index).Value);

            return builder.ToString();
        }

        public string GetTypingWord(bool includeNumbers = false)
        {
            var index = CaretIndex - 1;
            var word = "";
            
            for (; index >= 0; index--)
            {
                var selected = TextArea.Characters[index];

                if (char.IsLetter(selected.Value))
                {
                    word = word.Insert(0, selected.ToString());
                }
                else if (includeNumbers && char.IsNumber(selected.Value))
                {
                    word = word.Insert(0, selected.ToString());
                }
                else
                {
                    break;
                }
            }

            return word;
        }
    }
}