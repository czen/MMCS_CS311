﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SimpleLangLexer;
using SimpleLangParser;

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
                                            while a do
                                            begin
                                                while 5 do
                                                    a := 2;
                                                a := 2;
                                                b := 1
                                            end    
                                        end;
                                        for a := 0 to 5 do
                                            for b := 0 to 10 do
                                            begin
                                                if ((((a+3)-2)+3)*1)-(((a)))+(a-1-2-3-4-5-(6)) then
                                                begin
                                                    if b then a:=3 else b:=3;
                                                    for a:=3 to 4 do
                                                        b:=3
                                                end;
                                                c := 3;
                                                d := 2
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