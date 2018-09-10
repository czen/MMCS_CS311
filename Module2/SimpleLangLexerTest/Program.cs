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
        public static string[] testStrings = {
            @"begin 
            ,:+-*/ div mod and or not 
            end",
            @"begin
            *-+= */-= -**= -*/=
            end",
            @"begin
            > < >= <= = <> 
            end",
            @"begin
            //ggffgfgfgfggf//fddfdf
            end",
            @"begin
            {
            fsffgfgfg
            }
            <><><
            {
            end"
        };
        public static void TestTasks()
        {
            foreach (string fileContents in testStrings)
            {
                System.Console.WriteLine("String is \n<" + fileContents + ">\n");
                TextReader inputReader = new StringReader(fileContents);
                Lexer l = new Lexer(inputReader);
                System.Console.WriteLine("Tokens are \n<");
                try
                {
                    do
                    {
                        Console.WriteLine(l.TokToString(l.LexKind));
                        l.NextLexem();
                    } while (l.LexKind != Tok.EOF);
                    System.Console.WriteLine(">\n");
                }
                catch (LexerException e)
                {
                    Console.WriteLine("lexer error: " + e.Message);
                }
            }
        }

        public static void Main()
        {
            TestTasks();
        }
    }
}