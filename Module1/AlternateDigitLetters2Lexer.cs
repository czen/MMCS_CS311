using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerTasks
{
    class AlternateDigitLetters2Lexer : Lexer
    {
        protected System.Text.StringBuilder intString;
        public String message;

        public AlternateDigitLetters2Lexer(string input)
            : base(input)
        {
            intString = new System.Text.StringBuilder();
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
            if (char.IsLetter(currentCh))
            {
                message += currentCh;
                NextCh();
            }
            
            while (char.IsLetter(currentCh) || char.IsDigit(currentCh))
            {
                if (char.IsDigit(currentCh))
                {
                    message += currentCh;
                    NextCh();
                }
                else
                {
                    Error();
                }
                if (currentCharValue == -1)
                    break;

                if (char.IsDigit(currentCh))
                {
                    message += currentCh;
                    NextCh();
                }
                if (currentCharValue == -1)
                    break;

                if (char.IsLetter(currentCh))
                {
                    message += currentCh;
                    NextCh();
                }
                else
                {
                    Error();
                }
                if (currentCharValue == -1)
                    break;
                if (char.IsLetter(currentCh))
                {
                    message += currentCh;
                    NextCh();
                }
            }

            if (currentCharValue != -1) // StringReader вернет -1 в конце строки
            {
                Error();
            }
            
            System.Console.WriteLine("Alternate Digits and Chars with groups no more than 2 recognized " + message);

        }

        public static void Testing()
        {
            var tests = new Dictionary<string, string>{
                { "aa12c23dd1", "aa12c23dd1" },
                { "a", "a"},
                { "a2a", "a2a"},
                { "af45", "af45"},
                { "a6ar5t12", "a6ar5t12"},
                { "a88hye", "error"},
                { "ar123", "error"},
                { "12gh", "error" },
                { "t2gh;", "error" },
                { "", "error"}
            };

            int passedTest = 0;

            foreach (var t in tests)
            {
                var L = new AlternateDigitLetters2Lexer(t.Key);
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
