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
            var strList = new List<string>();
            strList.Add(@"begin
            id23:= 24;
            cycle; 2 id258 id29;
            end");
            strList.Add(@"begin
             id1 := 2;
             id3 := 1 + 2;
             id3 := 1 mod 2;
             id4 := id1 : id2;
             id4 := id1 - id2;
             id5 := 1 and 1 or 0;
             id5 := not id5;
             id5 := 10 * id5 div 3, id5 / 6;
             end");
            strList.Add("id1 = 4");
            strList.Add(@"begin
               id1 := 2;
               id2 *= 5 *2;
               id1 /= 2;
               id2 -= 10;
               id4 += 1 / 2;
               end");
            strList.Add(@"begin
             id1 := 1 = 2 < 3 <= 4 >= 2;
             id5 := 2 = 3 <> 45;
            ");
            strList.Add(@"begin
                id1 <>= 4
                // 5 + 2 // 2
                id3 := 3 // hello
            end");
            strList.Add(@"begin
                id2 := 1
                { sdidsj
                id1 := 2
                id2 += 1}
                id1 := 3;
                end");
            strList.Add(@"
                id1 ? 2;");
            strList.Add(@"begin
                id2 := 1
                { sdidsj
                id1 := 2
                id2 += 1
                id1 := 3;
                end");
            foreach (var fileContents in strList)
            {
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
                Console.WriteLine("---------------");
            }
        }
    }
}