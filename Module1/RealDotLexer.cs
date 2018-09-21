﻿using System;
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
                if (currentCh == '-')
                {
                    message += currentCh;
                }
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


            if (currentCharValue != -1) // StringReader вернет -1 в конце строки
            {
                Error();
            }

            number = Convert.ToDouble(message);
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
