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
        public static void Main()
        {
            string fileContents = @"begin 
or9
id23 := 24; +7 += :9
cycle; 2 id258 id29 ; 
- -9 -=
,
:
+
-
*
/
and
or
not
div mod
+= -= *= /=
> < >= <= = <>
//> < >= <= = <>
{+= -= *= /=123


1231231231221312
}
+-/+=
--=
<<=
or8
end";
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

            //-------------Примеры с ошибками--------------------------------
            fileContents = @"begin 
{ id45
end";
            inputReader = new StringReader(fileContents);
            l = new Lexer(inputReader);
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

            //-------------------------------------------------
            fileContents = @"begin
& //";


            inputReader = new StringReader(fileContents);
            l = new Lexer(inputReader);
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


            Console.ReadKey();

        }
    }
}