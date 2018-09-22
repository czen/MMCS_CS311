using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerTasks
{
    class DigitsSpacesLexer : Lexer
    {
        protected System.Text.StringBuilder mesString;
        public String message;

        public DigitsSpacesLexer(string input)
            : base(input)
        {
            mesString = new System.Text.StringBuilder();
            message = "";
        }

        public override void Parse()
        {
            NextCh();
            if (char.IsDigit(currentCh))
            {
                message += currentCh;
                NextCh();
            }
            else
            {
                Error();
            }
            while (char.IsDigit(currentCh))
            {
                message += currentCh;
                NextCh();
            }

            if (currentCh == ' ')
            {
                NextCh();
            }
            else
            {
                Error();
            }
            while (currentCh == ' ')
            {
                NextCh();
            }

            if (char.IsDigit(currentCh))
            {
                message += currentCh;
                NextCh();
            }
            else
            {
                Error();
            }
            while (char.IsDigit(currentCh))
            {
                message += currentCh;
                NextCh();
            }

            if (currentCharValue != -1)
            {
                Error();
            }

            System.Console.WriteLine("string with separated chars spaces recognised " + message);
        }

        public static void Testing()
        {
            var tests = new Dictionary<string, string>{
                { "1 23", "123" },
                { "12  567", "12567"},
                { "32          222", "32222"},
                { "32          1", "321"},
                { "4", "error"},
                { "42 ", "error"},
                { "1131 ;", "error"},
                { "12, 45", "error"},
                { "56     S", "error"}
            };

            int passedTest = 0;
            foreach (var t in tests)
            {
                var L = new DigitsSpacesLexer(t.Key);
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
