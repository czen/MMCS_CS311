using System;
using NUnit.Framework;
using System.IO;
using SimpleLexer;
using System.Collections.Generic;
using System.Linq;


namespace TestSimpleLexer
{
    
    public class LexemList: List<KeyValuePair<Tok, string>>
    {
        public void Add(Tok key, string value)
        {
            var element = new KeyValuePair<Tok, string>(key, value);
            this.Add(element);
        }
    }
    
    [TestFixture]
    public class TestSimpleLexer
    {
        public static List< KeyValuePair<Tok, string> > getLexerOutput(Lexer l)
        {
            List< KeyValuePair<Tok, string> > result = new List< KeyValuePair<Tok, string> >();
            do
            {
                result.Add(new KeyValuePair<Tok, string>(l.LexKind, l.LexText));
                l.NextLexem();
            } while (l.LexKind != Tok.EOF);

            return result;
        }
        
        [Test]
        public void TestId()
        {
            string text = @"id123";
            TextReader inputReader = new StringReader(text);
            Lexer l = new Lexer(inputReader);
            try
            {
                var lexems = getLexerOutput(l);
                Assert.IsTrue(lexems.Count == 1);
                Assert.IsTrue(lexems.First().Key == Tok.ID);
                Assert.IsTrue(lexems.First().Value == "id123");
            }
            catch (LexerException e)
            {
                Assert.Fail();
            }
        }
        
    }
    
    [TestFixture]
    public class TestSimpleLexerOps
    {
        [Test]
        public void TestOps()
        {
            string text = @",:+-* / -+ /**/";
            TextReader inputReader = new StringReader(text);
            Lexer l = new Lexer(inputReader);
            
            var lexems = TestSimpleLexer.getLexerOutput(l);
            Assert.IsTrue(lexems.Count == 12);
            CollectionAssert.AreEqual(new LexemList()
            {
                {Tok.COMMA, ","},
                {Tok.COLON, ":"},
                {Tok.PLUS, "+"},
                {Tok.MINUS, "-"},
                {Tok.MULT, "*"},
                {Tok.DIVISION, "/"},
                {Tok.MINUS, "-"},
                {Tok.PLUS, "+"},
                {Tok.DIVISION, "/"},
                {Tok.MULT, "*"},
                {Tok.MULT, "*"},
                {Tok.DIVISION, "/"},
   
            }.ToList(), lexems);
        }
        
        [Test]
        public void TestKeywords()
        {
            string text = @"div mod - and or not notmod anddiv modor *";
            TextReader inputReader = new StringReader(text);
            Lexer l = new Lexer(inputReader);
            
            var lexems = TestSimpleLexer.getLexerOutput(l);
            Assert.IsTrue(lexems.Count == 10);
            CollectionAssert.AreEqual(new LexemList()
            {
                {Tok.DIV, "div"},
                {Tok.MOD, "mod"},
                {Tok.MINUS, "-"},
                {Tok.AND, "and"},
                {Tok.OR, "or"},
                {Tok.NOT, "not"},
                {Tok.ID, "notmod"},
                {Tok.ID, "anddiv"},
                {Tok.ID, "modor"},
                {Tok.MULT, "*"}
   
            }.ToList(), lexems);
        }
        
        [Test]
        public void TestOpsFail()
        {
            string text = @"-`-$ # @";
            TextReader inputReader = new StringReader(text);
            Lexer l = new Lexer(inputReader);
            Assert.Throws<LexerException>(() =>
            {
                var lexems = TestSimpleLexer.getLexerOutput(l);
            });
        }
        
    }
    
    [TestFixture]
    public class TestSimpleLexerAssigns
    {
        [Test]
        public void TestAssigns()
        {
            string text = @"+ = -= *= /= +==:=6-=-*+/";
            TextReader inputReader = new StringReader(text);
            Lexer l = new Lexer(inputReader);
            
            var lexems = TestSimpleLexer.getLexerOutput(l);
            Assert.IsTrue(lexems.Count == 14);
            CollectionAssert.AreEqual(new LexemList()
            {
                {Tok.PLUS, "+"},
                {Tok.EQ, "="},
                {Tok.MINUSASSIGN, "-="},
                {Tok.MULTASSIGN, "*="},
                {Tok.DIVASSIGN, "/="},
                {Tok.PLUSASSIGN, "+="},
                {Tok.EQ, "="},
                {Tok.ASSIGN, ":="},
                {Tok.INUM, "6"},
                {Tok.MINUSASSIGN, "-="},
                {Tok.MINUS, "-"},
                {Tok.MULT, "*"},
                {Tok.PLUS, "+"},
                {Tok.DIVISION, "/"}
   
            }.ToList(), lexems);
        }
        
    }
    
    [TestFixture]
    public class TestSimpleLexerComparisons
    {
        [Test]
        public void TestComparisons()
        {
            string text = @"><>>=<=+<> > <=";
            TextReader inputReader = new StringReader(text);
            Lexer l = new Lexer(inputReader);
            
            var lexems = TestSimpleLexer.getLexerOutput(l);
            Assert.IsTrue(lexems.Count == 8);
            CollectionAssert.AreEqual(new LexemList()
            {
                {Tok.GT, ">"},
                {Tok.NEQ, "<>"},
                {Tok.GEQ, ">="},
                {Tok.LEQ, "<="},
                {Tok.PLUS, "+"},
                {Tok.NEQ, "<>"},
                {Tok.GT, ">"},
                {Tok.LEQ, "<="}
   
            }.ToList(), lexems);
        }
        
