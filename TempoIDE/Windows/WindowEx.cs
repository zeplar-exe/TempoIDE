using System.Windows;
using System.Windows.Shell;

namespace TempoIDE.Windows
{
    public class WindowEx : Window
    {
        public WindowEx()
        {
            Style = Application.Current.Resources["WindowStyle"] as Style;
            WindowChrome.SetWindowChrome(this, new WindowChrome { CaptionHeight = 20 });
        }
    }
}