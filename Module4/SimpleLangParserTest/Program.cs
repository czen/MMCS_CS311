using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SimpleLangParser;
using SimpleLexer;

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
        while 2 do begin
           a:=2
end;
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
    }
}
