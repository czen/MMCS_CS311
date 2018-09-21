using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerTasks
{
    public class IntZeroLexer : Lexer
    {

        protected System.Text.StringBuilder intString;
        public String numberString;
        public int number;

        public IntZeroLexer(string input)
            : base(input)
        {
            intString = new System.Text.StringBuilder();
        }

        public override void Parse()
        {
            NextCh();
            if (currentCh == '+' || currentCh == '-')
            {
                numberString += currentCh;
                NextCh();
            }

            if (char.IsDigit(currentCh) && currentCh != '0')
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
            System.Console.WriteLine("Integer without 0 recognized " + number);

        }

        public static void Testing()
        {
            var tests = new Dictionary<string, string>{
                { "-10", "-10" },
                { "24", "24"},
                { "100", "100"},
                { "-505", "-505"},
                { "-012", "error"},
                { "0123", "error"},
                { "1,glO", "error"}
            };

            int passedTest = 0;
            foreach (var t in tests)
            {
                var L = new IntZeroLexer(t.Key);
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
                    passedTest++;
                    System.Console.WriteLine("Test is passed");
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
