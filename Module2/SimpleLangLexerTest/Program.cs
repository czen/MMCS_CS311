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
        public static void Test0()
        {
            Console.WriteLine("---------------------");
            Console.WriteLine("Test");
            Console.WriteLine("---------------------");
            Console.WriteLine();

            string fileContents = @"begin  
id25 := 3;
cycle ; 2 id258  id29 ; 
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
            Console.WriteLine();

        }


        public static void Test1()
        {
            Console.WriteLine("---------------------");
            Console.WriteLine("Task 1");
            Console.WriteLine("---------------------");
            Console.WriteLine();

            string fileContents = @"id17 :12 + 25;
    id25 or not ar and er; 
id1 -2 / 3, 12; 
id12 := id15 mod 2*11; 
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
            Console.WriteLine();
        }


        public static void Test2()
        {
            Console.WriteLine("---------------------");
            Console.WriteLine("Task 2");
            Console.WriteLine("---------------------");
            Console.WriteLine();

            string fileContents = @"id17 +=12 -= 25;
   id1*=123 /= 1 + a2";
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
            Console.WriteLine();
        }

        public static void Test3()
        {
            Console.WriteLine("---------------------");
            Console.WriteLine("Task 3");
            Console.WriteLine("---------------------");
            Console.WriteLine();

            string fileContents = @"id17 > 25;
   id1<>a2 <=1, a2: idq>=23 = 1";
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
            Console.WriteLine();
        }

        public static void Test4()
        {
            Console.WriteLine("---------------------");
            Console.WriteLine("Task 4");
            Console.WriteLine("---------------------");
            Console.WriteLine();

            string fileContents = @"begin
   id14+=2 * 12 //ywetr%^321!lkkd
qe := 123 and we>2";
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
            Console.WriteLine();
        }


        public static void Test5()
        {
            Console.WriteLine("---------------------");
            Console.WriteLine("Task 5");
            Console.WriteLine("---------------------");
            Console.WriteLine();

            string fileContents = @"begin
   id14+=2 * 12 //ywetr%^321!lkkd
qe := 123 and we>2 {djkhsk4
dshj; += 21e3 1 &0, ^
wqe
}
vgh := 2 - 12 <=ejh {} -=1, 12:
rt:=12/=3 {ejwh 12
3 
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
            Console.WriteLine();
        }

        public static void Main()
        {
            Test0();
            Test1();
            Test2();
            Test3();
            Test4();
            Test5();
        }
            
    }
}

/*
 ---------------------
Test
---------------------

BEGIN
ID id25
ASSIGN
INUM 3
SEMICOLON
CYCLE
SEMICOLON
INUM 2
ID id258
ID id29
SEMICOLON
END

---------------------
Task 1
---------------------

ID id17
COLON
INUM 12
PLUS
INUM 25
SEMICOLON
ID id25
OR
NOT
ID ar
AND
ID er
SEMICOLON
ID id1
MINUS
INUM 2
DIVIDE
INUM 3
COMMA
INUM 12
SEMICOLON
ID id12
ASSIGN
ID id15
MOD
INUM 2
MULT
INUM 11
SEMICOLON
END

---------------------
Task 2
---------------------

ID id17
PLUSSELF
INUM 12
MINUSSELF
INUM 25
SEMICOLON
ID id1
MULTSELF
INUM 123
DIVIDESELF
INUM 1
PLUS
ID a2

---------------------
Task 3
---------------------

ID id17
MORE
INUM 25
SEMICOLON
ID id1
NOTEQ
ID a2
LESSEQ
INUM 1
COMMA
ID a2
COLON
ID idq
MOREEQ
INUM 23
EQUAL
INUM 1

---------------------
Task 4
---------------------

BEGIN
ID id14
PLUSSELF
INUM 2
MULT
INUM 12
COMMENT
ID qe
ASSIGN
INUM 123
AND
ID we
MORE
INUM 2

---------------------
Task 5
---------------------

BEGIN
ID id14
PLUSSELF
INUM 2
MULT
INUM 12
COMMENT
ID qe
ASSIGN
INUM 123
AND
ID we
MORE
INUM 2
LONGCOMMENT
ID vgh
ASSIGN
INUM 2
MINUS
INUM 12
LESSEQ
ID ejh
LONGCOMMENT
MINUSSELF
INUM 1
COMMA
INUM 12
COLON
ID rt
ASSIGN
INUM 12
DIVIDESELF
INUM 3
lexer error: Lexical error in line 10:
end
  ^
Comment was not closed
*/
