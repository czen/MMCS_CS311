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
        public static void test(string fileContents) {
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
        }
        public static void Main()
        {
            List<string> test_list = new List<string>();
            test_list.Add(@"begin 
id23 := 24;  
cycle ; 2 id258 id29 ; 
end");
            test_list.Add(@"begin 
//id23 := 24;  
{cycle ; 2 id258 id29 ; 
end}");
            test_list.Add(@" 
 + += -= - /= / * *=  : , mod div or not and <> < >  <= >= = ");
            test_list.Add(@" 
 {comment 
id23 := 24;  ");
           
            test_list.ForEach(s => { test(s); Console.WriteLine("--------------"); });
            
        }
    }
}