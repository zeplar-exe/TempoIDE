using System.Windows.Media.Imaging;
using TempoIDE.Properties;

namespace TempoIDE.Core.Static
{
    public static class IconCache
    {
        public static BitmapImage CsIcon => Resources.CsIcon.ToBitmapImage();
        public static BitmapImage PngIcon => Resources.PngIcon.ToBitmapImage();
        public static BitmapImage XmlIcon => Resources.XmlIcon.ToBitmapImage();

        public static BitmapImage ImageFromExtension(string extension)
        {
            return extension.Replace(".", "") switch
            {
                "cs" => CsIcon,
                "png" => PngIcon,
                "xml" => XmlIcon,
                _ => null
            };
        }
    }
}