using System.Windows.Media;

namespace TempoControls.Core.InfoStructs
{
    public readonly struct DrawInfo
    {
        public int FontSize { get; }
        public Typeface Typeface { get; }
        public double Dpi { get; }
        public double LineHeight { get; }
        public Brush Foreground { get; }

        public DrawInfo(int fontSize, Typeface typeface, double dpi, double lineHeight, Brush foreground)
        {
            FontSize = fontSize;
            Typeface = typeface;
            Dpi = dpi;
            LineHeight = lineHeight;
            Foreground = foreground;
        }
    }
}