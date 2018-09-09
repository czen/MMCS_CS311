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
    protected char currentCh;       // ��������� ��������� ������
    protected int currentCharValue; // ����� �������� ���������� ���������� �������
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


        if (currentCharValue != -1) // StringReader ������ -1 � ����� ������
        {
            Error();
        }

        System.Console.WriteLine("Integer is recognized");

    }
}

public class ToIntLexer : Lexer
{
    protected System.Text.StringBuilder ToIntString;
    public ToIntLexer(string input)
        : base(input)
    {
        ToIntString = new System.Text.StringBuilder();
    }
    public override void Parse()
    {
        string digit = "";
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            digit += currentCh;
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
            digit += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            digit += currentCh;
            NextCh();
        }


        if (currentCharValue != -1) // StringReader ������ -1 � ����� ������
        {
            Error();
        }

        int x = System.Convert.ToInt32(digit);
        System.Console.WriteLine("Integer " + x + " was recognized");

    }
}

public class IdLexer : Lexer
{

    protected System.Text.StringBuilder idString;

    public IdLexer(string input)
        : base(input)
    {
        idString = new System.Text.StringBuilder();
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


        if (currentCharValue != -1) // StringReader ������ -1 � ����� ������
        {
            Error();
        }

        System.Console.WriteLine("Identifier is recognized");

    }
}

public class SignIntLexer : Lexer
{

    protected System.Text.StringBuilder signIntString;

    public SignIntLexer(string input)
        : base(input)
    {
        signIntString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            NextCh();
        }
        else
        {
            Error();
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


        if (currentCharValue != -1) // StringReader ������ -1 � ����� ������
        {
            Error();
        }

        System.Console.WriteLine("Integer is recognized");

    }
}

public class CharDigitLexer : Lexer
{

    protected System.Text.StringBuilder CharDigitString;

    public CharDigitLexer(string input)
        : base(input)
    {
        CharDigitString = new System.Text.StringBuilder();
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

        bool fl = true;
        while ((char.IsDigit(currentCh) && fl) || (char.IsLetter(currentCh) && !fl))
        {
            NextCh();
            fl = !fl;
        }


        if (currentCharValue != -1) // StringReader ������ -1 � ����� ������
        {
            Error();
        }

        System.Console.WriteLine("CharDigit sequence is recognized");

    }
}

public class LetterSequenceLexer : Lexer
{

    protected System.Text.StringBuilder LetterSequenceString;

    public LetterSequenceLexer(string input)
        : base(input)
    {
        LetterSequenceString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string s = "";
        NextCh();
        if (char.IsLetter(currentCh))
        {
            s += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        bool fl = true;
        while (((currentCh == ',' || currentCh == ';') && fl) || (char.IsLetter(currentCh) && !fl))
        {
            if (!fl)
            {
                s += currentCh;
            }
            NextCh();
            fl = !fl;
        }


        if (currentCharValue != -1) // StringReader ������ -1 � ����� ������
        {
            Error();
        }

        System.Console.WriteLine("Letter sequence " + s +" is recognized");

    }
}

public class DigitSequenceLexer : Lexer
{

    protected System.Text.StringBuilder DigitSequenceString;

    public DigitSequenceLexer(string input)
        : base(input)
    {
        DigitSequenceString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string s = "";
        NextCh();
        if (char.IsDigit(currentCh))
        {
            s += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        bool fl = false;
        while (currentCh == ' ' || (char.IsDigit(currentCh) && fl))
        {
            if (currentCh != ' ')
            {
                s += currentCh;
                fl = false;
            }
            else
            {
                fl = true;
            }
            NextCh();
        }


        if (currentCharValue != -1) // StringReader ������ -1 � ����� ������
        {
            Error();
        }

        int x = System.Convert.ToInt32(s);
        System.Console.WriteLine("Digit sequence " + x + " is recognized");

    }
}

public class CharDigitGroupsLexer : Lexer
{

    protected System.Text.StringBuilder CharDigitGroupsString;

