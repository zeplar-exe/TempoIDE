using System.Windows.Media;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.ColorSchemes
{
    public class DefaultColorScheme : IColorScheme
    {
        public Brush Default => Brushes.White;

        public void Highlight(ref SyntaxTextBox textBox)
        {
            
        }
    }
}