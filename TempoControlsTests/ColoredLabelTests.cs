using System.Threading;
using NUnit.Framework;
using TempoControls.Controls;

namespace TempoControlsTests
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class ColoredLabelTests
    {
        [TestFixture, Apartment(ApartmentState.STA)]
        public class TextModificationTests
        {
            [Test]
            public void TestAppendString()
            {
                var label = new ColoredLabel();
                var text = "Hello world!";
                
                label.AppendText(text);
                
                Assert.True(label.Text == text);
            }

            [Test]
            public void TestAppendCharacter()
            {
                var label = new ColoredLabel();
                var character = 'a';
                
                label.AppendText(character);
                
                Assert.True(label.Text == character.ToString());
            }

            [Test]
            public void TestAppendTextInsertion()
            {
                var label = new ColoredLabel();

                var mainText = "Hello_World!";
                
                var insertionText = "___";
                var insertionIndex = 1;
                
                label.AppendText(mainText);
                label.AppendText(insertionText, insertionIndex);
                
                Assert.True(label.Text == mainText.Insert(insertionIndex, insertionText));
            }

            [Test]
            public void TestClear()
            {
                var label = new ColoredLabel();
                
                label.AppendText("Text");
                label.Clear();
                
                Assert.True(label.Text == string.Empty);
            }
        }
    }
}