    public CharDigitGroupsLexer(string input)
        : base(input)
    {
        CharDigitGroupsString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        string s = "";
        if (char.IsLetter(currentCh))
        {
            s += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        int count_chars = 1;
        int count_digits = 0;
        while ((char.IsDigit(currentCh) && count_digits < 2) || (char.IsLetter(currentCh) && count_chars < 2))
        {
            if (char.IsLetter(currentCh))
            {
                s += currentCh;
                count_chars++;
                count_digits = 0;              
            }
            else if (char.IsDigit(currentCh))
            {
                s += currentCh;
                count_digits++;
                count_chars = 0;
            }
            NextCh();
        }


        if (currentCharValue != -1) // StringReader ������ -1 � ����� ������
        {
            Error();
        }

        System.Console.WriteLine("Sequence " + s + " is recognized");

    }
}

public class DoubleLexer : Lexer
{
    protected System.Text.StringBuilder DoubleString;

    public DoubleLexer(string input)
        : base(input)
    {
        DoubleString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (char.IsDigit(currentCh))
        {
            NextCh();
        }
        else
        {
            Error();
        }

        bool found_dot = false;
        bool digit_after_dot = false;
        while (char.IsDigit(currentCh) || (currentCh == '.' && !found_dot))
        {
            if (currentCh == '.')
            {
                found_dot = true;
            }
            if (char.IsDigit(currentCh) && found_dot)
            {
                digit_after_dot = true;
            }
            NextCh();
        }


        if (currentCharValue != -1 || !digit_after_dot) // StringReader ������ -1 � ����� ������
        {
            Error();
        }

        System.Console.WriteLine("Double value is recognized");

    }
}


public class Program
{
    private static void TestIntLexer()
    {
        System.Console.WriteLine("/----------------------------------------------/");
        System.Console.WriteLine("������������ IntLexer...");
        string s1 = "-1532";
        string s2 = "0";
        string s3 = "2+1";
        string s4 = "";
        string s5 = "+";

        Lexer L1 = new IntLexer(s1);
        Lexer L2 = new IntLexer(s2);
        Lexer L3 = new IntLexer(s3);
        Lexer L4 = new IntLexer(s4);
        Lexer L5 = new IntLexer(s5);

        System.Console.WriteLine("���� ��� ������ " + s1);
        try
        {
            L1.Parse();        
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s5);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        System.Console.WriteLine("/----------------------------------------------/\n\n");

    }

    private static void TestToIntLexer()
    {
        System.Console.WriteLine("/----------------------------------------------/");
        System.Console.WriteLine("������������ ToIntLexer...");

        string s1 = "+143232";
        string s2 = "0";
        string s3 = "-143232";
        string s4 = "2+0";
        string s5 = "";

        Lexer L1 = new ToIntLexer(s1);
        Lexer L2 = new ToIntLexer(s2);
        Lexer L3 = new ToIntLexer(s3);
        Lexer L4 = new ToIntLexer(s4);
        Lexer L5 = new ToIntLexer(s5);

        System.Console.WriteLine("���� ��� ������ " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s5);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        System.Console.WriteLine("/----------------------------------------------/\n\n");
    }

    private static void TestIdLexer()
    {
        System.Console.WriteLine("/----------------------------------------------/");
        System.Console.WriteLine("������������ IdLexer...");

        string s1 = "a121fqweqf214155";
        string s2 = "121fqweqf214155";
        string s3 = "+121fqweqf214155";
        string s4 = "";
        string s5 = "f";

        Lexer L1 = new IdLexer(s1);
        Lexer L2 = new IdLexer(s2);
        Lexer L3 = new IdLexer(s3);
        Lexer L4 = new IdLexer(s4);
        Lexer L5 = new IdLexer(s5);

        System.Console.WriteLine("���� ��� ������ " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s5);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        System.Console.WriteLine("/----------------------------------------------/\n\n");
    }

    private static void TestSignIntLexer()
    {
        System.Console.WriteLine("/----------------------------------------------/");
        System.Console.WriteLine("������������ SignIntLexer...");

        string s1 = "+12";
        string s2 = "-12";
        string s3 = "+01";
        string s4 = "-01";
        string s5 = "12";

        Lexer L1 = new SignIntLexer(s1);
        Lexer L2 = new SignIntLexer(s2);
        Lexer L3 = new SignIntLexer(s3);
        Lexer L4 = new SignIntLexer(s4);
        Lexer L5 = new SignIntLexer(s5);

        System.Console.WriteLine("���� ��� ������ " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s5);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        System.Console.WriteLine("/----------------------------------------------/\n\n");
    }

