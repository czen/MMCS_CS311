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
    }
}
