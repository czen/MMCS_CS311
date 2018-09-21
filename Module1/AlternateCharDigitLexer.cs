using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerTasks
{
    public class AlternateCharDigitLexer : Lexer
    {
        protected System.Text.StringBuilder mesString;
        public String message;

        public AlternateCharDigitLexer(string input)
            : base(input)
        {
            mesString = new System.Text.StringBuilder();
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
                if (currentCharValue == -1)
                    break;

                if (!char.IsDigit(currentCh))
                {
                    Error();
                }
                message += currentCh;
                NextCh();

                if (currentCharValue == -1)
                    break;

                if (!char.IsLetter(currentCh))
                {
                    Error();
                }
                message += currentCh;
                NextCh();
            }

            if (currentCharValue != -1)
            {
                Error();
            }

            System.Console.WriteLine("string with alternate chars and digits is recognized " + message);
        }

        public static void Testing()
        {
            var tests = new Dictionary<string, string>{
                { "a1a", "a1a" },
                { "", "error"},
                { "c", "c"},
                { "y78", "error"},
                { "h5g6f7d8kl", "error"},
                { "a1d3f4g5", "a1d3f4g5"},
                { "a2", "a2"}
            };

            int passedTest = 0;
            foreach (var t in tests)
            {
                var L = new AlternateCharDigitLexer(t.Key);
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
