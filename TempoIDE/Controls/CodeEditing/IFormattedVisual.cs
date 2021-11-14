using System.Windows;
using System.Windows.Media;

namespace TempoIDE.Controls.CodeEditing
{
    public interface IFormattedVisual
    {
        public void Draw(DrawingContext context, Point point);
    }
}