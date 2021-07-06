using System.Linq;
using System.Windows.Media;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.ColorSchemes
{
    public class CsColorScheme : IColorScheme
    {
        public Brush Default => Brushes.White;
        public Brush Number => Brushes.LightCoral;
        public Brush Comment => Brushes.ForestGreen;
        public Brush Identifier => Brushes.CornflowerBlue;
        public Brush Type => Brushes.MediumPurple;
        public Brush Method => Brushes.LightGreen;
        public Brush Member => Brushes.CadetBlue;

        public void Highlight(ref SyntaxTextBox textBox)
        {
            var richText = textBox.GetPlainText();
            var readingWord = "";

            var startPoint = textBox.Document.ContentStart;
            var caretOffset = startPoint.GetOffsetToPosition(textBox.CaretPosition);

            textBox.Document.Blocks.Clear();

            foreach (var character in richText)
                if (char.IsLetter(character) || char.IsNumber(character))
                {
                    readingWord += character;
                }
                else
                {
                    if (CsIntellisense.Keywords.Contains(readingWord))
                    {
                        textBox.AppendColoredText(readingWord, textBox.Scheme.Identifier);
                    }
                    else if (int.TryParse(readingWord, out _) || float.TryParse(readingWord, out _))
                    {
                        textBox.AppendColoredText(readingWord, textBox.Scheme.Number);
                    }
                    else
                    {
                        textBox.AppendColoredText(readingWord, textBox.Scheme.Default);
                    }

                    if (character == ' ' || character == '\t')
                    {
                        textBox.AppendText(character.ToString());
                        caretOffset += 2; // Don't know why this works, but it does (properly aligns caret)
                    }

                    readingWord = "";
                }

            textBox.CaretPosition = startPoint.GetPositionAtOffset(caretOffset) ?? textBox.Document.ContentStart;
        }
    }
}