using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TempoIDE.Controls.CodeEditing
{
    public partial class CodeDisplay : UserControl
    {
        private List<FormattedBlock> blocks = new();

        public bool LineNumbersEnabled { get; set; } = true;

        public CodeDisplay()
        {
            InitializeComponent();
        }

        public void UpdateBlocks(IEnumerable<FormattedBlock> newLines)
        {
            blocks = newLines as List<FormattedBlock> ?? newLines.ToList();
        }

        protected override void OnRender(DrawingContext context)
        {
            base.OnRender(context);

            var yHeight = 0d;

            foreach (var line in blocks)
            {
                yHeight += line.Draw(context, new Point(0, yHeight));
            }
        }
    }

    public enum DisplayUpdateType
    {
        Append,
        Delete,
    }
}