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
                }
                else
                {
                    System.Console.WriteLine("Test is not passed");
                }
            }

        }
    }
}
