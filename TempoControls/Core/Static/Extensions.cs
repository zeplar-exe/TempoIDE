using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
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

        public static void SetString(this StringBuilder stringBuilder, string text)
        {
            stringBuilder.Clear();
            stringBuilder.Append(text);
        }
    }
}