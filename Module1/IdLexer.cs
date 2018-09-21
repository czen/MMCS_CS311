using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerTasks
{
    public class IdLexer : Lexer
    {

        protected System.Text.StringBuilder idString;
        public String id;

        public IdLexer(string input)
            : base(input)
        {
            idString = new System.Text.StringBuilder();
        }

        public override void Parse()
        {
            NextCh();

            if (char.IsLetter(currentCh))
            {
                id += currentCh;
                NextCh();
            }
            else
            {
                Error();
            }

            while (char.IsDigit(currentCh) || char.IsLetter(currentCh))
            {
                id += currentCh;
                NextCh();
            }

            if (currentCharValue != -1)
            {
                Error();
            }

            System.Console.WriteLine("id is recognized " + id);

        }

        public static void Testing()
        {
            var tests = new Dictionary<string, string>{
                { "a", "a" },
                { "a12", "a12"},
                { "abop4s", "abop4s"},
                { "abop4s;", "error"},
                { "", "error"},
                { "ty6+42", "error"},
                { "g,lO", "error"}
            };


            int passedTest = 0;
            foreach (var t in tests)
            {
                var L = new IdLexer(t.Key);
                bool passed = false;
                try
                {
                    L.Parse();
                    passed = L.id.Equals(t.Value);
                }
                catch (LexerException e)
                {
                    passed = t.Value.Equals("error");
                }

                if (passed)
                {
                    System.Console.WriteLine("Test is passed");
                    passedTest++;
                }
                else
                {
                    System.Console.WriteLine("Test is not passed");
                }
            }

            System.Console.WriteLine("{0} / {1} tests passed", passedTest, tests.Count);

        }
    }
}
