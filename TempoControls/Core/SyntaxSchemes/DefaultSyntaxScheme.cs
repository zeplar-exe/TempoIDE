using System.Windows.Media;
using TempoControls.Core.Types.Collections;

namespace TempoControls.Core.SyntaxSchemes
{
    public class DefaultSyntaxScheme : ISyntaxScheme
    {
        public Brush Default => Brushes.White;

        public void Highlight(ColoredLabel label, SyntaxCharCollection processedText)
        {
            
        }
    }
}