using System.Windows.Controls;
using System.Windows.Ink;

namespace TempoIDE.Core.UserActions.Standard;

public class ImageDrawAction : IUserAction
{
    private Stroke stroke;
    private InkCanvas canvas;

    public ImageDrawAction(Stroke stroke, InkCanvas canvas)
    {
        this.stroke = stroke;
        this.canvas = canvas;
    }

    public ActionResult Undo()
    {
        if (!canvas.Strokes.Remove(stroke))
            return new ActionResult(false,"The given stroke has already been removed.");

        return new ActionResult(true);
    }

    public ActionResult Redo()
    {
        if (canvas.Strokes.Contains(stroke))
            return new ActionResult(false, "The given stroke already exists.");
            
        canvas.Strokes.Add(stroke);

        return new ActionResult(true);
    }
}