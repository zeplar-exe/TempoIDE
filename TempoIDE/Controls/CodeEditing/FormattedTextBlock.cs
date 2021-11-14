using System.Windows;
using System.Windows.Media;
using Jammo.ParserTools;

namespace TempoIDE.Controls.CodeEditing
{
    public class FormattedTextBlock : FormattedBlock
    {
        public FormattedString Text { get; }
        public IndexSpan Span { get; }
        
        public FormattedTextBlock(FormattedString text, IndexSpan span)
        {
            Text = text;
            Span = span;
        }

        public override void Draw(DrawingContext context, Point point)
        {
            Text.Draw(context, point);
        }

        public override Size CalculateSize()
        {
            return new Size(Text.FormattedText.WidthIncludingTrailingWhitespace, Text.FormattedText.Height);
        }
    }
}