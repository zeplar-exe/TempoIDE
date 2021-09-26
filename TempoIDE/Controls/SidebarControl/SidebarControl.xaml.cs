using System.Windows;
using System.Windows.Controls;

namespace TempoIDE.Controls.SidebarControl
{
    public partial class SidebarControl : ItemsControl
    {
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                "Orientation", typeof(Orientation),
                typeof(SidebarControl)
            );
        
        public SidebarControl()
        {
            InitializeComponent();
        }
    }
}