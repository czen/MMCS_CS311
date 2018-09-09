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
    protected string number = "";

    public IntLexer(string input)
        : base(input)
    {

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

        System.Console.WriteLine("{0} is recognized", int.Parse(number));
    }
}

public class IdLexer : Lexer
{
    public IdLexer(string input)
        : base(input) { }

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

        while (char.IsDigit(currentCh) || char.IsLetter(currentCh))
        {
            NextCh();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Identificator is recognized");
    }
}

public class IntNoLeadingZerosLexer : IntLexer
{
    public IntNoLeadingZerosLexer(string input)
        : base(input) { }

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
            if (currentCh == '0')
                Error();
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

        System.Console.WriteLine("{0} is recognized", int.Parse(number));
    }
}

public class AlternatingLettersDigitsLexer : Lexer
{
    public AlternatingLettersDigitsLexer(string input)
        : base(input) { }

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

        while (currentCharValue != -1)
        {
            if (char.IsDigit(currentCh))
                NextCh();
            else
            {
                Error();
            }
            if (char.IsDigit(currentCh))
                Error();
            if (char.IsLetter(currentCh))
                NextCh();
        }

        while (char.IsDigit(currentCh) || char.IsLetter(currentCh))
        {
            NextCh();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("String is recognized");
    }
}

public class DelimitedLettersLexer : Lexer
{
    private System.Collections.Generic.List<char> letters;

    public DelimitedLettersLexer(string input)
        : base(input) { }

    public override void Parse()
    {
        NextCh();
        if (char.IsLetter(currentCh))
        {
            letters = new System.Collections.Generic.List<char>();
            letters.Add(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        while (currentCharValue != -1)
        {
            if (currentCh == ',' || currentCh == ';')
                NextCh();
            else
            {
                Error();
            }
            if (char.IsLetter(currentCh))
            {
                letters.Add(currentCh);
                NextCh();
            }
            else
            {
                Error();
            }
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.Write("List of letters: ");
        foreach (char l in letters)
            System.Console.Write("{0} ", l);
        System.Console.WriteLine();
    }
}

public class Program
{
    public static void IntLexerTest()
    {
        System.Console.WriteLine("IntLexerTest");
        string[] input = { "", "-0", "-0+1", "+001", "154216" };
        foreach (var s in input)
        {
            try
            {
                new IntLexer(s).Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
        System.Console.WriteLine();
    }

    public static void IdLexerTest()
    {
        System.Console.WriteLine("IdLexerTest");
        string[] input = { "", "b", "1b", "fa64d11", "a1111111" };
        foreach (var s in input)
        {
            try
            {
                new IdLexer(s).Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
        System.Console.WriteLine();
    }

    public static void IntLexerNoLeadingZerosTest()
    {
        System.Console.WriteLine("IntNoLeadingZerosLexerTest");
        string[] input = { "", "-0", "001", "-1000", "154216" };
        foreach (var s in input)
        {
            try
            {
                new IntNoLeadingZerosLexer(s).Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
        System.Console.WriteLine();
    }

    public static void AlternatingLettersDigitsLexerTest()
    {
        System.Console.WriteLine("AlternatingLettersDigitsLexerTest");
        string[] input = { "", "m&1", "j1)", "i93", "1b", "bb1", "b1", "a1b2b4m" };
        foreach (var s in input)
        {
            try
            {
                new AlternatingLettersDigitsLexer(s).Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
        System.Console.WriteLine();
    }

    public static void DelimitedLettersLexerTest()
    {
        System.Console.WriteLine("DelimitedLettersLexerTest");
        string[] input = { "", ",a;b", "a,b;", "-a,n,b", "A,n;Q,R" };
        foreach (var s in input)
        {
            try
            {
                new DelimitedLettersLexer(s).Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
        System.Console.WriteLine();
    }

    public static void Main()
    {
        IntLexerTest();
        IdLexerTest();
        IntLexerNoLeadingZerosTest();
        AlternatingLettersDigitsLexerTest();
        DelimitedLettersLexerTest();
    }
}