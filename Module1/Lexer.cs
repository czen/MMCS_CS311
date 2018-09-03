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


//TASK1
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
        int number = 0;
        bool isNegative = false;
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            isNegative = currentCh == '-';
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
            number = int.Parse(currentCh.ToString());
            System.Console.WriteLine("Number is: {0}", number);
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {

            number = number*10 + int.Parse(currentCh.ToString());
            System.Console.WriteLine("Number is: {0}", number);
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }
        if (isNegative)
            number *= -1;

        System.Console.WriteLine("Integer is recognized");
        System.Console.WriteLine("Your integer is:{0}", number);
    }
}


//TASK2
public class IdentLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public IdentLexer(string input)
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

        while (char.IsDigit(currentCh) || char.IsLetter(currentCh) || currentCh == '_')
            NextCh();

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Identificator is recognized");
    }
}


//TASK3
public class IntNotZeroLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public IntNotZeroLexer(string input)
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

        if (currentCh != '0' && char.IsDigit(currentCh))
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

//Task 4
public class LetterDigitInterleave: Lexer
{

    protected System.Text.StringBuilder intString;

    public LetterDigitInterleave(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        bool nextDigit = false;
        NextCh();
        if (char.IsLetter(currentCh))
        {
            NextCh();
            nextDigit = true;
        }
        else
        {
            Error();
        }

        while (nextDigit && char.IsDigit(currentCh) || !nextDigit && char.IsLetter(currentCh))
        {
            nextDigit = !nextDigit;
            NextCh();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Identificator is recognized");
    }
}

//Task 5
public class LettersSeparated : Lexer
{

    protected System.Text.StringBuilder intString;

    public LettersSeparated(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string chars = "";
        bool lastReadSeparator = false;
        NextCh();
        if (char.IsLetter(currentCh))
        {
            chars += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        while (currentCharValue != -1)
        {
                if ((currentCh == ';' || currentCh == ',') && !lastReadSeparator)
                {
                    lastReadSeparator = true;
                    NextCh();
                }
                else if (char.IsLetter(currentCh))
                {
                    lastReadSeparator = false;
                    while (char.IsLetter(currentCh))
                    {
                        chars += currentCh;
                        NextCh();
                    }
                }
                else
                    Error();
        }

        if (lastReadSeparator)
        {
            Error();
        }

        System.Console.WriteLine("Identificator is recognized");
        System.Console.WriteLine("Chars: {0}", chars);
    }
}
public class Program
{
    public static void Main()
    {
        //ints
        string intPos = "154216";
        string intNeg = "-154216";
        Lexer L1 = new IntLexer(intPos);
        Lexer L3 = new IntLexer("12x3");
        Lexer L2 = new IntLexer(intNeg);
        System.Console.WriteLine("task 1:");
        try
        {
            L1.Parse();
            L2.Parse();
            L3.Parse();
            
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        //idents
        string goodIdent = "abc_23d";
        string badIdent = "123a";
        Lexer LId1 = new IdentLexer(goodIdent);
        Lexer LId2 = new IdentLexer(badIdent);
        System.Console.WriteLine("task 2:");
        try
        {
            LId1.Parse();
            LId2.Parse();

        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        //not zeros
        string goodInt = "154216";
        string badInt = "054216";
        Lexer NotZeroL1 = new IntNotZeroLexer(goodInt);
        Lexer NotZeroL2 = new IntNotZeroLexer(badInt);
        System.Console.WriteLine("task 3:");
        try
        {
            NotZeroL1.Parse();
            NotZeroL2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        //interleaved
        Lexer Interleave1 = new LetterDigitInterleave("a1b2c3");
        Lexer Interleave2 = new LetterDigitInterleave("a1b2c33");
        Lexer Interleave3 = new LetterDigitInterleave("a1b2c");
        System.Console.WriteLine("task 4:");
        try
        {
            Interleave1.Parse();
            Interleave3.Parse();
            Interleave2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        //letters separated
        Lexer Separated1 = new LettersSeparated("a,b;c");
        Lexer Separated2 = new LettersSeparated("abc");
        Lexer Separated3 = new LettersSeparated("a;bc,");
        System.Console.WriteLine("task 4:");
        try
        {
            Separated1.Parse();
            Separated2.Parse();
            Separated3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
    }
}