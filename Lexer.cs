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

// Задание 1
public class NewIntLexer : IntLexer
{
    string intNum = "";

    public NewIntLexer(string input)
        : base(input)
    {
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+')
            NextCh();
        else if (currentCh == '-')
        {
            intNum += '-';
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
            intNum += currentCh;
            NextCh();
        }
        else
            Error();

        while (char.IsDigit(currentCh))
        {
            intNum += currentCh;
            NextCh();
        }


        if (currentCharValue != -1)
            Error();

        System.Console.WriteLine("Integer is recognized: " + System.Int32.Parse(intNum));
    }
}


// Задание 2
public class IDLexer : Lexer
{
    public IDLexer(string input)
        : base(input)
    {
    }

    public override void Parse()
    {
        NextCh();

        if (char.IsLetter(currentCh))
            NextCh();
        else
            Error();

        while (char.IsLetterOrDigit(currentCh))
            NextCh();

        if (currentCharValue != -1)
            Error();

        System.Console.WriteLine("ID is recognized");
    }
}

// Задание 3
public class NewIntLexer2 : IntLexer
{
    public NewIntLexer2(string input)
        : base(input)
    {
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
            NextCh();

        if (char.IsDigit(currentCh) && currentCh != '0')
            NextCh();
        else
            Error();

        while (char.IsDigit(currentCh))
            NextCh();

        if (currentCharValue != -1)
            Error();

        System.Console.WriteLine("Integer is recognized");
    }
}

// Задание 4
public class LetterDigitLexer : Lexer
{
    public LetterDigitLexer(string input)
        : base(input)
    {
    }

    public override void Parse()
    {
        NextCh();

        if (char.IsLetter(currentCh))
            NextCh();
        else
            Error();

        while (true)
        {
            if (currentCharValue == -1)
            {
                System.Console.WriteLine("Sequence of digits and letters is recognized");
                break;
            }

            if (char.IsDigit(currentCh))
                NextCh();
            else
                Error();

            if (currentCharValue == -1)
            {
                System.Console.WriteLine("Sequence of digits and letters is recognized");
                break;
            }

            if (char.IsLetter(currentCh))
                NextCh();
            else
                Error();
        }
    }
}

// Задание 5
public class LettersLexer : Lexer
{
    string letters = "";

    public LettersLexer(string input)
        : base(input)
    {
    }

    public override void Parse()
    {
        NextCh();

        while (true)
        {
            if (char.IsLetter(currentCh))
            {
                letters += currentCh;
                NextCh();
            }
            else
                Error();

            if (currentCharValue == -1)
            {
                System.Console.WriteLine("List of letters is recognized: " + letters);
                break;
            }

            if (currentCh == ',' || currentCh == ';')
                NextCh();
            else
                Error();
        }
    }
}

public class Program
{
    public static void Main()
    {
        string input = "154216";
        Lexer L = new IntLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        //Задание 1
        System.Console.WriteLine("\nTask 1:");

        input = "0";
        Lexer L1 = new NewIntLexer(input);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "";
        L1 = new NewIntLexer(input);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "365";
        L1 = new NewIntLexer(input);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "-22";
        L1 = new NewIntLexer(input);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "4-2";
        L1 = new NewIntLexer(input);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        //Задание 2
        System.Console.WriteLine("\nTask 2:");

        input = "";
        Lexer L2 = new IDLexer(input);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "d";
        L2 = new IDLexer(input);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "da236E0";
        L2 = new IDLexer(input);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "2abcd";
        L2 = new IDLexer(input);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        //Задание 3
        System.Console.WriteLine("\nTask 3:");

        input = "";
        Lexer L3 = new NewIntLexer2(input);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "25";
        L3 = new NewIntLexer2(input);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "-73060";
        L3 = new NewIntLexer2(input);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "012";
        L3 = new NewIntLexer2(input);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        // Задание 4
        System.Console.WriteLine("\nTask 4:");

        input = "a";
        Lexer L4 = new LetterDigitLexer(input);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a1b2c3";
        L4 = new LetterDigitLexer(input);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "aa2b3c";
        L4 = new LetterDigitLexer(input);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "r4c2n";
        L4 = new LetterDigitLexer(input);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "0a9u5";
        L4 = new LetterDigitLexer(input);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // Задание 5
        System.Console.WriteLine("\nTask 5:");

        input = "c";
        Lexer L5 = new LettersLexer(input);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "c,a,b";
        L5 = new LettersLexer(input);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "c,a;b,5;k";
        L5 = new LettersLexer(input);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "ca,g,h";
        L5 = new LettersLexer(input);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a;b,c;d;e,";
        L5 = new LettersLexer(input);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "e,f,g;h";
        L5 = new LettersLexer(input);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
    }
}