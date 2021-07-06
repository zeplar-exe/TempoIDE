using System.Windows;
using System.Windows.Controls;
using TempoIDE.Classes;
using TempoIDE.Classes.ColorSchemes;

namespace TempoIDE.UserControls
{
    public partial class SyntaxTextBox : RichTextBox
    {
        public IColorScheme Scheme;
        
        public SyntaxTextBox()
        {
            InitializeComponent();
        }

        public void Highlight()
        {
            var stb = this;
            Scheme.Highlight(ref stb);
        }
    }
}