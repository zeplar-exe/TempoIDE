using System.Globalization;
using System.Windows;
using System.Windows.Media;
using TempoControls.Core.InfoStructs;

namespace TempoControls.Core.Types
{
    public class SyntaxChar
    {
        public readonly char Value;
        public readonly Size Size;

        public Brush Foreground = Brushes.White;
        public Brush UnderlineColor = Brushes.Transparent;
        public UnderlineType UnderlineType = UnderlineType.Straight;

        public SyntaxChar(char value, DrawInfo info)
        {
            Value = value;

            var formatted = new FormattedText(
                Value.ToString(),
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                info.Typeface,
                info.FontSize,
                Brushes.White,
                info.Dpi
            );
            
            Size = new Size(formatted.WidthIncludingTrailingWhitespace, formatted.Height);
        }

        public static explicit operator char(SyntaxChar syntaxChar) => syntaxChar.Value;

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public enum UnderlineType
    {
        None = 0,
        Straight,
        Squiggly
    }
}