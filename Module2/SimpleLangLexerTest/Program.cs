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
                                    id1, id2,  id3, id4;
                                    34 + 32; 1/2, 2/3 + id1*id2;
                                    a mod 2; b div 3;
                                    a: integer; a:=2;
                                    not a or b and c;
                                    a += 2;
                                    a < b, a > b, a <= b, b >= a <= c;
                                    a <> b; a := b; a = b; // comment
                                    b -=1, c/=2, d*=0;
                                    a += a + 2;
                                    // a := a + 2;
                                    b { := a + 3};
                                    {Hello
                                    
                                    hello}
                                    {}
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