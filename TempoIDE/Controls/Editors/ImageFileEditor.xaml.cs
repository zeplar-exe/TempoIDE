using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace TempoIDE.Controls.Editors
{
    public partial class ImageFileEditor : FileEditor
    {
        public override bool IsFocused { get; }
        
        public ImageFileEditor()
        {
            InitializeComponent();
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
        
        public override void Refresh()
        {
            UpdateVisual();
        }

        public override void Update(FileInfo file)
        {
            
        }
        
        public override void FileWriter()
        {
            UpdateFile();
        }

        public override void UpdateVisual()
        {
            if (BoundFile == null)
                return;
            
            Image.Source = new BitmapImage(new Uri(BoundFile.FullName));
        }

        public override void UpdateFile()
        {
            
        }
    }
}