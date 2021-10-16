using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TempoIDE.Controls.Editors;
using TempoIDE.Core.Commands;
using TempoIDE.Core.UserActions;

namespace TempoIDE.Controls.Panels
{
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
        
        public static readonly RoutedCommandExt UndoCommand = new();

        public void UndoCommand_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ActionHelper.ProcessActionResult(Editor.Session.Undo());
        }
        
        public static readonly RoutedCommandExt RedoCommand = new();
        
        public void RedoCommand_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ActionHelper.ProcessActionResult(Editor.Session.Redo());
        }
        
        public void TabCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Editor == null;
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
}