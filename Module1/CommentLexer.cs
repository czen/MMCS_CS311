using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerTasks
{
    class CommentLexer : Lexer
    {
        protected System.Text.StringBuilder intString;
        public String message;

        public CommentLexer(string input)
            : base(input)
        {
            intString = new System.Text.StringBuilder();
        }

        public override void Parse()
        {
            NextCh();
            if (currentCh == '/' )
            {
                message += currentCh;
                NextCh();
            }
            else
            {
                Error();
            }
            if (currentCh == '*')
            {
                message += currentCh;
                NextCh();
            }
            else
            {
                Error();
            }

            bool prevIsStar = false;
            bool commentEnd = false;
            while (currentCharValue != -1)
            {
                message += currentCh;
                if (prevIsStar && currentCh == '/')
                {
                    NextCh();
                    commentEnd = true;
                    break;
                }
                prevIsStar = currentCh == '*';                
                NextCh();
            }

            if (currentCharValue != -1 || !commentEnd) // StringReader вернет -1 в конце строки
            {
                Error();
            }

            System.Console.WriteLine("comment is recognized " + message);

        }

        public static void Testing()
        {
            var tests = new Dictionary<string, string>{
                { "/**/", "/**/" },
                { "/*1*/", "/*1*/"},
                { "/****/", "/****/"},
                { "/* ;89**/", "/* ;89**/"},
                { "/* g;,1.w&^*/", "/* g;,1.w&^*/"},
                { "", "error"},
                { "*/", "error"},
                { "/ /", "error"},
                { "/ * */", "error"},
                { "/* * /", "error"},
                { "/*", "error"},
                { "/*/", "error"}
            };

            int passedTest = 0;

            foreach (var t in tests)
            {
                var L = new CommentLexer(t.Key);
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
