using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SimpleLangLexer;

namespace SimpleLangLexerTest
{
    class Program
    {
        public static void test1()
        {
            string fileContents = @"begin 
                                    id0 := 3, id1 := 4;
                                    id2 : id3;
                                    id0 := id0 mod 10 + id0 div 3;
                                    id2 := id2 / 3 - 5 * id1;
                                    id4 := 1 and 0 or 4;
                                    id5 := not id2;
                                    end";
            TextReader inputReader = new StringReader(fileContents);
            Lexer l = new Lexer(inputReader);
            try
            {
                do
                {
                    Console.WriteLine(l.TokToString(l.LexKind));
                    l.NextLexem();
                } while (l.LexKind != Tok.EOF);
            }
            catch (LexerException e)
            {
                Console.WriteLine("lexer error: " + e.Message);
            }
        }
        public static void test2()
        {
            string fileContents = @"begin
                                    id0 += id0; id0 -= id0;
                                    id0 *= id0; id0 /= id0;
                                    end";
            TextReader inputReader = new StringReader(fileContents);
            Lexer l = new Lexer(inputReader);
            try
            {
                do
                {
                    Console.WriteLine(l.TokToString(l.LexKind));
                    l.NextLexem();
                } while (l.LexKind != Tok.EOF);
            }
            catch (LexerException e)
            {
                Console.WriteLine("lexer error: " + e.Message);
            }
        }
        public static void test3()
        {
            string fileContents = @"begin
                                    id0 > id1; 
                                    id1 < id0;
                                    3 >= 4; 5 <= 6;
                                    7 = 8; 9 <> 10;
                                    end";
            TextReader inputReader = new StringReader(fileContents);
            Lexer l = new Lexer(inputReader);
            try
            {
                do
                {
                    Console.WriteLine(l.TokToString(l.LexKind));
                    l.NextLexem();
                } while (l.LexKind != Tok.EOF);
            }
            catch (LexerException e)
            {
                Console.WriteLine("lexer error: " + e.Message);
            }
        }
        public static void test4()
        {
            string fileContents = @"begin
                                    // id0 > id1; 
                                    id1 < id0;
                                    3 >= 4; // 5 <= 6;
                                    7 = 8;
                                    end";
            TextReader inputReader = new StringReader(fileContents);
            Lexer l = new Lexer(inputReader);
            try
            {
                do
                {
                    Console.WriteLine(l.TokToString(l.LexKind));
                    l.NextLexem();
                } while (l.LexKind != Tok.EOF);
            }
            catch (LexerException e)
            {
                Console.WriteLine("lexer error: " + e.Message);
            }
        }
        public static void Main()
        {
            Console.WriteLine("=============test 1============");
            //test1();
            Console.WriteLine("=============test 2============");
            test2();
            Console.WriteLine("=============test 3============");
            test3();
            Console.WriteLine("=============test 4============");
            test4();
        }
    }
}