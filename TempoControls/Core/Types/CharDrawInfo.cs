using System.Windows.Media;

namespace TempoControls.Core.Types
{
    public readonly struct CharDrawInfo
    {
        public readonly int FontSize;
        public readonly Typeface Typeface;
        public readonly double Dpi;

        public CharDrawInfo(int fontSize, Typeface typeface, double dpi)
        {
            FontSize = fontSize;
            Typeface = typeface;
            Dpi = dpi;
        }
    }
}