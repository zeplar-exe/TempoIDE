using System.Windows.Media;
using TempoControls;

namespace TempoIDE.Core.SyntaxSchemes
{
    public class DefaultSyntaxScheme : ISyntaxScheme
    {
        public Brush Default => Brushes.White;

        public void Highlight(ColoredLabel label, FormattedText processedText)
        {
            
        }
    }
}