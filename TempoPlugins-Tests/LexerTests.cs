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
            var lexer = new TelLexer("protocol");
            
            Assert.True(lexer.Lex().First().Id == TelTokenId.ProtocolKeyword);
        }

        [Test]
        public void TestInstruction()
        {
            var lexer = new TelLexer("define a as import");
            
            Assert.True(lexer.Lex().First().Id == TelTokenId.DefineInstruction);
        }

        [Test]
        public void TestString()
        {
            const string literal = "\"text\"";
            var lexer = new TelLexer($"\"This {literal} you talk about, is it real?\"");
            
            Assert.True(lexer.Lex().First().Id == TelTokenId.StringLiteral);
        }

        [Test]
        public void TestNumber()
        {
            var lexer = new TelLexer("12.34").Lex();

            Assert.True(lexer.First().Id == TelTokenId.NumericLiteral);
        }

        [Test]
        public void TestLexerError()
        {
            var lexer = new TelLexer("|");
            lexer.Lex();

            Assert.True(lexer.Errors.Count() == 1);
        }
    }
}