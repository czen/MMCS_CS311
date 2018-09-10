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

public class IntLexer : Lexer //task 1
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


public class IdentifierLexer : Lexer //task 2
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

public class NumberWithoutZeroLexer : Lexer //task 3
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

public class NumbersAndLettersLexer : Lexer // task 4
{
    protected System.Text.StringBuilder intString;
    public NumbersAndLettersLexer(string input)
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

        var flag = true;

        while (char.IsDigit(currentCh) && flag || char.IsLetter(currentCh) && !flag)
        {
            NextCh();
            flag = !flag;
        }


        if (currentCharValue != -1) 
        {
            Error();
        }

        System.Console.WriteLine("Numbers and letters are recognized");

    }
}

public class StringWithSymbolsLexer : Lexer // task 5
{
    protected System.Text.StringBuilder intString;
    public StringWithSymbolsLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string res = "";
        NextCh();
        if (char.IsLetter(currentCh))
        {
            res += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        var flag = true;

        while (char.IsLetter(currentCh) && !flag || currentCh == ',' && flag || currentCh == ';' && flag)
        {
            res += currentCh;
            NextCh();
            flag = !flag;
        }
        if (res[res.Length - 1] == ',' || res[res.Length - 1] == ';')
            Error();

        if (currentCharValue != -1)
        {
            Error();
        }

        System.Console.WriteLine("String is recognized");
        System.Console.WriteLine("Your string = " + res);

    }
}
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


        NumbersAndLettersLexer NAL = new NumbersAndLettersLexer("a1b2b3n4");
        try
        {
            NAL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        NAL = new NumbersAndLettersLexer("a1b2b3n4a");
        try
        {
            NAL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        NAL = new NumbersAndLettersLexer("a1");
        try
        {
            NAL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        NAL = new NumbersAndLettersLexer("a");
        try
        {
            NAL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

                 NAL = new NumbersAndLettersLexer("12345");
        try
        {
            NAL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        NAL = new NumbersAndLettersLexer("a1b2b3n43");
        try
        {
            NAL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        NAL = new NumbersAndLettersLexer("");
        try
        {
            NAL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        System.Console.WriteLine("----------");

        StringWithSymbolsLexer SWL = new StringWithSymbolsLexer("q,w,f;q");
        try
        {
            SWL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        SWL = new StringWithSymbolsLexer("t,d,w;s");
        try
        {
            SWL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        SWL = new StringWithSymbolsLexer("t");
        try
        {
            SWL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        SWL = new StringWithSymbolsLexer("aw;g");
        try
        {
            SWL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        SWL = new StringWithSymbolsLexer("a,w;g,");
        try
        {
            SWL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        SWL = new StringWithSymbolsLexer("");
        try
        {
            SWL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
    }
}