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
        public static bool Test(string fileContents)
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
                return false;
            }
            return true;
        }

        public static void Testing()
        {
            string[] fileContents = {
                @"begin 
id23 := 24;  
cycle ; 2 id258 id29 div mod ; : + ;
- * / not ; and or id45 *= 12 ID45 += 0
89 -= /= / <> < > <= >= =
end",
                 @"begin 
id23 := 24;  
cycle ; 2 id258 id29 div mod ; : + ;
- * / not ; and or id45 *= 12 ID45 += 0
89 -= /= / <> < > <= >= =
// += dkfkdf
//
{ += *= 09 ju243 }  {854 ufhf += -= }
//
// {
{}
end",
                  @"begin 
id23 := 24;  
cycle ; 2 id258 id29 div mod ; : + ;
{ not cycle >> >,.<><>@#$
}
// }
// += dkfkdf &^&*
//
{ += *= 09 ju243 }
end",
            };

            int cntSuccessfullTests = 0;
            int cntTests = fileContents.Length;
            foreach (string filec in fileContents)
            {
                bool success = Test(filec);
                if (success)
                    cntSuccessfullTests++;
            }

            string negTest =
@"begin 
id23 := 24;  
cycle ; 2 id258 id29 div mod ; : + ;
{ not cycle >> >,.<><>@#$
}
// }
{
// += dkfkdf &^&*
//
{ += *= 09 ju243 }
{
end";
            bool successNeg = !Test(negTest);
            cntTests++;
            if (successNeg)
                cntSuccessfullTests++;

            Console.WriteLine("{0} / {1} tests passed", cntSuccessfullTests, cntTests);
        }

        public static void Main()
        {
            Testing();
        }
    }
}