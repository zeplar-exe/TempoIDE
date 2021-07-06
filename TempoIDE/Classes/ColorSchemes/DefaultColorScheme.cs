using System.Windows.Media;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.ColorSchemes
{
    public class DefaultColorScheme : IColorScheme
    {
        public Brush Default => Brushes.White;
        public Brush Number => Brushes.White;
        public Brush Comment => Brushes.White;
        public Brush Identifier => Brushes.White;
        public Brush Type => Brushes.White;
        public Brush Method => Brushes.White;
        public Brush Member => Brushes.White;

        public void Highlight(ref SyntaxTextBox textBox)
        {
        }
    }
}