using System.Windows.Media;
using TempoIDE.UserControls;

namespace TempoIDE.Core.SyntaxSchemes
{
    public class DefaultSyntaxScheme : ISyntaxScheme
    {
        public Brush Default => Brushes.White;

        public void Highlight(ColoredLabel label)
        {
            
        }
    }
}