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

public class IdLexer : Lexer
{
    protected System.Text.StringBuilder idString;

    public IdLexer(string input) : base(input)
    {
        idString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (char.IsLetter(currentCh))
            NextCh();
        else
            Error();

        while (char.IsLetter(currentCh) || char.IsDigit(currentCh) || currentCh == '_')
            NextCh();

        if (currentCharValue != -1)
            Error();

        System.Console.WriteLine("Id id recognized");
    }
}

public class IntLexer1 : Lexer
{
    protected System.Text.StringBuilder intString;

    public IntLexer1(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
            NextCh();

        if (currentCh != '0' && char.IsDigit(currentCh))
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

public class IntChLexer : Lexer
{
    protected System.Text.StringBuilder intString;

    public IntChLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (char.IsLetter(currentCh))
            NextCh();
        else
            Error();

        while (char.IsDigit(currentCh) || char.IsLetter(currentCh))
            NextCh();

        if (currentCharValue != -1)
            Error();

        System.Console.WriteLine("Digits and chars are recognized");
    }
}


public class Program
{
    public static void Main()
    {
        string input = "wtr_56";
        Lexer L = new IdLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input1 = "_666_";
        Lexer L1 = new IdLexer(input1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }



        string input2 = "0746";
        Lexer L2 = new IntLexer1(input2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input3 = "-123";
        Lexer L3 = new IntLexer1(input3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input4 = "3aaa";
        Lexer L4 = new IntChLexer(input4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input5 = "a4b7c";
        Lexer L5 = new IntChLexer(input5);
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