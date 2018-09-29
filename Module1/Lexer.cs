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


public class NumbersWithSpaces : Lexer // extra task 1
{
    protected System.Text.StringBuilder intString;
    public NumbersWithSpaces(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string numbers = "";
        bool prev = false;

        NextCh();
        if (char.IsDigit(currentCh))
        {
            numbers += currentCh;
            NextCh();
            prev = true;
        }
        else
            Error();

        while (currentCharValue != -1)
            if (prev)
            {
                while (currentCh == ' ')
                    NextCh();
                prev = false;
            }
            else
            {
                if (!char.IsDigit(currentCh))
                    Error();
                prev = true;
                numbers += currentCh;
                NextCh();
            }

        if (!prev)
            Error();

        System.Console.WriteLine("String is recognized");
        System.Console.WriteLine("Your numbers = " + numbers);

    }
}

public class NumbersWordsLexer : Lexer //extra task2
{

    protected System.Text.StringBuilder intString;

    public NumbersWordsLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string res = "";
        int numbers = 0, words = 0;

        if (char.IsDigit(currentCh))
            numbers++;
        else if (char.IsLetter(currentCh))
            words++;

        res += currentCh;
        NextCh();

        while (currentCharValue != -1)
        {
            if (char.IsDigit(currentCh))
            {
                if (numbers < 2)
                {
                    numbers++;
                    words = 0;
                }
                else
                {
                    Error();
                }
            }
            else if (char.IsLetter(currentCh))
            {
                if (words < 2)
                {
                    words++;
                    numbers = 0;
                }
                else
                {
                    Error();
                }
            }

            res += currentCh;
            NextCh();
        }

        System.Console.WriteLine("String is recognized");
        System.Console.WriteLine("Your string = " + res);
    }
}

public class FloatNumbersLexer : Lexer //extra task 3
{

    protected System.Text.StringBuilder intString;

    public FloatNumbersLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        bool dot = false;
        bool after = false;
        bool end = false;
        string res = "";

        if (currentCh == '-')
            res += "-";
        NextCh();

        while (currentCharValue != -1)
        {
            if (currentCh == '.')
            {
                if (dot || !after)
                    Error();
                dot = true;
                res += '.';
                NextCh();
                end = true;
            }
            else if (char.IsDigit(currentCh))
            {
                after = true;
                res += currentCh;
                NextCh();
                end = false;
            }
            else
                Error();
        }
        if (!dot || end)
            Error();

        System.Console.WriteLine("String is recognized");
        System.Console.WriteLine("Your numbers = " + res);
    }
}

public class WithoutApLexer : Lexer //extra task 4
{

    protected System.Text.StringBuilder intString;

    public WithoutApLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        char ap = '\'';

        NextCh();
        if (currentCh == ap)
            NextCh();
        else
            Error();

        while (currentCharValue != -1)
        {
            if (currentCh != ap)
                NextCh();
            else
            {
                NextCh();
                break;
            }
        }

        if (currentCharValue != -1)
            Error();

        System.Console.WriteLine("String is recognized");
    }
}

public class CommLexer : Lexer //extra task 5
{

    protected System.Text.StringBuilder intString;

    public CommLexer(string input)
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
            if (currentCh == '*')
                NextCh();
            else
                Error();
        }
        else
            Error();

        while (currentCharValue != -1)
        {
            if (currentCh == '*' )
            {
                NextCh();
                if (currentCh == '/')
                {
                    NextCh();
                    break;
                }
            }
            NextCh();
        }

        if (currentCharValue != -1)
            Error();

        System.Console.WriteLine("String is recognized");
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

        System.Console.WriteLine("----------");

        NumbersWithSpaces NWS = new NumbersWithSpaces("1 2 33 4");
        try
        {
             NWS.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("----------");

        NumbersWordsLexer NWL = new NumbersWordsLexer("11aa2b33b4");
        try
        {
            NWL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("----------");

        FloatNumbersLexer FNL = new FloatNumbersLexer("1.");
        try
        {
            FNL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("----------");
        FNL = new FloatNumbersLexer("1.2");
        try
        {
            FNL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("----------");
        FNL = new FloatNumbersLexer(".1");
        try
        {
            FNL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("----------");
        FNL = new FloatNumbersLexer(".");
        try
        {
            FNL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("----------");
        

        WithoutApLexer WAPL = new WithoutApLexer("\'sdsds2dssdsd\'");
        try
        {
            WAPL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("----------");

        WAPL = new WithoutApLexer("\'sdsds2\'dssdsd\'");
        try
        {
            WAPL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("----------");
        CommLexer CL = new CommLexer("/* dsddssd */");
        try
        {
            CL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("----------");

        CL = new CommLexer("/* dsdds*sd */");
        try
        {
            CL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("----------");

        CL = new CommLexer("/* dsdds*/ sd */");
        try
        {
            CL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("----------");

    }
}