using System.Collections.Generic;
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
        o.Append(inputString + '\n');
        o.Append(new System.String(' ', position - 1) + "^\n");
        o.AppendFormat("Error in symbol {0}", currentCh);
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
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            NextCh();
        }

        if (char.IsDigit(currentCh))
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

        System.Console.WriteLine("Integer is recognized");

    }
}


//task1
public class IntLexerSave : Lexer
{

    protected System.Text.StringBuilder intString;
    protected string number;
    public int intnumber;
    public IntLexerSave(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            number += currentCh;
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
            number += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            number += currentCh;
            NextCh();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        intnumber = int.Parse(number);
        System.Console.WriteLine("Integer is recognized: " + intnumber.ToString());
    }
}

//task2
public class IdLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public IdLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
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

        while (char.IsLetterOrDigit(currentCh))
        {
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Identifier is recognized");
    }
}

//task3
public class CorrectIntLexer : Lexer
{

    protected System.Text.StringBuilder intString;
    public CorrectIntLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            NextCh();
        }

        if (char.IsDigit(currentCh) && currentCh != '0')
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
        System.Console.WriteLine("Correct integer is recognized");
    }
}

//task4
public class AltLetDigLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public AltLetDigLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
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

        int dig = 1;
        while (char.IsLetterOrDigit(currentCh))
        {
            if (char.IsLetter(currentCh) && dig == 0)
            {
                NextCh();
                dig = 1;
            }
            else if (char.IsDigit(currentCh) && dig == 1)
            {
                NextCh();
                dig = 0;
            }
            else Error();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Alternate letters and digits is recognized");
    }
}

//task5
public class LettersLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public LettersLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        List<char> letters = new List<char>(); 
        NextCh();

        if (char.IsLetter(currentCh))
        {
            letters.Add(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        int delim = 1;

        while (char.IsLetter(currentCh) || currentCh == ';' || currentCh == ',')
        {
            if (char.IsLetter(currentCh) && delim == 0)
            {
                letters.Add(currentCh);
                NextCh();
                delim = 1;
            }
            else if ((currentCh == ';' || currentCh == ',') && delim == 1)
            {
                NextCh();
                delim = 0;
            }
            else Error();
        }

        if (delim == 0)
            Error();

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Letters list is recognized, letters are: ");
        letters.ForEach(System.Console.Write);
        System.Console.WriteLine();
    }
}

//extra tasks
//etask1
public class DigitsLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public DigitsLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        List<char> digits = new List<char>();
        NextCh();

        if (char.IsDigit(currentCh))
        {
            digits.Add(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        int delim = 1;

        while (char.IsDigit(currentCh) || currentCh == ' ')
        {
            if (char.IsDigit(currentCh) && delim == 0)
            {
                digits.Add(currentCh);
                NextCh();
                delim = 1;
            }
            else if (currentCh == ' ')
            {
                NextCh();
                delim = 0;
            }
            else Error();
        }

        if (delim == 0)
            Error();

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("digits list is recognized, digits are: ");
        digits.ForEach(System.Console.Write);
        System.Console.WriteLine();
    }
}


//etask2  aa12c23dd1
public class Lexem1Lexer : Lexer
{

    protected System.Text.StringBuilder intString;
    public string result = "";
    public Lexem1Lexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();

        if (char.IsLetter(currentCh))
        {
            result += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        bool prevLetter = true;
        int cnt = 1;

        while (char.IsLetterOrDigit(currentCh))
        {
           if (char.IsLetter(currentCh)) //letter
            {
                if (prevLetter)
                {
                    ++cnt;
                    if (cnt == 3)
                        Error();
                    result += currentCh;
                }
                else
                {
                    cnt = 1;
                    prevLetter = true;
                    result += currentCh;
                }
            }
            else                               //digit
            {
                if (!prevLetter)
                {
                    ++cnt;
                    if (cnt == 3)
                        Error();
                    result += currentCh;
                }
                else
                {
                    cnt = 1;
                    prevLetter = false;
                    result += currentCh;
                }
            }
            NextCh();
        }
        

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Lexem1 recognized: " + result);
    }
}

