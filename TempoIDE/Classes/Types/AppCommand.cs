namespace TempoIDE.Classes.Types
{
    public readonly struct AppCommand
    {
        public readonly string Name;
        public readonly Keybind Keybind;

        public AppCommand(string name, Keybind keybind)
        {
            Name = name;
            Keybind = keybind;
        }
    }
}