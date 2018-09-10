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


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
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


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
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


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
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


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
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


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
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


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
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


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
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


        if (currentCharValue != -1 || !digit_after_dot) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Double value is recognized");

    }
}

public class TextLexer : Lexer
{
    protected System.Text.StringBuilder TextString;

    public TextLexer(string input)
        : base(input)
    {
        TextString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '\'')
        {
            NextCh();
        }
        else
        {
            Error();
        }

        bool close = false;
        bool quote_inside = false;
        while (currentCharValue != -1)
        {
            if (close)
            {
                quote_inside = true;
                break;
            }
            if (currentCh == '\'')
            {
                close = true;
            }
            NextCh();
        }


        if (currentCharValue != -1 || quote_inside || !close) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("String is recognized");

    }
}

public class CommentLexer : Lexer
{
    protected System.Text.StringBuilder CommentString;

    public CommentLexer(string input)
        : base(input)
    {
        CommentString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '/')
        {
            NextCh();
        }
        else
        {
            Error();
        }

        if (currentCh == '*')
        {
            NextCh();
        }
        else
        {
            Error();
        }

        bool last_star = false;
        bool last_stick = false;
        bool error = false;
        while (currentCharValue != -1)
        {
            if (last_star && last_stick)
            {
                error = true;
                break;
            }
            else if (currentCh == '*')
            {
                last_star = true;
            }
            else if (currentCh != '/' && last_star)
            {
                last_star = false;
            }
            else if (currentCh == '/' && last_star)
            {
                last_stick = true;
            }

            
            NextCh();
        }


        if (currentCharValue != -1 || error || !last_star || !last_stick) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Comment is recognized");

    }
}

public class IdGroupLexer : Lexer
{
    protected System.Text.StringBuilder IdGroupString;

    public IdGroupLexer(string input)
        : base(input)
    {
        IdGroupString = new System.Text.StringBuilder();
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

        bool new_id = true;
        bool error = false;
        while ((char.IsDigit(currentCh) && new_id) || char.IsLetter(currentCh) || (currentCh == '.' && new_id))
        {
            if (currentCh == '.')
            {
                new_id = false;
                error = true;
            }
            if(char.IsLetter(currentCh))
            {
                new_id = true;
                error = false;
            }
            NextCh();
        }


        if (currentCharValue != -1 || error) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Id sequence is recognized");

    }
}


public class Program
{
    private static void TestIntLexer()
    {
        System.Console.WriteLine("/----------------------------------------------/");
        System.Console.WriteLine("Тестирование IntLexer...");
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

        System.Console.WriteLine("Тест для строки " + s1);
        try
        {
            L1.Parse();        
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s5);
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
        System.Console.WriteLine("Тестирование ToIntLexer...");

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

        System.Console.WriteLine("Тест для строки " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s5);
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
        System.Console.WriteLine("Тестирование IdLexer...");

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

        System.Console.WriteLine("Тест для строки " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s5);
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
        System.Console.WriteLine("Тестирование SignIntLexer...");

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

        System.Console.WriteLine("Тест для строки " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s5);
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
        System.Console.WriteLine("Тестирование CharDigitLexer...");

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

        System.Console.WriteLine("Тест для строки " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s5);
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
        System.Console.WriteLine("Тестирование LetterSequenceLexer...");

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

        System.Console.WriteLine("Тест для строки " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s5);
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
        System.Console.WriteLine("Тестирование DigitSequenceLexer...");

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

        System.Console.WriteLine("Тест для строки " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s5);
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
        System.Console.WriteLine("Тестирование CharDigitGroupsLexer...");

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

        System.Console.WriteLine("Тест для строки " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s5);
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
        System.Console.WriteLine("Тестирование DoubleLexer...");

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

        System.Console.WriteLine("Тест для строки " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s5);
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

    private static void TestTextLexer()
    {
        System.Console.WriteLine("/----------------------------------------------/");
        System.Console.WriteLine("Тестирование TextLexer...");

        string s1 = "'fqqwe'";
        string s2 = "'fqw'ewewe'";
        string s3 = "eww'";
        string s4 = "''";
        string s5 = "'qwerew''";

        Lexer L1 = new TextLexer(s1);
        Lexer L2 = new TextLexer(s2);
        Lexer L3 = new TextLexer(s3);
        Lexer L4 = new TextLexer(s4);
        Lexer L5 = new TextLexer(s5);

        System.Console.WriteLine("Тест для строки " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s5);
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

    private static void TestCommentLexer()
    {
        System.Console.WriteLine("/----------------------------------------------/");
        System.Console.WriteLine("Тестирование CommentLexer...");

        string s1 = "/**/";
        string s2 = "/*ffq*fe/*/";
        string s3 = "/qwe";
        string s4 = "/*ewqeqe";
        string s5 = "*eee*/";

        Lexer L1 = new CommentLexer(s1);
        Lexer L2 = new CommentLexer(s2);
        Lexer L3 = new CommentLexer(s3);
        Lexer L4 = new CommentLexer(s4);
        Lexer L5 = new CommentLexer(s5);

        System.Console.WriteLine("Тест для строки " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s5);
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

    private static void TestIdGroupLexer()
    {
        System.Console.WriteLine("/----------------------------------------------/");
        System.Console.WriteLine("Тестирование IdGroupLexer...");

        string s1 = "a.b12c.fe";
        string s2 = ".a123f.ff";
        string s3 = "abc12";
        string s4 = "a12f.qe.";
        string s5 = "12.ffqwe2.ff";
        string s6 = "fqeqwe.12eqer.1fe3";
        string s7 = "";
        string s8 = "0";

        Lexer L1 = new IdGroupLexer(s1);
        Lexer L2 = new IdGroupLexer(s2);
        Lexer L3 = new IdGroupLexer(s3);
        Lexer L4 = new IdGroupLexer(s4);
        Lexer L5 = new IdGroupLexer(s5);
        Lexer L6 = new IdGroupLexer(s6);
        Lexer L7 = new IdGroupLexer(s7);
        Lexer L8 = new IdGroupLexer(s8);

        System.Console.WriteLine("Тест для строки " + s1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s5);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s6);
        try
        {
            L6.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s7);
        try
        {
            L7.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("Тест для строки " + s8);
        try
        {
            L8.Parse();
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
        TestTextLexer();
        TestCommentLexer();
        TestIdGroupLexer();
    }
}