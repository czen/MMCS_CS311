using System;
using NUnit.Framework;
using Lexer;

namespace TestLexer
{
    [TestFixture]
    public class TestIntLexer
    {
        [Test]
        public void TestIntParse()
        {
            Lexer.Lexer l = new IntLexer("154216");
            Assert.IsTrue(l.Parse(), "Не разбирает 154216");

            l = new IntLexer("1");
            Assert.IsTrue(l.Parse(), "Не разбирает 1");

            l = new IntLexer("0");
            Assert.IsTrue(l.Parse(), "Не разбирает 0");

            l = new IntLexer("-0");
            Assert.IsTrue(l.Parse(), "Не разбирает -0");

            l = new IntLexer("+0");
            Assert.IsTrue(l.Parse(), "Не разбирает +0");

            l = new IntLexer("+11");
            Assert.IsTrue(l.Parse(), "Не разбирает +11");

            l = new IntLexer("-12");
            Assert.IsTrue(l.Parse(), "Не разбирает -12");
        }

        [Test]
        public void TestIntFailDot()
        {
            Lexer.Lexer l = new IntLexer("1.54216");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает 1.54216");
        }

        [Test]
        public void TestIntFailSymbol()
        {
            Lexer.Lexer l = new IntLexer("а1");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает a1");
            l = new IntLexer("1a");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает 1а");
        }

        [Test]
        public void TestIntFailEpty()
        {
            Lexer.Lexer l = new IntLexer("");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает пустую строку");
        }

        [Test]
        public void TestIntCollectNumber()
        {
            IntLexer lInt = new IntLexer("12");
            Assert.IsTrue(lInt.Parse());
            Assert.AreEqual(lInt.parseResult, 12, "Не собирает число 12");

            lInt = new IntLexer("-12");
            Assert.IsTrue(lInt.Parse());
            Assert.AreEqual(lInt.parseResult, -12, "Не собирает число -12");

            lInt = new IntLexer("+12");
            Assert.IsTrue(lInt.Parse());
            Assert.AreEqual(lInt.parseResult, 12, "Не собирает число +12");

            lInt = new IntLexer("+0");
            Assert.IsTrue(lInt.Parse());
            Assert.AreEqual(lInt.parseResult, 0, "Не собирает число +0");
        }
    }

    [TestFixture]
    public class TestIdLexer
    {
        [Test]
        public void TestIdParse()
        {
            IdentLexer l = new IdentLexer("abc22");
            Assert.IsTrue(l.Parse(), "Не разбирает аbc22");

            l = new IdentLexer("a");
            Assert.IsTrue(l.Parse(), "Не разбирает а");
        }

        [Test]
        public void TestIdEmpty()
        {
            IdentLexer l = new IdentLexer("");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает пустую строку");
        }

        [Test]
        public void TestIdCaps()
        {
            IdentLexer l = new IdentLexer("AAAAA");
            Assert.IsTrue(l.Parse(), "Не разбирает заглавные буквы");
        }

        [Test]
        public void TestIdNumbers()
        {
            IdentLexer l = new IdentLexer("a12345");
            Assert.IsTrue(l.Parse(), "Не разбирает цифры в конце");
        }

        [Test]
        public void TestIdUnderscore()
        {
            IdentLexer l = new IdentLexer("a12_5");
            Assert.IsTrue(l.Parse(), "Не разбирает _");
        }

        [Test]
        public void TestIdDot()
        {
            IdentLexer l = new IdentLexer("f67.3");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает точку");
        }

        [Test]
        public void TestIdDollar()
        {
            IdentLexer l = new IdentLexer("f67$3");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает $");
        }
    }

    [TestFixture]
    public class TestIntNotZeroLexer
    {
        [Test]
        public void TestIntNotZeroParse()
        {
            IntNoZeroLexer l = new IntNoZeroLexer("1234");
            Assert.IsTrue(l.Parse(), "Не разбирает 1234");
        }

