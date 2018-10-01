using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SimpleLexer;

namespace SimpleLangLexerTest
{
    class Program
    {
        public static void Main()
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
    }
}