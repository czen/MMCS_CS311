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
        c := 234
    end;
    while a 
        do a := 5;

    for b := 5 to 7 
        do a := 7;

    if a then 
        b:= 5;

    if b then 
        c := 10
    else
        c := 7;

    Expr 5 * 3 + 5 * 5 * 0;
    Expr
    Expr 1 * 

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
