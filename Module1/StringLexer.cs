using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerTasks
{
    class StringLexer : Lexer
    {
        protected System.Text.StringBuilder intString;
        public String message;

        public StringLexer(string input)
            : base(input)
        {
            intString = new System.Text.StringBuilder();
        }

        public override void Parse()
        {
            NextCh();

            if (currentCh == '\'')
            {
                message += currentCh;
                NextCh();
            }
            else
            {
                Error();
            }

            while (currentCharValue != -1 && currentCh != '\'')
            {
                message += currentCh;
                NextCh();
            }

            if (currentCh == '\'')
            {
                message += currentCh;
                NextCh();
            }
            else
            {
                Error();
            }

            if (currentCharValue != -1)
            {
                Error();
            }
            
            System.Console.WriteLine("string is recognized " + message);

        }

        public static void Testing()
        {
            var tests = new Dictionary<string, string>{
                { "''", "''" },
                { "'1'", "'1'"},
                { "'g '", "'g '"},
                { "'abra768\"'", "'abra768\"'"},
                { "'tl;dr'", "'tl;dr'"},
                { "tl3;dr'", "error"},
                { "'12tt", "error"},
                { "'12t't", "error"},
                { "", "error"}
            };

            int passedTest = 0;

            foreach (var t in tests)
            {
                var L = new StringLexer(t.Key);
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
