using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TempoIDE.Core
{
    public static class Extensions
    {
        public static IEnumerable<Enum> EnumerateFlags(this Enum input)
        {
            return Enum.GetValues(input.GetType()).Cast<Enum>().Where(input.HasFlag);
        }
        
        public static T FindAncestorOfType<T>(this DependencyObject child) where T : DependencyObject
        {
            var currentParent = child;
            
            do
            {
                currentParent = VisualTreeHelper.GetParent(currentParent);
                
                var parent = currentParent as T;
                
                if (parent != null) 
                    return parent;
            }
            while (currentParent != null);
            
            return null;
        }
        
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {         
            var ms = new MemoryStream();
            
            bitmap.Save(ms, ImageFormat.Bmp);
            
            var image = new BitmapImage();
            
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            
            image.StreamSource = ms;
            
            image.EndInit();

            return image;
        }
        
        public static KeyGesture ToGesture(this string value)
        {
            var gestureKey = Key.None;
            var gestureModifiers = ModifierKeys.None;

            foreach (var key in value.Split("+"))
            {
                if (Enum.TryParse(key, out Key parsedKey))
                    gestureKey = parsedKey;
                else if (Enum.TryParse(key, out ModifierKeys modifier))
                    gestureModifiers |= modifier;
            }

            return new KeyGesture(gestureKey, gestureModifiers);
        }
    }
}