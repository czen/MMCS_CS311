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
        static void Main(string[] args)
        {
            string fileContents = @"begin
    a := 2;
    cycle a
    begin
        b := a;
        c := 234;
        while c do
            c := 1;
        b := c;
        for a := 3 to 4 do
        begin
            a := b;
            b := c
        end
    end;
    if 3 then 
            abc := 0
        else
            if 0 then
                abc := 1;
    m := 10 - k * 3
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
    }
}
