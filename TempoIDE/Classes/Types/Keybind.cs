using System.Linq;
using System.Windows.Input;

namespace TempoIDE.Classes.Types
{
    public struct Keybind
    {
        public readonly Key[] Keys;

        public Keybind(Key[] keys)
        {
            Keys = keys;
        }

        public readonly bool IsPressed()
        {
            return Keys.All(Keyboard.IsKeyDown);
        }

        public override string ToString()
        {
            return string.Join("+", Keys);
        }
    }
}