        [Test]
        public void TestComparisonsAndOps()
        {
            string text = @">+6<>>=<= +<> >+=<= <==";
            TextReader inputReader = new StringReader(text);
            Lexer l = new Lexer(inputReader);
            
            var lexems = TestSimpleLexer.getLexerOutput(l);
            Assert.IsTrue(lexems.Count == 13);
            CollectionAssert.AreEqual(new LexemList()
            {
                {Tok.GT, ">"},
                {Tok.PLUS, "+"},
                {Tok.INUM, "6"},
                {Tok.NEQ, "<>"},
                {Tok.GEQ, ">="},
                {Tok.LEQ, "<="},
                {Tok.PLUS, "+"},
                {Tok.NEQ, "<>"},
                {Tok.GT, ">"},
                {Tok.PLUSASSIGN, "+="},
                {Tok.LEQ, "<="},
                {Tok.LEQ, "<="},
                {Tok.EQ, "="}
   
            }.ToList(), lexems);
        }
        
    }
    
    [TestFixture]
    public class TestSimpleLexerLineCmt
    {
        [Test]
        public void TestComment()
        {
            string text = @" ts:=623 // 23 sa 3";
            TextReader inputReader = new StringReader(text);
            Lexer l = new Lexer(inputReader);
            
            var lexems = TestSimpleLexer.getLexerOutput(l);
            Assert.IsTrue(lexems.Count == 3);
            CollectionAssert.AreEqual(new LexemList()
            {
                {Tok.ID, "ts"},
                {Tok.ASSIGN, ":="},
                {Tok.INUM, "623"},
   
            }.ToList(), lexems);
        }

        [Test] public void TestCommentFileEnd()
        {
            string text = @" ts:=623 //";
            TextReader inputReader = new StringReader(text);
            Lexer l = new Lexer(inputReader);
            
            var lexems = TestSimpleLexer.getLexerOutput(l);
            Assert.IsTrue(lexems.Count == 3);
            CollectionAssert.AreEqual(new LexemList()
            {
                {Tok.ID, "ts"},
                {Tok.ASSIGN, ":="},
                {Tok.INUM, "623"},
   
            }.ToList(), lexems);
        }
        
        [Test] public void TestCommentNextLine()
        {
            string text = @" ts:=623 // cmt
                            id := 22";
            TextReader inputReader = new StringReader(text);
            Lexer l = new Lexer(inputReader);
            
            var lexems = TestSimpleLexer.getLexerOutput(l);
            Assert.AreEqual(6, lexems.Count);
            CollectionAssert.AreEqual(new LexemList()
            {
                {Tok.ID, "ts"},
                {Tok.ASSIGN, ":="},
                {Tok.INUM, "623"},
                {Tok.ID, "id"},
                {Tok.ASSIGN, ":="},
                {Tok.INUM, "22"},
   
            }.ToList(), lexems);
        }
        
    }
    
    [TestFixture]
    public class TestSimpleLexerMultLineCmt
    {
        [Test]
        public void TestMultLineComment()
        {
            string text = @" ts:=623 { 23 sa 3 }";
            TextReader inputReader = new StringReader(text);
            Lexer l = new Lexer(inputReader);
            
            var lexems = TestSimpleLexer.getLexerOutput(l);
            Assert.IsTrue(lexems.Count == 3);
            CollectionAssert.AreEqual(new LexemList()
            {
                {Tok.ID, "ts"},
                {Tok.ASSIGN, ":="},
                {Tok.INUM, "623"},
   
            }.ToList(), lexems);
        }

        [Test] 
        public void TestCommentFileEnd()
        {
            string text = @" ts:=623 { }";
            TextReader inputReader = new StringReader(text);
            Lexer l = new Lexer(inputReader);
            
            var lexems = TestSimpleLexer.getLexerOutput(l);
            Assert.IsTrue(lexems.Count == 3);
            CollectionAssert.AreEqual(new LexemList()
            {
                {Tok.ID, "ts"},
                {Tok.ASSIGN, ":="},
                {Tok.INUM, "623"},
   
            }.ToList(), lexems);
        }
        
        [Test] 
        public void TestCommentNextLine()
        {
            string text = @" ts:=623 { cmt
                           cmt } id := 22";
            TextReader inputReader = new StringReader(text);
            Lexer l = new Lexer(inputReader);
            
            var lexems = TestSimpleLexer.getLexerOutput(l);
            Assert.AreEqual(6, lexems.Count);
            CollectionAssert.AreEqual(new LexemList()
            {
                {Tok.ID, "ts"},
                {Tok.ASSIGN, ":="},
                {Tok.INUM, "623"},
                {Tok.ID, "id"},
                {Tok.ASSIGN, ":="},
                {Tok.INUM, "22"},
   
            }.ToList(), lexems);
        }
        
        [Test] 
        public void TestCommentNotClosed()
        {
            string text = @" ts:=623 { cmt
                           cmt  id := 22";
            TextReader inputReader = new StringReader(text);
            Lexer l = new Lexer(inputReader);

            Assert.Throws<LexerException>(() =>
            {
                var lexems = TestSimpleLexer.getLexerOutput(l);
            });


        }
        
    }
    
}