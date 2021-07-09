using System.Windows.Media;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.ColorSchemes
{
    public class DefaultSyntaxScheme : ISyntaxScheme
    {
        public Brush Default => Brushes.White;

        public void Highlight(SyntaxTextBox textBox)
        {
            
        }
        
        public string[] GetAutoCompletions(SyntaxTextBox textBox)
        {
            return null;
        }
    }
}