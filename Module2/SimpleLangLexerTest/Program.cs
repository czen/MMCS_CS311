﻿using System;
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
id23 :=24;
{ fg }
or 2=3 : 2<>3 and 3 >2, 5 mod 2
cycle ; 2>=3 id258 +=id29 ; 
end
//qwdfuhsdf";
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