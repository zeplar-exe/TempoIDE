using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using TempoIDE.Controls.CodeEditing;
using TempoIDE.Core.Helpers;
using Timer = System.Timers.Timer;

namespace TempoIDE.Controls.Editors;

public partial class TextFileEditor : FileEditor
{
    private readonly Timer updateTimer = new(500);
    private bool textChangedBeforeUpdate;

    public override bool IsFocused => v_Codespace.IsFocused;

    public TextFileEditor()
    {
        InitializeComponent();
    }

    public static TextFileEditor FromFile(FileInfo file)
    {
        var editor = file.Extension.Replace(".", "") switch
        {
            "cs" => new CsFileEditor(),
            _ => new TextFileEditor()
        };
            
        editor.Update(file);

        return editor;
    }

    private void TextEditor_OnLoaded(object sender, RoutedEventArgs e)
    {
        updateTimer.Elapsed += delegate { FileWriter(); };
    }
        
    public override void FileWriter()
    {
        if (textChangedBeforeUpdate)
        {
            UpdateFile();
        }
        else
        {
            Dispatcher.Invoke(UpdateVisual);
        }

        textChangedBeforeUpdate = false;
    }

    private void TextEditor_OnTextChanged(object? sender, string _)
    {
        textChangedBeforeUpdate = true;
    }

    public override void Refresh() => UpdateVisual();

    public override void Update(FileInfo? file)
    {
        if (file is not { Exists: true })
            return;

        BoundFile = file;

        UpdateVisual();

        // Codespace.TextArea.SetScheme(
        //     ExtensionAssociator.SchemeFromExtension(BoundFile?.Extension));
        //
        // Codespace.TextArea.SetCompletionProvider(
        //     ExtensionAssociator.CompletionProviderFromExtension(BoundFile?.Extension));
    }

    public override void UpdateVisual()
    {
        if (BoundFile == null) 
            return;
                    
        BoundFile.Refresh();
            
        var file = EnvironmentHelper.Current.Cache.GetFile(BoundFile);

        if (file.Content == v_Codespace.Text)
            return;

        v_Codespace.Clear();

        foreach (var line in Regex.Split(file.Content, @"\r\n?|\n"))
        {
            v_Codespace.Insert(0, line);
            v_Codespace.Insert(0, v_Codespace.LineBreak);
        }
            
        textChangedBeforeUpdate = false;
    }

    public override async void UpdateFile()
    {
        if (BoundFile == null)
            return;

        BoundFile.Refresh();
            
        EnvironmentHelper.Current.DirectoryWatcher.Buffer();
            
        await File.WriteAllLinesAsync(BoundFile.FullName, v_Codespace.Text.Split(v_Codespace.LineBreak),
            ApplicationHelper.GlobalEncoding);
            
        EnvironmentHelper.Current.DirectoryWatcher.Resume();
    }

    private void FileEditor_OnGotFocus(object sender, RoutedEventArgs e)
    {
        v_Codespace.Focus();
    }
}