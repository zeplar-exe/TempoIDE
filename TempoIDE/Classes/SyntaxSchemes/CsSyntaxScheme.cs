using System;
using System.Linq;
using System.Windows.Media;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TempoIDE.Classes.Types;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.SyntaxSchemes
{
    public class CsSyntaxScheme : IProgrammingLanguageColorScheme
    {
        public Brush Default => Brushes.White;
        public Brush Number => Brushes.LightCoral;
        public Brush Comment => Brushes.ForestGreen;
        public Brush Identifier => Brushes.CornflowerBlue;
        public Brush Type => Brushes.MediumPurple;
        public Brush Method => Brushes.LightGreen;
        public Brush Member => Brushes.CadetBlue;

        public void Highlight(ColoredLabel textBox)
        {
            int? readStartIndex = null;
            var word = "";

            var xmlData = XmlLoader.Get("intellisense.cs");
            var keywords = xmlData.Root.Element("keywords").Elements("kw");

            var text = textBox.Text;

            for (var index = 0; index < text.Length; index++)
            {
                var character = text[index];

                textBox.UpdateIndex(index, Default, new Typeface("Verdana"));

                if (char.IsNumber(character))
                {
                    if (readStartIndex != null)
                        word += character;
                    else
                        textBox.UpdateIndex(index, Number, new Typeface("Verdana"));
                }
                else if (char.IsLetter(character))
                {
                    if (readStartIndex == null)
                        readStartIndex = index;

                    word += character;
                }
                else
                {
                    if (readStartIndex == null)
                        continue;

                    if (keywords.Any(kw => word == kw.Value))
                        textBox.UpdateIndex(new IntRange(
                                readStartIndex.ToRealValue(),
                                readStartIndex.ToRealValue() + word.Length),
                            Identifier, new Typeface("Verdana"));
                    
                    word = "";
                    readStartIndex = null;
                }
            }

            if (readStartIndex == null)
                return;

            if (keywords.Any(kw => word == kw.Value))
            {
                textBox.UpdateIndex(new IntRange(
                        readStartIndex.ToRealValue(), 
                        readStartIndex.ToRealValue() + word.Length), 
                    Identifier, new Typeface("Verdana"));
            }
        }

        public string[] GetAutoCompletions(SyntaxTextBox textBox)
        {
            var xmlData = XmlLoader.Get("intellisense.cs");
            var keywords = xmlData.Root.Element("keywords").Elements("kw");
            
            var tree = CSharpSyntaxTree.ParseText(textBox.TextArea.Text);
            var root = tree.GetCompilationUnitRoot();

            // TODO: AutoComplete context

            var typingWord = textBox.GetTypingWordAtIndex(textBox.CaretIndex - 1);
                 
            if (string.IsNullOrWhiteSpace(typingWord))
                return null;
            
            return keywords.Select(kw => kw.Value).Where(kw => kw.StartsWith(typingWord) && kw != typingWord).ToArray();
        }
    }
}