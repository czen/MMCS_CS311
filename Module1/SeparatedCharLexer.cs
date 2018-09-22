using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerTasks
{
    public class SeparatedCharsLexer : Lexer
    {
        protected System.Text.StringBuilder mesString;
        public String message;

        public SeparatedCharsLexer(string input)
            : base(input)
        {
            mesString = new System.Text.StringBuilder();
            message = "";
        }

        public override void Parse()
        {
            NextCh();
            if (char.IsLetter(currentCh))
            {
                message += currentCh;
                NextCh();
            }
            else
            {
                Error();
            }

            while (true)
            {
                if (char.IsLetter(currentCh))
                {
                    message += currentCh;
                    NextCh();
                }
                else if (currentCh == ',' || currentCh == ';')
                {
                    NextCh();
                    break;
                }
                else
                {
                    Error();
                }
            }

            if (char.IsLetter(currentCh))
            {
                message += currentCh;
                NextCh();
            }
            else
            {
                Error();
            }

            while (char.IsLetter(currentCh))
            {
                message += currentCh;
                NextCh();
            }

            if (currentCharValue != -1)
            {
                Error();
            }

            System.Console.WriteLine("string with separated chars by ; or , is recognised " + message);
        }

        public static void Testing()
        {
            var tests = new Dictionary<string, string>{
                { "a;", "error" },
                { "a,h", "ah"},
                { ";fr", "error"},
                { "abg,abg", "abgabg"},
                { "abg;;", "error"},
                { ",,", "error"},
                { "tl;dr", "tldr"},
                { ",glO", "error"},
                { "", "error"}
            };

            int passedTest = 0;
            foreach (var t in tests)
            {
                var L = new SeparatedCharsLexer(t.Key);
                bool passed = false;
                try
                {
                    L.Parse();
                    passed = L.message.Equals(t.Value);
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
