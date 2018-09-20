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

public class IntAccumLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public IntAccumLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            intString.Append(currentCh);
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
            intString.Append(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            intString.Append(currentCh);
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        int result = int.Parse(intString.ToString());
        System.Console.WriteLine("output: " + result);

    }
}

public class IdLexer : Lexer
{

    public IdLexer(string input)
        : base(input)
    {}

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
        
        System.Console.WriteLine("Id is correct");
    }
}

public class StartNotZeroLexer : Lexer
{

    public StartNotZeroLexer(string input)
        : base(input)
    { }

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

        System.Console.WriteLine("Integer that doesn't start with zero is recognized");
    }
}

public class AlternateLexer : Lexer
{
    public char last;

    public AlternateLexer(string input)
        : base(input)
    { }

    public override void Parse()
    {
        NextCh();
        if (char.IsLetter(currentCh))
        {
            last = currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh) && char.IsLetter(last) || char.IsLetter(currentCh) && char.IsDigit(last))
        {
            last = currentCh;
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Alternate word is recognized!");
    }
}

public class LetterListLexer : Lexer
{
    public char last;
    public System.Collections.Generic.LinkedList<char> list;

    public LetterListLexer(string input)
        : base(input)
    {
        last = ',';
        list = new System.Collections.Generic.LinkedList<char>();
    }

    public override void Parse()
    {
        NextCh();

        while (char.IsLetter(currentCh) && (last == ',' || last == ';') || (currentCh == ',' || currentCh == ';') && char.IsLetter(last))
        {
            last = currentCh;
            if (char.IsLetter(currentCh))
            {
                list.AddLast(currentCh);
            }
            NextCh();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.Write("List of letters: ");
        foreach (char letter in list)
        {
            System.Console.Write(letter + " ");
        }
        System.Console.WriteLine();
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
        /*
        System.Console.WriteLine();
        System.Console.WriteLine("1 task's tests: ");
        IntAccumLexerTest();
        System.Console.WriteLine("------------------------------------");
        System.Console.WriteLine("2 task's tests: ");
        IdLexerTest();
        System.Console.WriteLine();
        System.Console.WriteLine("------------------------------------");
        System.Console.WriteLine("3 task's tests: ");
        StartNotZeroLexerTest();
        System.Console.WriteLine();
        System.Console.WriteLine("------------------------------------");
        System.Console.WriteLine("4 task's tests: ");
        AlternateLexerTest();
        System.Console.WriteLine();
        */
        System.Console.WriteLine("5 task's tests: ");
        LetterListLexerTest();
        System.Console.WriteLine();
    }

    public static void IntAccumLexerTest()
    {
        string[] inputs = { "", "-100", "-1", "0", "1", "100", "-0", "+5", "+0", "f", "++", "++1", "-", "+", "-a", "002" };
        foreach (string input in inputs)
        {
            System.Console.WriteLine("input: " + input);
            Lexer L = new IntAccumLexer(input);
            try
            {
                L.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }

    public static void IdLexerTest()
    {
        string[] inputs = { "", "a", "bb", "ccc", "d1", "e23", "f4g", "1", "0a" };
        foreach (string input in inputs)
        {
            System.Console.WriteLine("input: " + input);
            Lexer L = new IdLexer(input);
            try
            {
                L.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }

    public static void StartNotZeroLexerTest()
    {
        string[] inputs = { "", "-100", "-1", "0", "1", "100", "-0", "+5", "+0", "f", "++", "++1", "-", "+", "-a", "002", "005" };
        foreach (string input in inputs)
        {
            System.Console.WriteLine("input: " + input);
            Lexer L = new StartNotZeroLexer(input);
            try
            {
                L.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }

    public static void AlternateLexerTest()
    {
        string[] inputs = { "", "a", "a1b", "c2d4f5", "1a2b", "1", "15" };
        foreach (string input in inputs)
        {
            System.Console.WriteLine("input: " + input);
            Lexer L = new AlternateLexer(input);
            try
            {
                L.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }

    public static void LetterListLexerTest()
    {
        string[] inputs = { "", "a", "a;b", "12", "1;b", "b;1", "cc;", "a;b;c;d" };
        foreach (string input in inputs)
        {
            System.Console.WriteLine("input: " + input);
            Lexer L = new LetterListLexer(input);
            try
            {
                L.Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}