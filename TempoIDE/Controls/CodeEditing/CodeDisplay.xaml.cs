using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TempoIDE.Controls.CodeEditing.BlockElements;
using TempoIDE.Properties;

namespace TempoIDE.Controls.CodeEditing;

public partial class CodeDisplay : UserControl, INotifyPropertyChanged
{
    public FormattedDocument Document { get; }

    public delegate void TextChangedHandler(CodeDisplay display);
    
    public event PropertyChangedEventHandler? PropertyChanged;

    public CodeDisplay()
    {
        Document = new FormattedDocument();
        
        InitializeComponent();
    }

    protected override Size MeasureOverride(Size constraint)
    {
        // TODO: Include measuring for overlay visuals
        
        var width = 0d;
        var height = 0d;

        foreach (var line in Document.Lines)
        {
            var formattedText = line.CreateFormattedText();
            
            width = Math.Max(formattedText.WidthIncludingTrailingWhitespace, width);
            height += formattedText.Height;
        }

        return new Size(width, height);
    }

    protected override void OnRender(DrawingContext context)
    {
        base.OnRender(context);
        
        Document.Draw(context);
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}