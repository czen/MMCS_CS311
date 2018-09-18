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
            string fileContents = @":begin line1
id23 := 24;  line2
cycle ; 2 id258, id29 ; line3
1 + x - 3 / iend div 22 mod 3 or not true line4
x += 1 y -= 9 z /= y y *= 8 line5
1 < 2 > 0 <= 9 >= 6 = 6 <> 8 line6
// 1 < 2 > 0 //<= 9 >= 6 = 6 <> 8 line7
{comment} line8
end
//
{}";
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