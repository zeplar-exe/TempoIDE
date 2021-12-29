namespace TempoIDE.Core.UserActions;

public interface IUserAction
{
    public ActionResult Undo();
    public ActionResult Redo();
}