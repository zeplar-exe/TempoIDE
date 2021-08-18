using System;
using System.Threading;
using NUnit.Framework;
using TempoControls;
using TempoControls.Core.Types;

namespace TempoControlsTests
{
    [TestFixture]
    public class ColoredTextBoxTests
    {
        [TestFixture, Apartment(ApartmentState.STA)]
        public class TextModificationTests
        {
            [Test]
            public void TestAppend()
            {
                var textBox = new ColoredTextBox();
                var text = "Hello world!";

                textBox.AppendTextAtCaret(text);
                
                Assert.True(textBox.TextArea.Text == text);
            }

            [Test]
            public void TestSingleBackspace()
            {
                var textBox = new ColoredTextBox();
                var text = "Power of the y!";
                
                textBox.AppendTextAtCaret(text);
                textBox.Backspace(1);
                
                Assert.True(textBox.TextArea.Text == text[..^1]);
            }

            [Test]
            public void TestMultiBackspace()
            {
                var textBox = new ColoredTextBox();
                var text = "Power of the y!";
                var backspaceCount = 5;
                
                textBox.AppendTextAtCaret(text);
                textBox.Backspace(backspaceCount);
                
                Assert.True(textBox.TextArea.Text == text[..^backspaceCount]);
            }

            [Test]
            public void TestSelectionBackspace()
            {
                var textBox = new ColoredTextBox();
                var text = "Power of the y!";
                
                textBox.AppendTextAtCaret(text);
                textBox.Select(new IntRange(0, text.Length));
                textBox.Backspace(0);
                
                Assert.True(textBox.TextArea.Text == string.Empty);
            }
        }
    }
}