using System;
using System.Collections.Generic;

namespace lab01
{
    public class LexerException : System.Exception
    {
        public LexerException(string msg)
            : base(msg)
        {
        }

    }

    public class Lexer
    {

        protected int position;
        protected char currentCh;       // очередной считанный символ
        protected int currentCharValue; // целое значение очередного считанного символа
        protected System.IO.StringReader inputReader;
        protected string inputString;

        public Lexer(string input)
        {
            inputReader = new System.IO.StringReader(input);
            inputString = input;
        }

        public void Error()
        {
            System.Text.StringBuilder o = new System.Text.StringBuilder();
            if (inputString == "") {
                o.AppendFormat("Empty string\n", currentCh);
                throw new LexerException(o.ToString());
            }
            o.Append(inputString + '\n');
            o.Append(new System.String(' ', position - 1) + "^\n");
            o.AppendFormat("Error in symbol {0}\n", currentCh);
            throw new LexerException(o.ToString());
        }

        protected void NextCh()
        {
            this.currentCharValue = this.inputReader.Read();
            this.currentCh = (char)currentCharValue;
            this.position += 1;
        }

        public virtual void Parse()
        {

        }
    }

    public class IntLexer : Lexer
    {

        protected System.Text.StringBuilder intString;

        public IntLexer(string input)
            : base(input)
        {
            intString = new System.Text.StringBuilder();
        }

