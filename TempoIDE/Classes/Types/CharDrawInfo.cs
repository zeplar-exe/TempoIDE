using System.Windows.Media;

namespace TempoIDE.Classes
{
    public readonly struct CharDrawInfo
    {
        public readonly Brush Foreground;
        public readonly int FontSize;
        public readonly Typeface Typeface;
        public readonly double Dpi;

        public CharDrawInfo(int fontSize, Typeface typeface, double dpi, Brush foreground)
        {
            FontSize = fontSize;
            Typeface = typeface;
            Dpi = dpi;
            Foreground = foreground;
        }
    }
}