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
        b:=a
    end;

    while a do
    begin
       b:=1;
       c:=b
    end;

    for i := 1 to 5 do
    begin
        c:=i
    end;

    if (a+1)/b then c:=z+b else c:=44+2;
    if z then c:=z*z
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
