using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace TempoIDE.Classes
{
    public static class Extensions
    {
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            var pos = text.IndexOf(search, StringComparison.Ordinal);
            
            if (pos < 0)
            {
                return text;
            }
            
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        } 
        
        public static TCollectionType LastIndex<TCollectionType>(this IList<TCollectionType> collection)
        {
            if (collection.Count == 0)
            {
                throw new IndexOutOfRangeException($"Collection {collection} is empty.");
            }
            
            return collection[^1];
        }

        public static T ToRealValue<T>(this T? nullable) where T : struct
        {
            return (T) nullable;
        }
        
        public static KeyGesture ToGesture(this string value)
        {
            Key gestureKey = Key.None;
            ModifierKeys gestureModifiers = ModifierKeys.None;

            foreach (var key in value.Split("+"))
            {
                if (Enum.TryParse(key, out Key parsedKey))
                    gestureKey = parsedKey;
                else if (Enum.TryParse(key, out ModifierKeys modifier))
                    gestureModifiers |= modifier;
            }

            return new KeyGesture(gestureKey, gestureModifiers);
        }

        public static string FromGesture(this KeyGesture value)
        {
            var gesture = value;
            var text = $"{gesture.Key.ToString()}";
            
            foreach (var name in gesture.Modifiers.ToString().Split(", "))
            {
                if (Enum.TryParse(name, out ModifierKeys modifier))
                    text += "+" + name;
            }
            
            return text;
        }
    }
}