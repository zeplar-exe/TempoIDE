using System.Windows.Media;
using TempoControls.Core.Types;

namespace TempoControls.SyntaxSchemes
{
    public readonly struct HighlightInfo
    {
        public readonly IntRange Range;
        public readonly FormattedText ProcessedText;

        public HighlightInfo(FormattedText processedText, IntRange range)
        {
            Range = range;
            ProcessedText = processedText;
        }
    }
}