//etask3 123.45678
public class RealLexer : Lexer
{

    protected System.Text.StringBuilder intString;
    public RealLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();

        if (currentCh == '-')
        {
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
            NextCh();
        }
        else
        {
            Error();
        }

        bool dot = false;
        bool lastdot = false;
        while (char.IsDigit(currentCh) || currentCh == '.')
        {
           if (currentCh == '.')
            {
                if (dot)
                    Error();
                dot = true;
                lastdot = true;
            }
            else
            {
                if (lastdot)
                    lastdot = false;
            }
            NextCh();
        }

        if (lastdot)
            Error();

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Real number recognized");
    }
}

//etask4 'string'
public class Lexem2Lexer : Lexer
{

    protected System.Text.StringBuilder intString;
    public Lexem2Lexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();

        if (currentCh == '\'')
        {
            NextCh();
        }
        else
        {
            Error();
        }

        while (currentCharValue != -1 && currentCh != '\'')
        {
            NextCh();
        }

        if (currentCh == '\'')
        {
            NextCh();
        }
        else
        {
            Error();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("String lexem recognized");
    }
}

//etask5 /*comment*/
public class Lexem3Lexer : Lexer
{

    protected System.Text.StringBuilder intString;
    public Lexem3Lexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();

        if (currentCh == '/')
        {
            NextCh();
        }
        else
        {
            Error();
        }
        if (currentCh == '*')
        {
            NextCh();
        }
        else
        {
            Error();
        }

        bool prevstar = false;

        while (!(prevstar && currentCh == '/'))
        {
            if (currentCharValue == -1) // StringReader вернет -1 в конце строки
            {
                Error();
            }
            if (currentCh == '*')
                prevstar = true;
            else
                prevstar = false;
            NextCh();
        }

        if (prevstar && currentCh == '/')
            NextCh();
        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Comment lexem recognized");
    }
}

//extra extra task Id1.Id2.Id3
public class IdsLexer : Lexer
{

    protected System.Text.StringBuilder intString;
    public IdsLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }
    protected void ParseId()
    {
        if (char.IsLetter(currentCh))
        {
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsLetterOrDigit(currentCh))
        {
            NextCh();
        }
    }

    public override void Parse()
    {
        NextCh();
        while (true)
        {
            ParseId();
            if (currentCharValue == -1)
                break;
            if (currentCh != '.')
                Error();
            NextCh();
        }

        /*if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }*/

        System.Console.WriteLine("ids lexem recognized");
    }
}

