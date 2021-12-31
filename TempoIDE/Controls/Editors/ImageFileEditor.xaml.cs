using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TempoIDE.Core.Commands;
using TempoIDE.Core.UserActions;
using TempoIDE.Core.UserActions.Standard;

namespace TempoIDE.Controls.Editors;

public partial class ImageFileEditor : FileEditor
{
    public bool Saved { get; private set; }
        
    public override bool IsFocused => v_Canvas.IsFocused;
        
    public ImageFileEditor()
    {
        InitializeComponent();
    }
        
    public static readonly RoutedCommandExt UndoCommand = new();

    public void UndoCommand_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        ActionHelper.ProcessActionResult(Session.Undo());
    }
        
    public static readonly RoutedCommandExt RedoCommand = new();
        
    public void RedoCommand_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        ActionHelper.ProcessActionResult(Session.Redo());
    }

    public static ImageFileEditor FromFile(FileInfo file)
    {
        var editor = file.Extension.Replace(".", "") switch
        {
            _ => new ImageFileEditor()
        };
            
        editor.Update(file);

        return editor;
    }
        
    private void Canvas_OnLoaded(object sender, RoutedEventArgs e)
    {
        v_Canvas.EraserShape = new EllipseStylusShape(15, 15);
    }
        
    private void ImageFileEditor_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        return; // TODO: Figure out why erasing does not work
            
        if (v_DrawButton.IsChecked.GetValueOrDefault())
        {
            if (v_Canvas.EditingMode == InkCanvasEditingMode.EraseByPoint)
                v_Canvas.EditingMode = InkCanvasEditingMode.Ink;
            else
                v_Canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }
    }
        
    private void DrawButton_OnIsCheckedChanged(object sender, RoutedEventArgs e)
    {
        if (v_Canvas.ActiveEditingMode == InkCanvasEditingMode.Ink)
            v_Canvas.EditingMode = InkCanvasEditingMode.None;
        else
            v_Canvas.EditingMode = InkCanvasEditingMode.Ink;
    }

    public override void Refresh()
    {
        UpdateVisual();
    }

    public override void Update(FileInfo file)
    {
        BoundFile = file;
            
        UpdateVisual();
    }
        
    public override void FileWriter()
    {
        UpdateFile();
    }

    public override void UpdateVisual()
    {
        if (BoundFile == null)
            return;
            
        v_Image.Source = new BitmapImage(new Uri(BoundFile.FullName));
    }

    public override void UpdateFile()
    {
        if (BoundFile == null)
            return;
        
        if (!Saved)
            return;
            
        var bitmap = new RenderTargetBitmap(
            (int)v_Canvas.Width, (int)v_Canvas.Height, 
            96d, 96d, PixelFormats.Default);
            
        bitmap.Render(v_Canvas);
            
        var encoder = new BmpBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmap));
            
        using var stream = BoundFile.OpenWrite();
        encoder.Save(stream);
    } // Courtesy of https://social.msdn.microsoft.com/Forums/vstudio/en-US/ba4dc89f-0169-43a9-8374-68e1fb34a222/saving-inkcanvas-as-image?forum=wpf

    private void Canvas_OnStrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
    {
        Session.AddAction(new ImageDrawAction(e.Stroke, v_Canvas));
    }
}