        [Test]
        public void TestIntNotZeroFail()
        {
            IntNoZeroLexer l = new IntNoZeroLexer("01234");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает 0");

            l = new IntNoZeroLexer("+01234");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает +0");

            l = new IntNoZeroLexer("-01234");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает -0");

            l = new IntNoZeroLexer("0");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает 0");
        }

        [Test]
        public void TestIntNotZeroPass()
        {
            IntNoZeroLexer l = new IntNoZeroLexer("12034");
            Assert.IsTrue(l.Parse(), "Не разбирает 12034");

            l = new IntNoZeroLexer("+12034");
            Assert.IsTrue(l.Parse(), "Не разбирает +12034");

            l = new IntNoZeroLexer("12034");
            Assert.IsTrue(l.Parse(), "Не разбирает 12034");

            l = new IntNoZeroLexer("-12034");
            Assert.IsTrue(l.Parse(), "Не разбирает -12034");
        }

    }

    [TestFixture]
    public class TestLetterDigitLexer
    {

        [Test]
        public void TestLetterDigitParse()
        {
            LetterDigitLexer l = new LetterDigitLexer("a2f5j3");
            Assert.IsTrue(l.Parse(), "Не разбирает a2f5j3");

            l = new LetterDigitLexer("a");
            Assert.IsTrue(l.Parse(), "Не разбирает a");

            l = new LetterDigitLexer("a2");
            Assert.IsTrue(l.Parse(), "Не разбирает a2");

            l = new LetterDigitLexer("a2B");
            Assert.IsTrue(l.Parse(), "Не разбирает a2B");

        }

        [Test]
        public void TestLetterDigitFail()
        {
            LetterDigitLexer l = new LetterDigitLexer("a25j3");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает a25j3");

            l = new LetterDigitLexer("a2jb3");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает a2jb3");

            l = new LetterDigitLexer("2a3b");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает 2a3b");

            l = new LetterDigitLexer("a2f 3b");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает a2f 3b");
        }
    }


    [TestFixture]
    public class TestLetterListLexer
    {
        [Test]
        public void TestLetterListParse()
        {
            LetterListLexer l = new LetterListLexer("a,b,c");
            Assert.IsTrue(l.Parse(), "Не пропускает a,b,c");
            Assert.AreEqual(l.ParseResult.Count, 3, "неправильно собран список-результат");
            Assert.Contains('a', l.ParseResult);
            Assert.Contains('b', l.ParseResult);
            Assert.Contains('c', l.ParseResult);

            l = new LetterListLexer("a");
            Assert.IsTrue(l.Parse(), "Не пропускает a");
            Assert.AreEqual(l.ParseResult.Count, 1, "неправильно собран список-результат");
            Assert.Contains('a', l.ParseResult);

            l = new LetterListLexer("a,B");
            Assert.IsTrue(l.Parse(), "Не пропускает a,B");
            Assert.AreEqual(l.ParseResult.Count, 2, "неправильно собран список-результат");
            Assert.Contains('a', l.ParseResult);
            Assert.Contains('B', l.ParseResult);

            l = new LetterListLexer("a;B");
            Assert.IsTrue(l.Parse(), "Не пропускает a;B");

            l = new LetterListLexer("a,B;c");
            Assert.IsTrue(l.Parse(), "Не пропускает a,B;c");

            l = new LetterListLexer("a;b,c;d");
            Assert.IsTrue(l.Parse(), "Не пропускает a;b,c;d");

        }

        [Test]
        public void TestLetterListFail()
        {
            LetterListLexer l = new LetterListLexer("a,b,");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает a,b,");

            l = new LetterListLexer(",b");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает ,b");

            l = new LetterListLexer(";b");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает ;b");

            l = new LetterListLexer("B;");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает B;");

            l = new LetterListLexer("a,,b");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает a,,b");

            l = new LetterListLexer("a;;b");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает a;;b");

            l = new LetterListLexer("a;1");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает цифры");
        }

    }