public class Program
{
    public static void test1()
    {
        System.Console.WriteLine("tests for task1");
        string[] input = { "", "12a5", "+1237", "-345", "4+5" };


        for (int i = 0; i < input.Length; ++i)
        {
            System.Console.WriteLine("      #{0}", i);
            System.Console.WriteLine("Input line: '{0}'", input[i]);
            IntLexerSave ILS = new IntLexerSave(input[i]);
            try
            {
                ILS.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
    public static void test2()
    {
        System.Console.WriteLine("tests for task2");
        string[] input = { "", "a1Abc25", "+1237", "45abCd", "a*d" };


        for (int i = 0; i < input.Length; ++i)
        {
            System.Console.WriteLine("      #{0}", i);
            System.Console.WriteLine("Input line: '{0}'", input[i]);
            IdLexer ILS = new IdLexer(input[i]);
            try
            {
                ILS.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }

    public static void test3()
    {
        System.Console.WriteLine("tests for task3");
        string[] input = { "", "-2", "9000", "0003" };

        for (int i = 0; i < input.Length; ++i)
        {
            System.Console.WriteLine("      #{0}", i);
            System.Console.WriteLine("Input line: '{0}'", input[i]);
            CorrectIntLexer ILS = new CorrectIntLexer(input[i]);
            try
            {
                ILS.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }

    public static void test4()
    {
        System.Console.WriteLine("tests for task4");
        string[] input = { "", "2avavS", "a3b5v5v", "aa" };

        for (int i = 0; i < input.Length; ++i)
        {
            System.Console.WriteLine("      #{0}", i);
            System.Console.WriteLine("Input line: '{0}'", input[i]);
            AltLetDigLexer ILS = new AltLetDigLexer(input[i]);
            try
            {
                ILS.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
    public static void test5()
    {
        System.Console.WriteLine("tests for task5");
        string[] input = { "", "a,b,c,d", "1;c;b;d", "f;A;d;G" };

        for (int i = 0; i < input.Length; ++i)
        {
            System.Console.WriteLine("      #{0}", i);
            System.Console.WriteLine("Input line: '{0}'", input[i]);
            LettersLexer ILS = new LettersLexer(input[i]);
            try
            {
                ILS.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
    public static void etest1()
    {
        System.Console.WriteLine("tests for extra task1");
        string[] input = { "", "1 2   4 5", "  ", "2 3 b 2" };

        for (int i = 0; i < input.Length; ++i)
        {
            System.Console.WriteLine("      #{0}", i);
            System.Console.WriteLine("Input line: '{0}'", input[i]);
            DigitsLexer ILS = new DigitsLexer(input[i]);
            try
            {
                ILS.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
    public static void etest2()
    {
        System.Console.WriteLine("tests for extra task2");
        string[] input = { "", "ab12cd", "1bv2", "b23cd1f2f3" };

        for (int i = 0; i < input.Length; ++i)
        {
            System.Console.WriteLine("      #{0}", i);
            System.Console.WriteLine("Input line: '{0}'", input[i]);
            Lexem1Lexer ILS = new Lexem1Lexer(input[i]);
            try
            {
                ILS.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
    public static void etest3()
    {
        System.Console.WriteLine("tests for extra task3");
        string[] input = { "", ".123", "123.", "123.344" };

        for (int i = 0; i < input.Length; ++i)
        {
            System.Console.WriteLine("      #{0}", i);
            System.Console.WriteLine("Input line: '{0}'", input[i]);
            RealLexer ILS = new RealLexer(input[i]);
            try
            {
                ILS.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
    public static void etest4()
    {
        System.Console.WriteLine("tests for extra task4");
        string[] input = { "", "'string'", "not'string", "'string'string" };

        for (int i = 0; i < input.Length; ++i)
        {
            System.Console.WriteLine("      #{0}", i);
            System.Console.WriteLine("Input line: {0}", input[i]);
            Lexem2Lexer ILS = new Lexem2Lexer(input[i]);
            try
            {
                ILS.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
    public static void etest5()
    {
        System.Console.WriteLine("tests for extra task5");
        string[] input = { "", "/*comment*/", "not/*comment", "/*wrong", "/*and*/wrong*/" };

        for (int i = 0; i < input.Length; ++i)
        {
            System.Console.WriteLine("      #{0}", i);
            System.Console.WriteLine("Input line: {0}", input[i]);
            Lexem3Lexer ILS = new Lexem3Lexer(input[i]);
            try
            {
                ILS.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
    public static void extraextratest()
    {
        System.Console.WriteLine("tests for extra extra task");
        string[] input = { "", "Id123", "Id123.Id2.ab34bd", "Id1."};

        for (int i = 0; i < input.Length; ++i)
        {
            System.Console.WriteLine("      #{0}", i);
            System.Console.WriteLine("Input line: {0}", input[i]);
            IdsLexer ILS = new IdsLexer(input[i]);
            try
            {
                ILS.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
    public static void Main()
    {
        /*test1();
        System.Console.WriteLine("------------------------------");
        test2();
        System.Console.WriteLine("------------------------------");
        test3();
        System.Console.WriteLine("------------------------------");
        test4();
        System.Console.WriteLine("------------------------------");
        test5();
        System.Console.WriteLine("------------------------------");
        etest1();
        System.Console.WriteLine("------------------------------");
        etest2();
        System.Console.WriteLine("------------------------------");
        etest3();
        System.Console.WriteLine("------------------------------");
        etest4();
        System.Console.WriteLine("------------------------------");
        etest5();
        System.Console.WriteLine("------------------------------");*/
        extraextratest();
        System.Console.WriteLine("------------------------------");

    }
}