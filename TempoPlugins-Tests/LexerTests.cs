using System.Linq;
using NUnit.Framework;
using TempoPlugins;

namespace TempoPlugins_Tests
{
    public class LexerTests
    {
        [Test]
        public void TestKeyword()
        {
            var lexer = TelLexer.Lex("protocol");
            
            Assert.True(lexer.First().Id == TelTokenId.ProtocolKeyword);
        }

        [Test]
        public void TestInstruction()
        {
            var lexer = TelLexer.Lex("define a as import");
            
            Assert.True(lexer.First().Id == TelTokenId.DefineInstruction);
        }

        [Test]
        public void TestString()
        {
            const string literal = "\"text\"";
            var lexer = TelLexer.Lex($"\"This {literal} you talk about, is it real?\"");

            Assert.True(lexer.First().Id == TelTokenId.StringLiteral);
        }

        [Test]
        public void TestNumber()
        {
            
        }
    }
}