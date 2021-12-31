using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TempoIDE.Controls.Editors;

namespace TempoIDE.Controls.Panels;

public partial class EditorTabItem : UserControl
{
    public Editor Editor;

    public bool IsSelected;

    public event EventHandler Selected;
    public event EventHandler Closed;
        
    public EditorTabItem()
    {
        InitializeComponent();
    }
        
    public void Select()
    {
        IsSelected = true;
        Selected?.Invoke(this, EventArgs.Empty);
    }

    private void EditorTabItem_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        Select();
    }

    private void Close_OnClick(object sender, RoutedEventArgs e)
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }
}