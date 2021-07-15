using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TempoIDE.Classes.Types;
using TempoIDE.UserControls;

namespace TempoIDE.Classes.ColorSchemes
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
            // var keywords = new List<string>();
            //
            // foreach (var keyword in xmlData.Root.Element("keywords").Elements("kw"))
            // {
            //     keywords.Add(keyword.Value);
            // }

            var keywords = xmlData.Root.Element("keywords").Elements("kw");

            var text = textBox.Text;

            foreach (var character in text)
            {
                var charIndex = text.IndexOf(character);
                
                textBox.UpdateIndex(charIndex, Default, new Typeface("Verdana"));
                
                if (char.IsNumber(character))
                {
                    textBox.UpdateIndex(charIndex, Number, new Typeface("Verdana"));
                    
                    if (readStartIndex != null)
                        word += character;
                }
                else if (char.IsLetter(character))
                {
                    readStartIndex ??= charIndex;

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
                    Identifier,
                    new Typeface("Verdana")
                );
            }
        }

        public string[] GetAutoCompletions(SyntaxTextBox textBox)
        {
            var xmlData = XmlLoader.Get("intellisense.cs");
            var keywords = xmlData.Root.Element("keywords").Elements("kw");
            
            // var tree = CSharpSyntaxTree.ParseText(textBox.TextArea.Text);
            // var root = tree.GetCompilationUnitRoot();

            var typingWord = textBox.GetTypingWordAtIndex(textBox.CaretIndex - 1);
            
            if (string.IsNullOrWhiteSpace(typingWord))
                return null;
            
            return keywords.Select(kw => kw.Value).Where(kw => kw.StartsWith(typingWord) && kw != typingWord).ToArray();
        }
    }
}