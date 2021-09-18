namespace TempoIDE.Plugins
{
    public class ApplicationExtender : IExtender
    {
        public virtual void ApplicationStartup(string[] args) { }

        public virtual void UpdateShortcut(Shortcut shortcut, string gesture)
        {
            Properties.Shortcuts.Default[shortcut.ToString()] = gesture;
        }
    }

    public enum Shortcut
    {
        Copy, Paste, Cut,
        SelectAll
    }
}