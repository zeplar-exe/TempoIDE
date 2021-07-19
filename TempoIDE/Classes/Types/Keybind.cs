using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace TempoIDE.Classes.Types
{
    public readonly struct Keybind
    {
        public readonly Key Key;
        public readonly ModifierKeys Modifiers;

        public Keybind(Key key, ModifierKeys modifiers)
        {
            Key = key;
            Modifiers = modifiers;
        }

        public override string ToString()
        {
            var str = Modifiers.ToString().Split(" ")
                .Append(Key.ToString())
                .Where(k => k != "None");

            return string.Join("+", str);
        }
    }
}