using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SimpleLangParser;
using SimpleLangLexer;

namespace SimpleLangParserTest
{
    class Program
    {
        static void Test0(string[] args)
        {
            string fileContents = @"begin
    a := 2;
    cycle a
    begin
        b := a;
        c := 234
    end
end";
            TextReader inputReader = new StringReader(fileContents);
            Lexer l = new Lexer(inputReader);
            Parser p = new Parser(l);
            try
            {
                p.Progr();
                if (l.LexKind == Tok.EOF)
                {
                    Console.WriteLine("Program successfully recognized");
                }
                else
                {
                    p.SyntaxError("end of file was expected");
                }
            }
            catch (ParserException e)
            {
                Console.WriteLine("lexer error: " + e.Message);
            }
            catch (LexerException le)
            {
                Console.WriteLine("parser error: " + le.Message);
            }
        }

        static void Test1(string[] args)
        {
            string fileContents = @"begin
    a := 2;
    while a do
    begin
        b := a;
        a := 0
    end
end";
            TextReader inputReader = new StringReader(fileContents);
            Lexer l = new Lexer(inputReader);
            Parser p = new Parser(l);
            try
            {
                p.Progr();
                if (l.LexKind == Tok.EOF)
                {
                    Console.WriteLine("Program successfully recognized");
                }
                else
                {
                    p.SyntaxError("end of file was expected");
                }
            }
            catch (ParserException e)
            {
                Console.WriteLine("lexer error: " + e.Message);
            }
            catch (LexerException le)
            {
                Console.WriteLine("parser error: " + le.Message);
            }
        }

        static void Test2(string[] args)
        {
            string fileContents = @"begin
    a := 2;
    for i := 0 to a do
    begin
    b := i;
    c := a
    end
end";
            TextReader inputReader = new StringReader(fileContents);
            Lexer l = new Lexer(inputReader);
            Parser p = new Parser(l);
            try
            {
                p.Progr();
                if (l.LexKind == Tok.EOF)
                {
                    Console.WriteLine("Program successfully recognized");
                }
                else
                {
                    p.SyntaxError("end of file was expected");
                }
            }
            catch (ParserException e)
            {
                Console.WriteLine("lexer error: " + e.Message);
            }
            catch (LexerException le)
            {
                Console.WriteLine("parser error: " + le.Message);
            }
        }

        static void Test3(string[] args)
        {
            string fileContents = @"begin
    a := 2;
    if a then
    begin
    a := 3
    end
    else
    begin
    a := 4
    end; 
    if a then
    begin
    a := 1
    end
end";
            TextReader inputReader = new StringReader(fileContents);
            Lexer l = new Lexer(inputReader);
            Parser p = new Parser(l);
            try
            {
                p.Progr();
                if (l.LexKind == Tok.EOF)
                {
                    Console.WriteLine("Program successfully recognized");
                }
                else
                {
                    p.SyntaxError("end of file was expected");
                }
            }
            catch (ParserException e)
            {
                Console.WriteLine("lexer error: " + e.Message);
            }
            catch (LexerException le)
            {
                Console.WriteLine("parser error: " + le.Message);
            }
        }

        static void Test4(string[] args)
        {
            string fileContents = @"begin
    a := 2+3;
    a := a * 2 + 1;
    a := ((2 - 1) / 5) * 7
end";
            TextReader inputReader = new StringReader(fileContents);
            Lexer l = new Lexer(inputReader);
            Parser p = new Parser(l);
            try
            {
                p.Progr();
                if (l.LexKind == Tok.EOF)
                {
                    Console.WriteLine("Program successfully recognized");
                }
                else
                {
                    p.SyntaxError("end of file was expected");
                }
            }
            catch (ParserException e)
            {
                Console.WriteLine("lexer error: " + e.Message);
            }
            catch (LexerException le)
            {
                Console.WriteLine("parser error: " + le.Message);
            }
        }

        static void Main(string[] args)
        {
            Test0(args);
            Test1(args);
            Test2(args);
            Test3(args);
            Test4(args);
        }
    }
}