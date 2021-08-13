using System.Windows.Media;
using TempoControls.Controls;

namespace TempoControls.SyntaxSchemes
{
    public class DefaultSyntaxScheme : ISyntaxScheme
    {
        public Brush Default => Brushes.White;

        public void Highlight(ColoredLabel label, HighlightInfo info)
        {
            
        }
    }
}