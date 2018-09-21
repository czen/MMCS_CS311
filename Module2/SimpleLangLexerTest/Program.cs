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
        public static void test(string fileContents)
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
            string file1 = @"begin 
id23 := 24;  
{cycl}e ; 2 id258 id29 ; 
4 div 2 mod 5 or a not b
end";

            string file2 = @"begin
            wow = 2 <> 17 // something happening
            int x := 42;
            a += 4;
            a + 5 - 4
            boo /= 1;
            42 / a * p *= e
            //";



            // незакрытый до конца файла комментарий
            string file3 = @"meow := 14;
end
4 - 11;  div { 1,7 
not b
cycle x";

            string file4 = @"<<>++=*/**/";


            Console.WriteLine("\n --- Test 1 --- ");
            test(file1);
            Console.WriteLine("\n --- Test 2 --- ");
            test(file2);
            Console.WriteLine("\n --- Test 3 --- ");
            test(file3);
            Console.WriteLine("\n --- Test 4 --- ");
            test(file4);
        }
    }
}
