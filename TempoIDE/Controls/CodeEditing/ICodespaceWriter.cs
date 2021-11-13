using System.IO;

namespace TempoIDE.Controls.CodeEditing
{
    public interface ICodespaceWriter
    {
        public void Import(CodeDisplay codeDisplay, StreamReader reader);
        public void Export(CodeDisplay codeDisplay, StreamWriter writer);

        public void AddLine(CodeDisplay codeDisplay, IFormattedVisual visual);
        public void RemoveLine(CodeDisplay codeDisplay, IFormattedVisual visual);
    }
}