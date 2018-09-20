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
                numberString += currentCh;
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
                { "a;", "a" },
                { ";fr", "fr"},
                { "abg,abg", "abgabg"},
                { "abg;;", "error"},
                { ",,", "error"},
                { "tl;dr", "tldr"},
                { ",glO", "glO"}
            };

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
                    passed = true;
                }

                if (passed)
                {
                    System.Console.WriteLine("Test is passed");
                }
                else
                {
                    System.Console.WriteLine("Test is not passed");
                }
            }

        }
    }
}
