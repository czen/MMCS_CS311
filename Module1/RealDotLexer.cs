using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerTasks
{
    class RealDotLexer : Lexer
    {
        protected System.Text.StringBuilder intString;
        public String message;
        public double number;

        public RealDotLexer(string input)
            : base(input)
        {
            intString = new System.Text.StringBuilder();
        }

        public override void Parse()
        {
            NextCh();
            if (currentCh == '+' || currentCh == '-')
            {
                message += currentCh;
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

            if (currentCh == '.')
            {
                message += currentCh;
                NextCh();
            }
            else
            {
                Error();
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
            
            System.Console.WriteLine("Real numbers with dot is recognized " + message);

        }

        public static void Testing()
        {
            var tests = new Dictionary<string, string>{
                { "+1.4", "+1.4" },
                { "-4.3", "-4.3"},
                { "234.0", "234.0"},
                { "0.46", "0.46"},
                { "90424.12300", "90424.12300"},
                { "", "error"},
                { ".89", "error"},
                { "123.", "error"},
                { "f12.4", "error"},
                { "123.44;", "error"}
            };

            int passedTest = 0;

            foreach (var t in tests)
            {
                var L = new RealDotLexer(t.Key);
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
