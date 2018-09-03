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
    protected System.Text.StringBuilder res;
    public int parsedNumber;
    public IntLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
        res = new System.Text.StringBuilder();
    }


    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            if (currentCh == '-')
                res.Append('-');
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
            res.Append(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            res.Append(currentCh);
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Integer is recognized");
        
        bool result = int.TryParse(res.ToString(), out parsedNumber);

    }
}


public class IdentifierLexer : Lexer
{
    protected System.Text.StringBuilder intString;
    public IdentifierLexer(string input)
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

        while (char.IsDigit(currentCh) || char.IsLetter(currentCh))
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

public class NumberWithoutZeroLexer : Lexer
{
    protected System.Text.StringBuilder intString;
    public NumberWithoutZeroLexer(string input)
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

        if(currentCh == '0')
        {
            Error();
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

        System.Console.WriteLine("Number without zero is recognized");

    }
}

//public class NumbersAndLetteLexer : Lexer
//{
//    protected System.Text.StringBuilder intString;
//    public NumbersAndLetteLexer(string input)
//        : base(input)
//    {
//        intString = new System.Text.StringBuilder();
//    }


//    public override void Parse()
//    {
//        NextCh();
//        if (currentCh == '+' || currentCh == '-')
//        {
//            NextCh();
//        }

//        if (currentCh == '0')
//        {
//            Error();
//        }

//        if (char.IsDigit(currentCh))
//        {
//            NextCh();
//        }
//        else
//        {
//            Error();
//        }

//        while (char.IsDigit(currentCh))
//        {
//            NextCh();
//        }


//        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
//        {
//            Error();
//        }

//        System.Console.WriteLine("Number without zero is recognized");

//    }
//}

public class Program
{
    public static void Main()
    {
        string input = "1542161";
        IntLexer L = new IntLexer(input);

        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        if (L.parsedNumber == 1542161)
        {
            System.Console.WriteLine("Parsed successfully");
            System.Console.WriteLine(L.parsedNumber);
        }

        if (L.parsedNumber != -1542161)
        {
            System.Console.WriteLine("Parsed successfully");
            System.Console.WriteLine("It's not your number");
        }
        L = new IntLexer("0");

        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        if (L.parsedNumber == 0)
        {
            System.Console.WriteLine("Parsed successfully");
            System.Console.WriteLine(L.parsedNumber);
        }

        if (L.parsedNumber != -1542161)
        {
            System.Console.WriteLine("Parsed successfully");
            System.Console.WriteLine("It's not your number");
        }

        System.Console.WriteLine("----------");


        IdentifierLexer IL = new IdentifierLexer("a02020220ssss0");

        try
        {
            IL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        IL = new IdentifierLexer("");

        try
        {
            IL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        IL = new IdentifierLexer("02020220ssss0");

        try
        {
            IL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        IL = new IdentifierLexer("s");

        try
        {
            IL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        System.Console.WriteLine("----------");

        NumberWithoutZeroLexer NL = new NumberWithoutZeroLexer("12345");
        try
        {
            NL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        NL = new NumberWithoutZeroLexer("02345");
        try
        {
            NL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        NL = new NumberWithoutZeroLexer("0");
        try
        {
            NL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        NL = new NumberWithoutZeroLexer("");
        try
        {
            NL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        NL = new NumberWithoutZeroLexer("10");
        try
        {
            NL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("----------");

    }
}