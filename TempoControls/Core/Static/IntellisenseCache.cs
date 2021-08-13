using System.Windows.Media.Imaging;
using System.Xml.Linq;
using TempoControls.Properties;

namespace TempoControls.Core.Static
{
    public static class IntellisenseCache
    {
        public static XDocument IntellisenseCs => XDocument.Parse(Resources.CsIntellisense);
    }
}