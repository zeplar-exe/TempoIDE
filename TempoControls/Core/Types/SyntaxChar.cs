using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace TempoControls.Core.Types
{
    public readonly struct SyntaxChar
    {
        public readonly char Value;
        public readonly Size Size;

        public SyntaxChar(char value, CharDrawInfo info)
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
}