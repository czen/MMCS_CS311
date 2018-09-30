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

                                    a := 2;
                                    cycle a
                                    begin
                                        b := a;
                                        c := 234
                                    end;
                            
                                    while u do
                                    begin
                                        cycle 78
                                        t:= 9;
                                        pop:= 90
                                    end;

                                    for HH:= 5 to 10 do
                                         k := 8;

                                    if exp then ror := 666;
                                    
                                    if exp then pk := 777
                                    else pk := 8;

                                    a:= 7*9 / (90 + 7) * 6                                    

                                    end";

            Console.WriteLine("-------------------------------------------------");

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

            //-------------------Неправильный пример --------------------

            fileContents = @"begin
                                    a := 2;

                                    a := 2;
                                    cycle a
                                    begin
                                        b := a;
                                        c := 234
                                    end;
                            
                                    while u 
                                    begin
                                        cycle 78
                                        t:= 9;
                                        pop:= 90
                                    end ";

            Console.WriteLine("\n-------------------------------------------------");
           

            inputReader = new StringReader(fileContents);
            l = new Lexer(inputReader);
            p = new Parser(l);
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


            Console.ReadKey();
        }
    }
}
