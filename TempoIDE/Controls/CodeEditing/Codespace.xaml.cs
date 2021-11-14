using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Jammo.ParserTools;
using TempoControls.Core.InfoStructs;
using TempoIDE.Properties;

namespace TempoIDE.Controls.CodeEditing
{
    public partial class Codespace : UserControl, INotifyPropertyChanged
    {
        private readonly StringBuilder textBuilder = new();
        
        public string Text => textBuilder.ToString();

        public DrawInfo DrawInfo { get; private set; } = new(14, new Typeface("Arial"), 96, 15, Brushes.White);
        //public TextCaret Caret { get; } = new();
        
        private bool lineNumbersEnabled = true;
        public bool LineNumbersEnabled
        {
            get => lineNumbersEnabled;
            set
            {
                if (lineNumbersEnabled == value)
                    return;

                lineNumbersEnabled = value;
                OnPropertyChanged();
            } 
        }
        
        public bool IsReadonly { get; set; }

        public delegate void TextChangedHandler(Codespace codespace);
        public event TextChangedHandler TextChanged;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public Codespace()
        {
            InitializeComponent();
        }

        public void Render()
        {
            // TODO: Replace Brushes.White with SkinHelper.CurrentSkin.TextForegroundColor after merge
            Display.UpdateBlocks(new[]
            {
                new FormattedTextBlock(new FormattedString(Text, DrawInfo, Brushes.White), new IndexSpan(0, Text.Length))
            });
            // TODO: When SharpEye is ready, every namespace that has metrics is its own block, ad infinitum until you reach fields/methods
        }

        public void Insert(int index, string text)
        {
            textBuilder.Insert(index, text);
            
            InvalidateTextChanged();
        }

        public void Clear()
        {
            textBuilder.Clear();
            
            InvalidateTextChanged();
        }

        public void InvalidateTextChanged()
        {
            TextChanged?.Invoke(this);
            Render();
        }

        private void Codespace_OnTextInput(object sender, TextCompositionEventArgs e)
        {
            Insert(0, e.Text);
        }

        private void Codespace_OnKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}