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
end";

            string file2 = @"begin
// something happening
int x := 42;
a += 4;
boo /= 1;";

            // незакрытый до конца файла комментарий
            string file3 = @"meow := 14;
end
4 - 11;  div { 1,7 
not b
cycle x";

            test(file1);
            test(file2);
            test(file3);
           
        }
    }
}
