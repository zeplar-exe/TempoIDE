using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TempoIDE.Properties;

namespace TempoIDE.Controls.CodeEditing
{
    public partial class CodeDisplay : UserControl, INotifyPropertyChanged
    {
        private List<FormattedBlock> blocks = new();

        public event PropertyChangedEventHandler PropertyChanged;

        public CodeDisplay()
        {
            InitializeComponent();
        }

        public void UpdateBlocks(IEnumerable<FormattedBlock> newBlocks)
        {
            blocks = newBlocks as List<FormattedBlock> ?? newBlocks.ToList();
            
            InvalidateMeasure();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (blocks.Count == 0)
                return new Size(0, 0);
            
            return new Size(
                blocks.Max(b => b.CalculateSize().Width),
                blocks.Sum(b => b.CalculateSize().Height));
        }

        protected override void OnRender(DrawingContext context)
        {
            base.OnRender(context);

            var yHeight = 0d;

            foreach (var line in blocks)
            {
                line.Draw(context, new Point(0, yHeight));
                yHeight += line.CalculateSize().Height;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum DisplayUpdateType
    {
        Append,
        Delete,
    }
}