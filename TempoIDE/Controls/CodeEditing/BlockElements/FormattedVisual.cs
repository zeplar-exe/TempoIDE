using System.Windows;
using System.Windows.Media;

namespace TempoIDE.Controls.CodeEditing.BlockElements;

public abstract class FormattedVisual
{
    public abstract void Draw(DrawingContext context, Point point);
}