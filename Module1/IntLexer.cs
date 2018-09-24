using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerTasks
{
    public class IntLexer : Lexer
    {

        protected System.Text.StringBuilder intString;
        public String numberString;
        public int number;

        public IntLexer(string input)
            : base(input)
        {
            intString = new System.Text.StringBuilder();
        }

        public override void Parse()
        {
            NextCh();
            if (currentCh == '+' || currentCh == '-')
            {
                if (currentCh == '-')
                {
                    numberString += currentCh;
                }
                NextCh();
            }

            if (char.IsDigit(currentCh))
            {
                numberString += currentCh;
                NextCh();
            }
            else
            {
                Error();
            }

            while (char.IsDigit(currentCh))
            {
                numberString += currentCh;
                NextCh();
            }


            if (currentCharValue != -1) // StringReader вернет -1 в конце строки
            {
                Error();
            }

            number = Convert.ToInt32(numberString);
            System.Console.WriteLine("Integer is recognized " + number);

        }

        public static void Testing()
        {
            var tests = new Dictionary<string, string>{
                { "+1234", "1234" },
                { "105", "105"},
                { "-6", "-6"},
                { "990", "990"},
                { "94172", "94172"},
                { "tl3;dr", "error"},
                { "12tt", "error"},
                { "", "error"}
            };

            int passedTest = 0;

            foreach (var t in tests)
            {
                var L = new IntLexer(t.Key);
                bool passed = false;
                try
                {
                    L.Parse();
                    passed = L.numberString.Equals(t.Value);
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
