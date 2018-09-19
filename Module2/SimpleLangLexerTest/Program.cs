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
        public static void Main()
        {
            string fileContents = @"begin 
id23 := 24;  
cycle ; 2 id258 id29 ;
id29 + 2 - id258 ; not and 5 div id5 : 8 ;
id666 += 3 , id29 /= id666 ;
5 < 8 or not 12 mod 7 <= 6 , id5 = 10 , id5 <> 11 ;
id12345 ; // 5 and 7 or 3 ;
12 { } 24 { 36 , 48 } 60 { 72 ; 84
96 108 } , 120
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
    }
}