    [TestFixture]
    public class TestDigitListLexer
    {
        [Test]
        public void TestDigitListParse()
        {
            DigitListLexer l = new DigitListLexer("1 2");
            Assert.IsTrue(l.Parse(), "Не пропускает 1 2");
            Assert.AreEqual(l.ParseResult.Count, 2, "неправильно собран список-результат");
            Assert.Contains(1, l.ParseResult);
            Assert.Contains(2, l.ParseResult);

            l = new DigitListLexer("1");
            Assert.IsTrue(l.Parse(), "Не пропускает 1");
            Assert.AreEqual(l.ParseResult.Count, 1, "неправильно собран список-результат");
            Assert.Contains(1, l.ParseResult);

            l = new DigitListLexer("5    6");
            Assert.IsTrue(l.Parse(), "Не пропускает 5    6");
            Assert.AreEqual(l.ParseResult.Count, 2, "неправильно собран список-результат");
            Assert.Contains(5, l.ParseResult);
            Assert.Contains(6, l.ParseResult);
        }

        [Test]
        public void TestDigitListFail()
        {
            DigitListLexer l = new DigitListLexer("12");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает 12");

            l = new DigitListLexer(" 1");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает пробелы в начале");

            l = new DigitListLexer("1  ");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает пробелы в конце");

            l = new DigitListLexer("1, 2");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает ,");

            l = new DigitListLexer("1;2");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает ;");

            l = new DigitListLexer("1a2");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает 1a2");
        }

    }

    [TestFixture]
    public class TestLetterDigitGroupLexer
    {
        [Test]
        public void TestLetterDigitGroupParse()
        {
            LetterDigitGroupLexer l = new LetterDigitGroupLexer("aa12ab23");
            Assert.IsTrue(l.Parse(), "Не пропускает aa12ab23");
            Assert.AreEqual(l.ParseResult, "aa12ab23", "неправильно собран aa12ab23");

            l = new LetterDigitGroupLexer("a1b2");
            Assert.IsTrue(l.Parse(), "Не пропускает a1b2");
            Assert.AreEqual(l.ParseResult, "a1b2", "неправильно собран a1b2");

            l = new LetterDigitGroupLexer("aa");
            Assert.IsTrue(l.Parse(), "Не пропускает aa");
            Assert.AreEqual(l.ParseResult, "aa", "неправильно собран aa");

            l = new LetterDigitGroupLexer("A11");
            Assert.IsTrue(l.Parse(), "Не пропускает A11");
            Assert.AreEqual(l.ParseResult, "A11", "неправильно собран A11");

            l = new LetterDigitGroupLexer("ab33cd58e3n5ss32");
            Assert.IsTrue(l.Parse(), "Не пропускает ab33cd58e3n5ss32");
            Assert.AreEqual(l.ParseResult, "ab33cd58e3n5ss32", "неправильно собран ab33cd58e3n5ss32");
        }

        [Test]
        public void TestLetterDigitGroupFail()
        {
            LetterDigitGroupLexer l = new LetterDigitGroupLexer("1a");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает 1a");

            l = new LetterDigitGroupLexer("a111b");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает a111b");

            l = new LetterDigitGroupLexer("A_34b");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает A_34b");

            l = new LetterDigitGroupLexer("");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает пустую строку");

            l = new LetterDigitGroupLexer("11");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает 11");

            l = new LetterDigitGroupLexer("_A34");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает _A34");
        }
    }

