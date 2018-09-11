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
        public static void TestLexer(string fileContents)
        {
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
            Console.WriteLine("------------------- test 1 -------------------");
            string fileContents = @"begin 
id12 := 10 + 2 // add
3 < 4;
end";
            TestLexer(fileContents);

            Console.WriteLine("------------------- test 2 -------------------");
            fileContents = @"begin 
{value} val /= 5 or 3; 
// abc;
end";
            TestLexer(fileContents);

            Console.WriteLine("------------------- test 3 -------------------");
            fileContents = @" { simple
lang lexer
val = 1 }
a += 3 >= 1,
cycle:   24 div 6; 
end";
            TestLexer(fileContents);

            Console.WriteLine("------------------- test 4 -------------------");
            fileContents = @" begin
{ lalala
123";
            TestLexer(fileContents);

            Console.WriteLine("------------------- test 5 -------------------");
            fileContents = @"begin   
a := 12.3;
end";
            TestLexer(fileContents);
        }
    }
}