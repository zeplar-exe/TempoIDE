using System.Windows;
using System.Windows.Media;

namespace TempoControls.Core.Types.Collections
{
    public interface IColoredLabelLine
    {
        public void Draw(DrawingContext context, Point origin);
    }

    public class ColoredTextLine : IColoredLabelLine
    {
        private readonly FormattedText text;

        public ColoredTextLine(FormattedText text)
        {
            this.text = text;
        }

        public void Draw(DrawingContext context, Point origin)
        {
            context.DrawText(text, origin);
        }
    }

    public class CsMetricLine : IColoredLabelLine
    {
        public void Draw(DrawingContext context, Point origin)
        {
            
        }
    }
}