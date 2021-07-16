using System;
using System.Linq;
using System.Windows.Input;

namespace TempoIDE.Classes.Types
{
    public readonly struct Keybind
    {
        private readonly Key[] keys;

        public Keybind(Key[] keys)
        {
            this.keys = keys;
        }

        public bool IsPressed()
        {
            return keys.Length > 1 && keys.All(Keyboard.IsKeyDown);
        }

        public override string ToString()
        {
            return string.Join("+", keys);
        }
    }
}