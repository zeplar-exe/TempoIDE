using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TempoControls.Core.InfoStructs;
using TempoControls.Core.IntTypes;
using TempoIDE.Controls.CodeEditing.BlockElements;
using TempoIDE.Properties;

namespace TempoIDE.Controls.CodeEditing;

public partial class Codespace : UserControl, INotifyPropertyChanged
{
    private StringBuilder TextBuilder { get; }
        
    public string Text => TextBuilder.ToString();

    public DrawInfo DrawInfo { get; private set; } = new(14, new Typeface("Arial"), 96, Brushes.White);
    public TextCaret Caret { get; }
    public IntRange SelectionRange { get; private set; }
    public bool IsReadOnly { get; set; }
        
    private char b_lineBreak = '\n';

    public char LineBreak
    {
        get => b_lineBreak;
        set
        {
            if (b_lineBreak == value)
                return;

            b_lineBreak = value;
            Caret.SetLineBreak(value.ToString());
        }
    }

    private bool b_lineNumbersEnabled = true;
    public bool LineNumbersEnabled
    {
        get => b_lineNumbersEnabled;
        set
        {
            if (b_lineNumbersEnabled == value)
                return;

            b_lineNumbersEnabled = value;
            OnPropertyChanged();
        } 
    }
        
    public bool IsReadonly { get; set; }
    
    public event EventHandler<string>? TextChanged;
    public event EventHandler<FormattedDocument>? ModificationReady;
    public event EventHandler? VisualChanged;

    public event PropertyChangedEventHandler? PropertyChanged;
        
    public Codespace()
    {
        TextBuilder = new StringBuilder();
        Caret = new TextCaret(TextBuilder.ToString());

        Caret.IndexChanged += CaretMoved;
        
        InitializeComponent();
    }

    private void CaretMoved(object? sender, int index)
    {
        if (SelectionRange.Size > 0)
        {
            SelectionRange = new IntRange(index, index);
        }
        
        InvalidateVisualChanged();
    }
    
    public void UpdateDisplay()
    {
        // TODO: Replace default color with SkinHelper.CurrentSkin.TextForegroundColor after merge
        v_Display.Document.UpdateLines(Text.Split(LineBreak).Select(l => new FormattedString(l, DrawInfo)));
        // TODO: When SharpEye is ready, every namespace that has metrics is its own block, ad infinitum until you reach fields/methods
    }

    public void Select(IntRange range)
    {
        SelectionRange = range;

        InvalidateVisualChanged();
    }

    public void Insert(int index, char text)
    {
        TextBuilder.Insert(index, text);
        
        InvalidateTextChanged();
    }

    public void Insert(int index, IEnumerable<char> text)
    {
        TextBuilder.Insert(index, text);
            
        InvalidateTextChanged();
    }

    public void DeleteAt(int index)
    {
        TextBuilder.Remove(index, 0);
        
        InvalidateTextChanged();
    }

    public void DeleteAt(IntRange range)
    {
        range = range.Arrange();
        TextBuilder.Remove(range.Start, range.Size);
        
        InvalidateTextChanged();
    }

    public void ReplaceSelection(IEnumerable<char> text)
    {
        DeleteAt(SelectionRange);
        Insert(SelectionRange.Start, text);
    }

    public void Clear()
    {
        TextBuilder.Clear();
            
        InvalidateTextChanged();
    }

    private void InvalidateTextChanged()
    {
        UpdateDisplay();
        TextChanged?.Invoke(this, Text);
        InvalidateVisualChanged();
    }

    public void InvalidateVisualChanged()
    {
        VisualChanged?.Invoke(this,EventArgs.Empty);
        ModificationReady?.Invoke(this, v_Display.Document);
        v_Display.InvalidateVisual();
    }

    private void Codespace_OnTextInput(object sender, TextCompositionEventArgs e)
    {
        Insert(Caret.Index, e.Text);
    }

    private void Codespace_OnKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:
                Insert(Caret.Index, LineBreak);
                
                e.Handled = true;
                break;
            case Key.Back:
                if (SelectionRange.Size > 0)
                {
                    ReplaceSelection("");
                    Caret.MoveIndex(SelectionRange.Start - 1);
                }
                else if (SelectionRange.Start > 0)
                {
                    Caret.MoveColumn(-1);
                    DeleteAt(SelectionRange);
                }
                
                e.Handled = true;
                break;
            case Key.Delete:
                if (SelectionRange.Size > 0)
                {
                    ReplaceSelection("");
                    Caret.MoveIndex(SelectionRange.Start);
                }
                else if (SelectionRange.End < TextBuilder.Length)
                {
                    DeleteAt(SelectionRange.Start);
                }

                e.Handled = true;
                break;
        }
    }
    
    private void ColoredTextBox_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (IsReadOnly)
            return;

        Caret.MoveIndex(0);
        
        Focus();
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}