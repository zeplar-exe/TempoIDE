using System;
using TempoControls.Core.IntTypes;

namespace TempoIDE.Controls.CodeEditing;

public class TextCaret
{
    private string LineBreak { get; set; } = Environment.NewLine;
    private string TextContext { get; set; }
    private string[] Lines { get; set; }

    private int b_index;

    public int Index
    {
        get => b_index;
        private set
        {
            if (b_index == value)
                return;

            b_index = value;

            IndexChanged?.Invoke(this, value);
        }
    }

    public IntVector Position { get; private set; }

    public event EventHandler<int>? IndexChanged;

    public TextCaret(string context)
    {
        UpdateContext(context);
    }

    public void SetLineBreak(string lineBreak) => LineBreak = lineBreak;
    public void UpdateContext(string context)
    {
        TextContext = context;
        Lines = TextContext.Split(LineBreak);
    }

    public void MoveLine(int lines)
    {
        
    }

    public void MoveToLine(uint line)
    {
            
    }

    public void MoveColumn(int columns)
    {
            
    }

    public void MoveToColumn(uint column)
    {
            
    }

    public void MoveIndex(int index)
    {
        Index = index;
    }
}