using System.Windows.Media;

namespace TempoControls.Core.Types
{
    public readonly struct DrawInfo
    {
        public readonly int FontSize;
        public readonly Typeface Typeface;
        public readonly double Dpi;

        public DrawInfo(int fontSize, Typeface typeface, double dpi)
        {
            FontSize = fontSize;
            Typeface = typeface;
            Dpi = dpi;
        }
    }
}