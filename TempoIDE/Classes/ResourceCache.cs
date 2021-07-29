using System.Windows.Media.Imaging;
using System.Xml.Linq;
using TempoIDE.Properties;

namespace TempoIDE.Classes
{
    public static class ResourceCache
    {
        public static XDocument IntellisenseCs => XDocument.Parse(Resources.IntellisenseCs);
        
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