    private static void TestCharDigitLexer()
    {
        System.Console.WriteLine("/----------------------------------------------/");
        System.Console.WriteLine("������������ CharDigitLexer...");

        string s1 = "a1b2c3";
        string s2 = "1a2b3c";
        string s3 = "";
        string s4 = "aaaaa";
        string s5 = "+a1b2c3";

        Lexer L1 = new CharDigitLexer(s1);
        Lexer L2 = new CharDigitLexer(s2);
        Lexer L3 = new CharDigitLexer(s3);
        Lexer L4 = new CharDigitLexer(s4);
        Lexer L5 = new CharDigitLexer(s5);

        System.Console.WriteLine("���� ��� ������ " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s5);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        System.Console.WriteLine("/----------------------------------------------/\n\n");
    }

    private static void TestLetterSequenceLexer()
    {
        System.Console.WriteLine("/----------------------------------------------/");
        System.Console.WriteLine("������������ LetterSequenceLexer...");

        string s1 = "a;b;c,d,e;f,g;h";
        string s2 = ";a,b";
        string s3 = "";
        string s4 = "abcdef";
        string s5 = ",;";

        Lexer L1 = new LetterSequenceLexer(s1);
        Lexer L2 = new LetterSequenceLexer(s2);
        Lexer L3 = new LetterSequenceLexer(s3);
        Lexer L4 = new LetterSequenceLexer(s4);
        Lexer L5 = new LetterSequenceLexer(s5);

        System.Console.WriteLine("���� ��� ������ " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s5);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        System.Console.WriteLine("/----------------------------------------------/\n\n");
    }

    private static void TestDigitSequenceLexer()
    {
        System.Console.WriteLine("/----------------------------------------------/");
        System.Console.WriteLine("������������ DigitSequenceLexer...");

        string s1 = "1      3  4 5          6 7";
        string s2 = " 1 2 3    4";
        string s3 = "";
        string s4 = "123";
        string s5 = "1    3 6 70";

        Lexer L1 = new DigitSequenceLexer(s1);
        Lexer L2 = new DigitSequenceLexer(s2);
        Lexer L3 = new DigitSequenceLexer(s3);
        Lexer L4 = new DigitSequenceLexer(s4);
        Lexer L5 = new DigitSequenceLexer(s5);

        System.Console.WriteLine("���� ��� ������ " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s5);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        System.Console.WriteLine("/----------------------------------------------/\n\n");
    }

    private static void TestCharDigitGroupslexer()
    {
        System.Console.WriteLine("/----------------------------------------------/");
        System.Console.WriteLine("������������ CharDigitGroupsLexer...");

        string s1 = "a12bc3fe45";
        string s2 = "abc12f34";
        string s3 = "1ab23fe";
        string s4 = " af12ef45";
        string s5 = "";

        Lexer L1 = new CharDigitGroupsLexer(s1);
        Lexer L2 = new CharDigitGroupsLexer(s2);
        Lexer L3 = new CharDigitGroupsLexer(s3);
        Lexer L4 = new CharDigitGroupsLexer(s4);
        Lexer L5 = new CharDigitGroupsLexer(s5);

        System.Console.WriteLine("���� ��� ������ " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s5);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        System.Console.WriteLine("/----------------------------------------------/\n\n");
    }
    
    private static void TestDoubleLexer()
    {
        System.Console.WriteLine("/----------------------------------------------/");
        System.Console.WriteLine("������������ DoubleLexer...");

        string s1 = "123.456";
        string s2 = ".123";
        string s3 = "12345";
        string s4 = "12345.";
        string s5 = "1.2.3";

        Lexer L1 = new DoubleLexer(s1);
        Lexer L2 = new DoubleLexer(s2);
        Lexer L3 = new DoubleLexer(s3);
        Lexer L4 = new DoubleLexer(s4);
        Lexer L5 = new DoubleLexer(s5);

        System.Console.WriteLine("���� ��� ������ " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("���� ��� ������ " + s5);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        System.Console.WriteLine("/----------------------------------------------/\n\n");
    }
    public static void Main()
    {
        TestIntLexer();
        TestToIntLexer();
        TestIdLexer();
        TestSignIntLexer();
        TestCharDigitLexer();
        TestLetterSequenceLexer();
        TestDigitSequenceLexer();
        TestCharDigitGroupslexer();
        TestDoubleLexer();
    }
}