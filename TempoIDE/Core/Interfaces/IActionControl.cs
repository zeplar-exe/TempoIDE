using TempoIDE.Core.UserActions;

namespace TempoIDE.Core.Interfaces
{
    public interface IActionControl
    {
        public ActionSession Session { get; }
    }
}