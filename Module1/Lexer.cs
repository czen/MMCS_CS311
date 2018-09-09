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

// Task 1
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

// Task 2
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

        while (char.IsLetterOrDigit(currentCh))
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

// Task 3
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

// Task 4
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

// Task 5
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
            letters = new System.Collections.Generic.List<char> { currentCh };
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

        System.Console.Write("List of letters: ");
        foreach (char l in letters)
            System.Console.Write("{0} ", l);
        System.Console.WriteLine();
    }
}

// Extra task 1
public class DelimitedDigitsLexer : Lexer
{
    private System.Collections.Generic.List<char> digits;

    public DelimitedDigitsLexer(string input) : base(input)
    {
    }

    public override void Parse()
    {
        NextCh();
        if (char.IsDigit(currentCh))
        {
            digits = new System.Collections.Generic.List<char>() { currentCh };
            NextCh();
        }
        else
        {
            Error();
        }

        // to not recognise "11 1" string
        if (char.IsDigit(currentCh))
            Error();

        while (currentCharValue != -1)
        {
            while (currentCh == ' ')
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
        }

        System.Console.Write("List of digits: ");
        foreach (char l in digits)
            System.Console.Write("{0} ", l);
        System.Console.WriteLine();
    }
}

// Extra task 2
public class LettersDigitsLexer : Lexer
{
    private string lexeme;
    private int count;
    private bool isPrevDigit = false;

    public LettersDigitsLexer(string input) : base(input)
    {
    }

    public override void Parse()
    {
        NextCh();

        if (char.IsLetterOrDigit(currentCh))
        {
            count = 1;
            if (char.IsDigit(currentCh)) isPrevDigit = true;
            lexeme += currentCh;
            NextCh();
        }
        else
            Error();

        while (currentCharValue != -1)
        {
            if (char.IsDigit(currentCh))
            {
                if (!isPrevDigit)
                {
                    count = 1;
                    isPrevDigit = true;
                }
                else
                    ++count;
                if (count > 2)
                    Error();
                lexeme += currentCh;
                NextCh();
            }
            else if (!char.IsLetterOrDigit(currentCh))
                Error();

            if (char.IsLetter(currentCh))
            {
                if (isPrevDigit)
                {
                    count = 1;
                    isPrevDigit = false;
                }
                else
                    ++count;
                if (count > 2)
                    Error();
                lexeme += currentCh;
                NextCh();
            }
        }

        System.Console.Write("Lexeme: {0}", lexeme);
        System.Console.WriteLine();
    }
}

// Extra task 3
public class DoubleLexer : Lexer
{
    public DoubleLexer(string input) : base(input)
    {
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

        if (currentCh == '.')
        {
            NextCh();
            if (currentCharValue == -1)
                Error();
        }

        while (char.IsDigit(currentCh))
            NextCh();

        if (currentCharValue != -1)
        {
            Error();
        }

        System.Console.WriteLine("Double is recognised");
    }
}

// Extra task 4
public class StringLexer : Lexer
{
    public StringLexer(string input) : base(input)
    {
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '\'')
            NextCh();
        else
            Error();

        while (currentCh != '\'')
        {
            if (currentCharValue == -1)
                Error();
            else
                NextCh();
        }
        NextCh();
        if (currentCharValue != -1)
            Error();

        System.Console.WriteLine("String is recognised");
    }
}

// Extra task 5
public class CommentLexer : Lexer
{
    public CommentLexer(string input) : base(input)
    {
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

        while (true)
        {
            while (currentCh != '*')
            {
                if (currentCharValue == -1)
                    Error();
                else
                    NextCh();
            }
            NextCh();
            if (currentCharValue == -1)
                Error();
            if (currentCh == '/')
            {
                NextCh();
                if (currentCharValue != -1)
                    Error();
                else
                    break;
            }
        }

        System.Console.WriteLine("Comment is recognised");
    }
}

// Extra hard task
public class IdsLexer : Lexer
{
    public IdsLexer(string input) : base(input)
    {
    }

    public override void Parse()
    {
        NextCh();

        while (true)
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

            if (currentCharValue == -1)
                break;

            if (currentCh == '.')
            {
                NextCh();
            }
        }

        System.Console.WriteLine("Identificators are recognized");
    }
}

public class Program
{
    public static void IntLexerTest()
    {
        System.Console.WriteLine("------------------------------IntLexerTest------------------------------");
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
        System.Console.WriteLine("------------------------------IdLexerTest------------------------------");
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

    public static void IntNoLeadingZerosLexerTest()
    {
        System.Console.WriteLine("------------------------------IntNoLeadingZerosLexerTest------------------------------");
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
        System.Console.WriteLine("------------------------------AlternatingLettersDigitsLexerTest------------------------------");
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
        System.Console.WriteLine("------------------------------DelimitedLettersLexerTest------------------------------");
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

    public static void DelimitedDigitsLexerTest()
    {
        System.Console.WriteLine("------------------------------DelimitedDigitsLexerTest------------------------------");
        string[] input = { "", " 1", "11 0", "1 ", "_ 1", "1   2 3 4" };
        foreach (var s in input)
        {
            try
            {
                new DelimitedDigitsLexer(s).Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
        System.Console.WriteLine();
    }

    public static void LettersDigitsLexerTest()
    {
        System.Console.WriteLine("------------------------------LettersDigitsLexerTest------------------------------");
        string[] input = { "", "aa12c23dd1", "111a", "1a1aaa1" };
        foreach (var s in input)
        {
            try
            {
                new LettersDigitsLexer(s).Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
        System.Console.WriteLine();
    }

    public static void DoubleLexerTest()
    {
        System.Console.WriteLine("------------------------------DoubleLexerTest------------------------------");
        string[] input = { "", "-00.22", "0.1.3", "+3.65", "0.", ".11" };
        foreach (var s in input)
        {
            try
            {
                new DoubleLexer(s).Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
        System.Console.WriteLine();
    }

    public static void StringLexerTest()
    {
        System.Console.WriteLine("------------------------------StringLexerTest------------------------------");
        string[] input = { "", "'", "'str'd'", "''", "'string'" };
        foreach (var s in input)
        {
            try
            {
                new StringLexer(s).Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
        System.Console.WriteLine();
    }

    public static void CommentLexerTest()
    {
        System.Console.WriteLine("------------------------------CommentLexerTest------------------------------");
        string[] input = { "", "/*", "/**", "/*ff*fff*/f", "/**/", "/*c*omment*comment*/" };
        foreach (var s in input)
        {
            try
            {
                new CommentLexer(s).Parse();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
        System.Console.WriteLine();
    }

    public static void IdsLexerTest()
    {
        System.Console.WriteLine("------------------------------IdsLexerTest------------------------------");
        string[] input = { "", ".", ".b12v", "b1.", "fa64d11.a2726b.", "a1111111.b7jjjj.aaakka", "b" };
        foreach (var s in input)
        {
            try
            {
                new IdsLexer(s).Parse();
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
        IntNoLeadingZerosLexerTest();
        AlternatingLettersDigitsLexerTest();
        DelimitedLettersLexerTest();

        DelimitedDigitsLexerTest();
        LettersDigitsLexerTest();
        DoubleLexerTest();
        StringLexerTest();
        CommentLexerTest();

        IdsLexerTest();
    }
}