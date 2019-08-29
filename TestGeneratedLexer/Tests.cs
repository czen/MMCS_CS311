using System;
using NUnit.Framework;
using GeneratedLexer;

namespace TestGeneratedLexer
{
    [TestFixture]
    public class GeneratedLexerTests
    {
        [Test]
        public void TestIdCount()
        {
            LexerAddon lexer = new LexerAddon(@"id1 id2 id3
                                                  id4 id5  ");
            lexer.Lex();
            Assert.AreEqual(5, lexer.idCount);
        }
        
        [Test]
        public void TestIdInfo()
        {
            LexerAddon lexer = new LexerAddon(@"i22d1 i id3
                                                  Md4 inNd5  ");
            lexer.Lex();
            Assert.AreEqual(5, lexer.idCount);
            Assert.AreEqual(5, lexer.maxIdLength);
            Assert.AreEqual(1, lexer.minIdLength);
            Assert.AreEqual(3.4, lexer.avgIdLength, 0.001);
        }
        
        [Test]
        public void TestNumbers()
        {
            LexerAddon lexer = new LexerAddon(@"i22d1 5.6 i 32 id3
                                                  Md4 8.9 inNd5 1  42 ");
            lexer.Lex();
            
            Assert.AreEqual(75, lexer.sumInt);
            Assert.AreEqual(14.5, lexer.sumDouble, 0.001);
        }
        
        [Test]
        public void TestString()
        {
            LexerAddon lexer = new LexerAddon(@"3 389 3 'ssfsf ' ");
            lexer.Lex();
            
            // TODO: checks in this test
        }
        
        [Test]
        public void TestSingleLineCmt()
        {
            LexerAddon lexer = new LexerAddon(@"i22d1 5.6  // i 32 id3
                                                  Md4 8.9 inNd5 1  42 ");
            lexer.Lex();
            
            Assert.AreEqual(3, lexer.idCount);
            Assert.AreEqual(43, lexer.sumInt);
            Assert.AreEqual(14.5, lexer.sumDouble, 0.001);
        }
        
        [Test]
        public void TestMultiLineCmt()
        {
            LexerAddon lexer = new LexerAddon(@"i22d1 5.6  { i 32 id3
                                                  Md4 2.3 2 33} 8.9 inNd5 1  42 ");
            lexer.Lex();
            
            Assert.AreEqual(2, lexer.idCount);
            Assert.AreEqual(43, lexer.sumInt);
            Assert.AreEqual(14.5, lexer.sumDouble, 0.001);
        }
        
        [Test]
        public void TestMultiLineCmtIds()
        {
            LexerAddon lexer = new LexerAddon(@"i22d1 5.6  { i 32 id3
                                                  Md4  tgg begin ide2
                                                   end ids 2.3 2 33} 8.9 inNd5 1  42 ");
            lexer.Lex();
            
            Assert.AreEqual(6, lexer.idsInComment.Count);
            Assert.Contains("i", lexer.idsInComment);
            Assert.Contains("id3", lexer.idsInComment);
            Assert.Contains("Md4", lexer.idsInComment);
            Assert.Contains("tgg", lexer.idsInComment);
            Assert.Contains("ide2", lexer.idsInComment);
            Assert.Contains("ids", lexer.idsInComment);
            
        }
    }
}