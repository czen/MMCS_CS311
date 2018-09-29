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
        public static void TestParser(string fileContents)
        {
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
            Console.WriteLine("------------------- test 1 -------------------");
            string fileContents = @"begin
for a := 4 to 10 do cycle a a := a+1;
if bbb then bbb := 2 else bbb := 1;
if 3/(i4-i3)*v then i4 := 5 
end";

            TestParser(fileContents);

            Console.WriteLine("------------------- test 2 -------------------");
            fileContents = @"begin
while a*(1+c) do a := 0;
for a := 2 do b := 10;
end
";
            TestParser(fileContents);

            Console.WriteLine("------------------- test 3 -------------------");
            fileContents = @"begin
if a-b then 
begin 
for b := a-2 to a+2 do a := a*2;
a := b*4
end

else
while b do a := b
end";

            TestParser(fileContents);
        }
    }
}
