using System.Windows;
using System.Windows.Media;

namespace TempoIDE.Controls.CodeEditing.BlockElements.Common;

public class UnderlineVisual : FormattedVisual
{
    public UnderlineType Type { get; }
    public Brush Brush { get; }
    public int Thickness { get; }

    public UnderlineVisual(UnderlineType type, Brush brush, int thickness)
    {
        Type = type;
        Brush = brush;
        Thickness = thickness;
    }
    
    public override void Draw(DrawingContext context, Point point)
    {
        switch (Type)
        {
             case UnderlineType.Straight:
                 context.DrawLine(new Pen(Brush, Thickness), point, point + new Vector(10, 0));
                 break;
             case UnderlineType.Squiggly:
                 break;
        }
    }
}

public enum UnderlineType
{
    Straight,
    Squiggly
}