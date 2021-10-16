using System.Windows.Controls;
using TempoIDE.Core.Interfaces;
using TempoIDE.Core.UserActions;

namespace TempoIDE.Controls.Editors
{
    public abstract class Editor : UserControl, IActionControl
    {
        public new abstract bool IsFocused { get; }
        public ActionSession Session => ActionHelper.GetOrCreateSession(Uid);
    }
}