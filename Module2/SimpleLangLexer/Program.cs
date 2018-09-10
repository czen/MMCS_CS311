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
        private static void Test0()
        {
            string fileContents = @"begin 
                                    id23 := 24;  
                                    cycle ; 2 id258 id29 ; 
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

        private static void Test1()
        {
            string fileContents = @"begin 
                                    id1 := 24;
                                    id2 := 2 + 4 * 2;
                                    id1 := id1 / id2 mod 2;
                                    id2 := id1 div 3;      
                                    id3 := 1; id4 := 0;
                                    id4 := not id3 or id4 and 1;
                                    id4 : 1; 
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

        private static void Test2()
        {
            string fileContents = @"begin 
                                    id1 := 4;
                                    id1 += 2;
                                    id1 -= 3;
                                    id1 *= 6;
                                    id1 /= 2;
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

        private static void Test3()
        {
            string fileContents = @"begin 
                                    id1 := 4;
                                    bool1 := id1 < 2;
                                    bool1 := id1 <= 7;
                                    bool1 := id1 > 1;
                                    bool1 := id1 >= 3;
                                    bool1 := id1 = 4;
                                    bool1 := id1 <> 0;
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

        private static void Test4()
        {
            string fileContents = @"begin 
                                    id1 := 1; // some comment
                                    id2 := 0;
                                    // another comment                
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
            //           Test1();
            //            Test2();
            //            Test3();
            Test4();
        }
    }
}