        public override void Parse()
        {
            string s = "";
            NextCh();
            if (currentCh == '+' || currentCh == '-')
            {
                s += currentCh;
                NextCh();
            }

            if (char.IsDigit(currentCh))
            {
                s += currentCh;
                NextCh();
            }
            else
            {
                Error();
            }

            while (char.IsDigit(currentCh))
            {
                s += currentCh;
                NextCh();
            }

            var intDigit = Convert.ToInt32(s);
            System.Console.WriteLine(intDigit);

            if (currentCharValue != -1) // StringReader вернет -1 в конце строки
            {
                Error();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("Integer is recognized\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public class IdLexer : Lexer
    {

        protected System.Text.StringBuilder idString;

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
                NextCh();
            }
            else
            {
                Error();
            }

            while (char.IsLetterOrDigit(currentCh) || currentCh == '_')
            {
                NextCh();
            }


            if (currentCharValue != -1) // StringReader вернет -1 в конце строки
            {
                Error();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("Id is recognized\n");
            Console.ForegroundColor = ConsoleColor.White;

        }
    }

    public class NotStartWithZeroIntLexer : Lexer
    {

        protected System.Text.StringBuilder idString;

        public NotStartWithZeroIntLexer(string input)
            : base(input)
        {
            idString = new System.Text.StringBuilder();
        }

        public override void Parse()
        {
            NextCh();
            if (currentCh == '+' || currentCh == '-')
            {
                NextCh();
            }

            if (currentCh != '0' && currentCharValue != -1)
            {
                NextCh();
            }
            else
            {
                Error();
            }

            while (char.IsDigit(currentCh))
            {
                NextCh();
            }

            if (currentCharValue != -1) // StringReader вернет -1 в конце строки
            {
                Error();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("Not starting with zero int is recognized\n");
            Console.ForegroundColor = ConsoleColor.White;

        }
    }

    public class AlternatingDigitsAndCharsLexer: Lexer
    {

        /*
            Не совсем понял, количество чередующихся должно быть четным, 
            или может прерваться либо на цифре, либо на символе?
            Пока что сделал второй вариант
        */ 
        protected System.Text.StringBuilder idString;

        public AlternatingDigitsAndCharsLexer(string input)
            : base(input)
        {
            idString = new System.Text.StringBuilder();
        }

        public override void Parse()
        {
            NextCh();
            if (char.IsLetter(currentCh))
            {
                NextCh();
            }
            else
            {
                Error();
            }

            while (char.IsLetterOrDigit(currentCh)){
                if (char.IsDigit(currentCh))
                {
                    NextCh();
                }
                else
                {
                    if (currentCharValue == -1){
                        break;
                    }
                    Error();
                }

                if (char.IsLetter(currentCh))
                {
                    NextCh();
                }
                else
                {
                    if (currentCharValue == -1){
                        break;
                    }
                    Error();
                }
            }

            if (currentCharValue != -1) // StringReader вернет -1 в конце строки
            {
                Error();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("Alternating is recognized\n");
            Console.ForegroundColor = ConsoleColor.White;

        }
    }


    public class CharListLexer: Lexer
    {
        protected System.Text.StringBuilder idString;

        public CharListLexer(string input)
            : base(input)
        {
            idString = new System.Text.StringBuilder();
        }

        public override void Parse()
        {
            NextCh();
            List<char> list = new List<char>();

            if (char.IsLetter(currentCh)) {
                list.Add(currentCh);
                NextCh();
            }
            else
                Error();

            while (true) {
                if (currentCh == ',' || currentCh == ';')
                    NextCh();
                else 
                    Error();

                if (char.IsLetter(currentCh)) {
                    list.Add(currentCh);
                    NextCh();
                }
                else
                    Error();

                if (currentCharValue == -1) 
                    break;
            }

            list.ForEach(System.Console.Write);
            System.Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("ListChars is recognized\n");
            Console.ForegroundColor = ConsoleColor.White;

        }
    }

    public class IntSpacesLexer: Lexer
    {
        protected System.Text.StringBuilder idString;

        public IntSpacesLexer(string input)
            : base(input)
        {
            idString = new System.Text.StringBuilder();
        }

        public override void Parse()
        {
            NextCh();
            List<char> list = new List<char>();

            if (char.IsDigit(currentCh)) {
                list.Add(currentCh);
                NextCh();
            }
            else
                Error();

            while (true) {
                while (currentCh == ' ') {
                    NextCh();
                }

                if (char.IsDigit(currentCh)) {
                    list.Add(currentCh);
                    NextCh();
                }
                else
                    Error();

                if (currentCharValue == -1) 
                    break;
            }

            list.ForEach(System.Console.Write);
            System.Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("intChars is recognized\n");
            Console.ForegroundColor = ConsoleColor.White;

        }
    }

    public class GroupsLexer: Lexer
    {
        protected System.Text.StringBuilder idString;

        public GroupsLexer(string input)
            : base(input)
        {
            idString = new System.Text.StringBuilder();
        }

        public override void Parse()
        {
            NextCh();
            List<char> list = new List<char>();

            int digit_counter = 0;
            int letter_counter = 0;

            if (char.IsLetterOrDigit(currentCh)) {
                list.Add(currentCh);
                letter_counter = char.IsLetter(currentCh) ? 1 : 0;
                digit_counter = char.IsDigit(currentCh) ? 1 : 0;
                NextCh();
            }
            else
                Error();

            while (char.IsLetterOrDigit(currentCh)){
                if (char.IsLetter(currentCh)){
                    if (letter_counter < 2) { 
                        list.Add(currentCh);
                        letter_counter += 1;
                        digit_counter = 0;
                        NextCh();
                    } else 
                        Error();
                } 
                
                if (char.IsDigit(currentCh)){
                    if (digit_counter < 2){
                        list.Add(currentCh);
                        digit_counter += 1;
                        letter_counter = 0;
                        NextCh();
                    } else 
                        Error();
                } 
            }

            if (currentCharValue != -1) 
                Error();

            list.ForEach(System.Console.Write);
            System.Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("GroupsLexer is recognized\n");
            Console.ForegroundColor = ConsoleColor.White;

        }
    }
    

    public class FloatLexer: Lexer
    {
        protected System.Text.StringBuilder idString;

        public FloatLexer(string input)
            : base(input)
        {
            idString = new System.Text.StringBuilder();
        }

        public override void Parse()
        {
            NextCh();
            if (char.IsDigit(currentCh))
                NextCh();
            else 
                Error();

            while (char.IsDigit(currentCh)) {
                NextCh();
            }

            if (currentCh != '.')
                Error();
            else 
                NextCh();

            if (char.IsDigit(currentCh))
                NextCh();
            else 
                Error();

            while (char.IsDigit(currentCh))
                NextCh();

            if (currentCharValue != -1) 
                Error();

            System.Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("FloatLexer is recognized\n");
            Console.ForegroundColor = ConsoleColor.White;

        }
    }

    public class StringLexer: Lexer
    {
        protected System.Text.StringBuilder idString;

        public StringLexer(string input)
            : base(input)
        {
            idString = new System.Text.StringBuilder();
        }

        public override void Parse()
        {
            NextCh();
            if (currentCh == '\'')
                NextCh();
            else 
                Error();

            while (char.IsLetter(currentCh)) {
                NextCh();
            }

            if (currentCh != '\'')
                Error();
            else 
                NextCh();

            if (currentCharValue != -1) 
                Error();

            System.Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("StringLexer is recognized\n");
            Console.ForegroundColor = ConsoleColor.White;

        }
    }

    public class CommentLexer: Lexer
    {
        protected System.Text.StringBuilder idString;

        public CommentLexer(string input)
            : base(input)
        {
            idString = new System.Text.StringBuilder();
        }

        public override void Parse()
        {
            NextCh();
            if (currentCh == '/')
                NextCh();
            else 
                Error();

            if (currentCh == '*')
                NextCh();
            else 
                Error();

            while (char.IsLetter(currentCh)) {
                NextCh();
            }

            if (currentCh != '*')
                Error();
            else 
                NextCh();

            if (currentCh != '/')
                Error();
            else 
                NextCh();

            if (currentCharValue != -1) 
                Error();

            System.Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("CommentLexer is recognized\n");
            Console.ForegroundColor = ConsoleColor.White;

        }
    }

    public class IdsLexer: Lexer
    {
        protected System.Text.StringBuilder idString;

        public IdsLexer(string input)
            : base(input)
        {
            idString = new System.Text.StringBuilder();
        }

        public override void Parse()
        {
            NextCh();
            if (char.IsLetterOrDigit(currentCh) || currentCh == '_'){
                NextCh();
            }
            else
                Error();

            while (true){
                while (char.IsLetterOrDigit(currentCh) || currentCh == '_'){
                    NextCh();
                }

                if (currentCh != '.' && currentCharValue != -1)
                    Error();
                
                if (currentCharValue == -1)
                    break;
                
                NextCh();
            }

            if (currentCharValue != -1) 
                Error();

            System.Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("IdsLexer is recognized\n");
            Console.ForegroundColor = ConsoleColor.White;

        }
    }
    public class Program
    {
        public static void Main()
        {
            List<string> intTests = new List<string>() { 
                "",
                " ",
                "1234",
                "0",
                "34343+",
                "d3d3D",
             };
            System.Console.WriteLine("TESTING INTLEXER:");
            foreach (string t in intTests)
            {
                Lexer L = new IntLexer(t);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    System.Console.WriteLine("Test for '{0}':", t);
                    Console.ForegroundColor = ConsoleColor.White;
                    L.Parse();
                }
                catch (LexerException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            List<string> idTests = new List<string>() { 
                    "", 
                    " ",
                    "0", 
                    "34343+",
                    "d3d3D",
                    "id_35",
                    "i",
                    "_",
                    "ID_032_ID",
                    "not_id+",
                    "+id-" };
            System.Console.WriteLine("TESTING IDLEXER:");
            foreach (string t in idTests)
            {
                Lexer L = new IdLexer(t);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    System.Console.WriteLine("Test for '{0}':", t);
                    Console.ForegroundColor = ConsoleColor.White;
                    L.Parse();
                }
                catch (LexerException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            List<string> zeroTests = new List<string>() { 
                    "", 
                    " ",
                    "0", 
                    "-1234", 
                    "+1234", 
                    "1234", 
                    "-0123",
                    "+0123",
                    "0123",
            };
            System.Console.WriteLine("TESTING NotStartWithZeroIntLexer:");
            foreach (string t in zeroTests)
            {
                Lexer L = new NotStartWithZeroIntLexer(t);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    System.Console.WriteLine("Test for '{0}':", t);
                    Console.ForegroundColor = ConsoleColor.White;
                    L.Parse();
                }
                catch (LexerException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            List<string> alternativeTests = new List<string>() { 
                    "", 
                    " ",
                    "34343",
                    "d3d3D",
                    "d3d3",
                    "3d3d",
                    "i",
            };
            System.Console.WriteLine("TESTING ALTERNATIVELEXER:");
            foreach (string t in alternativeTests)
            {
                Lexer L = new AlternatingDigitsAndCharsLexer(t);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    System.Console.WriteLine("Test for '{0}':", t);
                    Console.ForegroundColor = ConsoleColor.White;
                    L.Parse();
                }
                catch (LexerException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            List<string> listTests = new List<string>() { 
                    "", 
                    " ",
                    "a,d,c;d",
                    "a,d,c;d,e,f,q;w,e,r,t;y",
                    "a,dd,c;d",
                    "aa,d,c;d",
                    "1,2,3;4",
                    ";a;d,c;d",
                    "a;d,c;d;",
                    "A;T,D;D",
                    ",",
                    "i",
            };
            System.Console.WriteLine("TESTING LISTLEXER:");
            foreach (string t in listTests)
            {
                Lexer L = new CharListLexer(t);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    System.Console.WriteLine("Test for '{0}':", t);
                    Console.ForegroundColor = ConsoleColor.White;
                    L.Parse();
                }
                catch (LexerException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            List<string> spacesTests = new List<string>() { 
                    "", 
                    " ", 
                    "a d c d",
                    "a   d c     d",
                    "1 2 3 4",
                    "1    2        3   4",
                    "   1    2        3   4",
                    "1    2        3   4   ",
            };
            System.Console.WriteLine("TESTING INTSPACESLEXER:");
            foreach (string t in spacesTests)
            {
                Lexer L = new IntSpacesLexer(t);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    System.Console.WriteLine("Test for '{0}':", t);
                    Console.ForegroundColor = ConsoleColor.White;
                    L.Parse();
                }
                catch (LexerException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            List<string> groupTests = new List<string>() { 
                    "", 
                    " ", 
                    "a d c d",
                    "a1b22a2",
                    "aa12bb22a2",
                    "12bb22a2",
                    "123bb22a2",
                    "1bbb22a2",
                    "1bb22a2a",
            };
            System.Console.WriteLine("TESTING GROUPSLEXER:");
            foreach (string t in groupTests)
            {
                Lexer L = new GroupsLexer(t);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    System.Console.WriteLine("Test for '{0}':", t);
                    Console.ForegroundColor = ConsoleColor.White;
                    L.Parse();
                }
                catch (LexerException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            List<string> floatTests = new List<string>() { 
                    "", 
                    " ", 
                    "1.234",
                    "112421.234000000",
                    ".234000000",
                    "123.",
            };
            System.Console.WriteLine("TESTING GROUPSLEXER:");
            foreach (string t in floatTests)
            {
                Lexer L = new FloatLexer(t);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    System.Console.WriteLine("Test for '{0}':", t);
                    Console.ForegroundColor = ConsoleColor.White;
                    L.Parse();
                }
                catch (LexerException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            List<string> strTests = new List<string>() { 
                    "", 
                    " ", 
                    "'asdas'",
                    "'as'das'",
                    "''asdas''",
                    "'asdas''",
            };
            System.Console.WriteLine("TESTING STRINGLEXER:");
            foreach (string t in strTests)
            {
                Lexer L = new StringLexer(t);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    System.Console.WriteLine("Test for '{0}':", t);
                    Console.ForegroundColor = ConsoleColor.White;
                    L.Parse();
                }
                catch (LexerException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            List<string> commentTests = new List<string>() { 
                    "", 
                    " ", 
                    "/*asdas*/",
                    "/*as*/das*/",
                    "a/*asdas*/",
            };
            System.Console.WriteLine("TESTING COMMENTLEXER:");
            foreach (string t in commentTests)
            {
                Lexer L = new CommentLexer(t);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    System.Console.WriteLine("Test for '{0}':", t);
                    Console.ForegroundColor = ConsoleColor.White;
                    L.Parse();
                }
                catch (LexerException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            List<string> idsTests = new List<string>() { 
                    "", 
                    " ", 
                    "Id1.Id2.Id3",
                    "Id_1.Id_2.Id_3",
                    "Id_1'.Id_2.Id_3",
                    "Id_1'.Id_2.Id_3.",
                    ".Id_1.Id_2.Id_3",
            };
            System.Console.WriteLine("TESTING IDSLEXER:");
            foreach (string t in idsTests)
            {
                Lexer L = new IdsLexer(t);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    System.Console.WriteLine("Test for '{0}':", t);
                    Console.ForegroundColor = ConsoleColor.White;
                    L.Parse();
                }
                catch (LexerException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine(e.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }
}

