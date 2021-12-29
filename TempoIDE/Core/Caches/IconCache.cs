using System.Windows.Media.Imaging;
using TempoIDE.Properties;

namespace TempoIDE.Core.Caches;

public static class IconCache
{
    public static readonly BitmapImage CsIcon = Resources.CsIcon.ToBitmapImage();
    public static readonly BitmapImage PngIcon = Resources.PngIcon.ToBitmapImage();
    public static readonly BitmapImage XmlIcon = Resources.XmlIcon.ToBitmapImage();

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