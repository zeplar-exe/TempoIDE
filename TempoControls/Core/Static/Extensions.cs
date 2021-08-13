using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace TempoControls.Core.Static
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
    }
}