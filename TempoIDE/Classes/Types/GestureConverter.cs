using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace TempoIDE.Classes.Types
{
    [ValueConversion(typeof(string), typeof(KeyGesture))]
    public class GestureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Key gestureKey = Key.None;
            ModifierKeys gestureModifiers = ModifierKeys.None;

            if (value == null)
                return null;
            
            foreach (var key in ((string) value).Split("+"))
            {
                if (Enum.TryParse(key, out Key parsedKey))
                    gestureKey = parsedKey;
                else if (Enum.TryParse(key, out ModifierKeys modifier))
                    gestureModifiers |= modifier;
            }

            return new KeyGesture(gestureKey, gestureModifiers);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var gesture = (KeyGesture) value;
            var text = $"{gesture.Key.ToString()}";
            
            if (value == null)
                return null;
            
            foreach (var name in gesture.Modifiers.ToString().Split(", "))
            {
                if (Enum.TryParse(name, out ModifierKeys modifier))
                    text += "+" + name;
            }
            
            return text;
        }
    }
}