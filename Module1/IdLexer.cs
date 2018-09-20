using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerTasks
{
    public class IdLexer : Lexer
    {

        protected System.Text.StringBuilder idString;
        public String id;

        public IdLexer(string input)
            : base(input)
        {
            idString = new System.Text.StringBuilder();
        }

        public override void Parse()
        {
            NextCh();

            if (char.IsLetter(currentCh))
            {
                id += currentCh;
                NextCh();
            }
            else
            {
                Error();
            }

            while (char.IsDigit(currentCh) || char.IsLetter(currentCh))
            {
                id += currentCh;
                NextCh();
            }

            if (currentCharValue != -1)
            {
                Error();
            }

            System.Console.WriteLine("id is recognized " + id);

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
