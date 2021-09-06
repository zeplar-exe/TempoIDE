using System.Windows.Media;

namespace TempoControls.Core.InfoStructs
{
    public readonly struct DrawInfo
    {
        public readonly int FontSize;
        public readonly Typeface Typeface;
        public readonly double Dpi;
        public readonly double LineHeight;

        public DrawInfo(int fontSize, Typeface typeface, double dpi, double lineHeight)
        {
            FontSize = fontSize;
            Typeface = typeface;
            Dpi = dpi;
            LineHeight = lineHeight;
        }
    }
}