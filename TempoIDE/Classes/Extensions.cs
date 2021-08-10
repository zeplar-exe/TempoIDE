using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TempoIDE.Classes
{
    public static class Extensions
    {
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

        public static string ReplaceFirst(this string text, string search, string replace)
        {
            var pos = text.IndexOf(search, StringComparison.Ordinal);
            
            if (pos < 0)
            {
                return text;
            }
            
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static T ToRealValue<T>(this T? nullable) where T : struct
        {
            return (T) nullable;
        }
        
        public static Bitmap ToBitmap(this WriteableBitmap writeBmp)
        {
            Bitmap bmp;
            
            using (var stream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                
                enc.Frames.Add(BitmapFrame.Create(writeBmp));
                enc.Save(stream);
                bmp = new Bitmap(stream);
            }
            
            return bmp;
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

        public static string[] Split(this string str, string separator)
        {
            return str.Split(new[] { separator }, StringSplitOptions.None);
        }
        
        public static byte[] ReadAllBytes(this BinaryReader reader)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }
    
        }
    }

    public static class MathExt
    {
        public static int Clamp(int n, int min, int max)
        {
            if (n > max)
                return max;

            if (n < min)
                return min;

            return n;
        }
        
        public static float Clamp(float n, float min, float max)
        {
            if (n > max)
                return max;

            if (n < min)
                return min;

            return min;
        }
        
        public static double Clamp(double n, double min, double max)
        {
            if (n > max)
                return max;

            if (n < min)
                return min;

            return min;
        }
    }
}