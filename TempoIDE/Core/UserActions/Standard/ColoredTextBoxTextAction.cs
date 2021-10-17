using TempoControls;
using TempoControls.Core.IntTypes;

namespace TempoIDE.Core.UserActions.Standard
{
    public class ColoredTextBoxTextAction : IUserAction
    {
        private readonly IntRange range;
        private readonly string text;
        private readonly ColoredTextBox textBox;

        public ColoredTextBoxTextAction(IntRange range, string text, ColoredTextBox textBox)
        {
            this.range = range;
            this.text = text;
            this.textBox = textBox;
        }

        public ActionResult Undo()
        {
            textBox.TextArea.RemoveIndex(range);

            return new ActionResult(true);
        }

        public ActionResult Redo()
        {
            textBox.TextArea.AppendText(text, range.Start);

            return new ActionResult(true);
        }
    }
}