    [TestFixture]
    public class TestDoubleLexer
    {
        [Test]
        public void TestDoubleParse()
        {

            DoubleLexer l = new DoubleLexer("123.4");
            Assert.IsTrue(l.Parse(), "Не понимает 123.4");
            Assert.AreEqual(l.ParseResult, 123.4, "Неправильно прочитал 123.4");

            l = new DoubleLexer("123");
            Assert.IsTrue(l.Parse(), "Не понимает 123");
            Assert.AreEqual(l.ParseResult, 123, "Неправильно прочитал 123");

            l = new DoubleLexer("0.4");
            Assert.IsTrue(l.Parse(), "Не понимает 0.4");
            Assert.AreEqual(l.ParseResult, 0.4, "Неправильно прочитал 0.4");

            l = new DoubleLexer("0.4");
            Assert.IsTrue(l.Parse(), "Не понимает 0.4");
            Assert.AreEqual(l.ParseResult, 0.4, "Неправильно прочитал 0.4");

        }

        [Test]
        public void TestDoubleFail()
        {
            DoubleLexer l = new DoubleLexer(".4");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает .4");

            l = new DoubleLexer("4.");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает 4.");

            l = new DoubleLexer(".");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает .");

            l = new DoubleLexer("");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает пустую строку");
        }

    }

    [TestFixture]
    public class TestQuotedStringLexer
    {

        [Test]
        public void TestQuotedStringParse()
        {
            StringLexer l = new StringLexer("''");
            Assert.IsTrue(l.Parse(), "Не пропускает ''");

            l = new StringLexer("'aa#2N3@_3-x//45'");
            Assert.IsTrue(l.Parse(), "Не пропускает 'aa#2N3@_3-x//45'");

            l = new StringLexer("'23 3 a'");
            Assert.IsTrue(l.Parse(), "Не пропускает '23 3 a'");
        }

        [Test]
        public void TestQuotedStringFail()
        {
            StringLexer l = new StringLexer("'");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает '");

            l = new StringLexer("aa'");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает aa'");

            l = new StringLexer("b 'add'");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает b 'add'");

            l = new StringLexer("'add' d");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает 'add' d");
        }

    }

    [TestFixture]
    public class TestCommentLexer
    {

        [Test]
        public void TestCommentParse()
        {
            CommentLexer l = new CommentLexer("/**/");
            Assert.IsTrue(l.Parse(), "Не пропускает /**/");

            l = new CommentLexer("/*as3 @4&*_ -dd %~f*/");
            Assert.IsTrue(l.Parse(), "Не пропускает /*as3 @4&*_ -dd %~f*/");

            l = new CommentLexer("/*asda \n */");
            Assert.IsTrue(l.Parse(), "Не пропускает перенос строки");
        }

        [Test]
        public void TestCommentFail()
        {
            CommentLexer l = new CommentLexer("*/");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает */'");

            l = new CommentLexer("/*");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает /*'");

            l = new CommentLexer("/* \n");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает */ \n'");

            l = new CommentLexer("/**/*/");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает /**/*/'");
        }

    }
    
    [TestFixture]
    public class TestIdChainLexer
    {
        [Test]
        public void TestIdChainParse()
        {
            IdentChainLexer l = new IdentChainLexer("Id1");
            Assert.IsTrue(l.Parse(), "Не пропускает Id1");
            
            l = new IdentChainLexer("Id1.Id2");
            Assert.IsTrue(l.Parse(), "Не пропускает Id1.Id2");
            
            l = new IdentChainLexer("uUd.k_22.sa3");
            Assert.IsTrue(l.Parse(), "Не пропускает uUd.k_22.sa3");
        }
        
        [Test]
        public void TestIdChainFail()
        {
            IdentChainLexer l = new IdentChainLexer("3Id1");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает 3Id1");
            
            l = new IdentChainLexer(".Id2");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает .Id2");
            
            l = new IdentChainLexer("uUd.");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает uUd.");
            
            l = new IdentChainLexer("uUd.3sa");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает uUd.3sa");
            
            l = new IdentChainLexer("uUd. _sa");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает uUd. _sa");
            
            l = new IdentChainLexer("uUd,sa");
            Assert.Throws<LexerException>(() => { l.Parse(); }, "Пропускает uUd,sa");
            
        }
    }
}