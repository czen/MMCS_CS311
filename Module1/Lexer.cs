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

// Дополнительное задание 1
// (здесь не допускаются пробелы в начале и конце списка)
public class DigitsLexer : Lexer
{
    string digits;
    public DigitsLexer(string input)
        : base(input) {}

    public override void Parse()
    {
        NextCh();
        while (true)
        {
            if (char.IsDigit(currentCh))
            {
                digits += currentCh;
                NextCh();
            }
            else
                Error();

            if (currentCharValue == -1)
            {
                System.Console.WriteLine("List of digits is recognized: " + digits);
                break;
            }

            if (currentCh == ' ')
                NextCh();
            else
                Error();
            while (currentCh == ' ')
                NextCh();
        }
    }
}

// Дополнительное задание 2
public class DigitsLettersGroupsLexer : Lexer
{
    string groups;

    public DigitsLettersGroupsLexer(string input)
        : base(input) {}

    public override void Parse()
    {
        NextCh();

        // Если последовательность начинается с цифр, 
        // прочтём их и перейдём к буквам
        if (char.IsDigit(currentCh))
        {
            groups += currentCh;
            NextCh();
            if (char.IsDigit(currentCh))
            {
                groups += currentCh;
                NextCh();
            }
        }

        while (true)
        {
            if (currentCharValue == -1)
            {
                System.Console.WriteLine("Groups of digits and letters are recognized: " + groups);
                break;
            }

            if (char.IsLetter(currentCh))
            {
                groups += currentCh;
                NextCh();
                if (char.IsLetter(currentCh))
                {
                    groups += currentCh;
                    NextCh();
                }
            }
            else
                Error();

            if (currentCharValue == -1)
            {
                System.Console.WriteLine("Groups of digits and letters are recognized: " + groups);
                break;
            }

            if (char.IsDigit(currentCh))
            {
                groups += currentCh;
                NextCh();
                if (char.IsDigit(currentCh))
                {
                    groups += currentCh;
                    NextCh();
                }
            }
            else
                Error();
        }
    }
}

// Дополнительное задание 3
public class DoubleLexer : Lexer
{
    string DoubleNum = "";
    public DoubleLexer(string input)
        : base(input) {}

    public override void Parse()
    {
        NextCh();

        if (char.IsDigit(currentCh))
        {
            DoubleNum += currentCh;
            NextCh();
        }
        else
            Error();

        while (char.IsDigit(currentCh))
        {
            DoubleNum += currentCh;
            NextCh();
        }

        if (currentCh == '.')
        {
            DoubleNum += ',';
            NextCh();
        }
        else
            Error();

        if (char.IsDigit(currentCh))
        {
            DoubleNum += currentCh;
            NextCh();
        }
        else
            Error();

        while (char.IsDigit(currentCh))
        {
            DoubleNum += currentCh;
            NextCh();
        }

        if (currentCharValue != -1)
            Error();

        System.Console.WriteLine("Double is recognized: " + System.Double.Parse(DoubleNum));
    }
}

// Дополнительное задание 4
public class StringLexer : Lexer
{
    public StringLexer(string input)
        : base(input) {}

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
            NextCh();
        }
        NextCh();
        if (currentCharValue != -1)
            Error();
        System.Console.WriteLine("String is recognized");
    }
}

// Дополнительное задание 5
public class CommentLexer : Lexer
{
    public CommentLexer(string input)
        : base(input) {}

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

        while (true)
        {
            while (currentCh != '*')
            {
                if (currentCharValue == -1)
                    Error();
                NextCh();
            }
            NextCh();
            if (currentCh == '/')
            {
                NextCh();
                if (currentCharValue != -1)
                    Error();
                System.Console.WriteLine("Comment is recognized");
                break;
            }
            else
                Error();
        }
    }
}

// Дополнительное сложное задание
public class IDListLexer : Lexer
{
    public IDListLexer(string input)
        : base(input) {}

    public override void Parse()
    {
        NextCh();
        while (true)
        {
            if (char.IsLetter(currentCh))
                NextCh();
            else
                Error();
            while (char.IsLetterOrDigit(currentCh))
                NextCh();

            if (currentCharValue == -1)
            {
                System.Console.WriteLine("ID List is recognized");
                break;
            }

            if (currentCh == '.')
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

        // Дополнительное задание 1
        System.Console.WriteLine("\nExtra Task 1:");

        input = "4";
        Lexer LL1 = new DigitsLexer(input);
        try
        {
            LL1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "4 5   2";
        LL1 = new DigitsLexer(input);
        try
        {
            LL1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "4,5   2";
        LL1 = new DigitsLexer(input);
        try
        {
            LL1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "1 2  3   4    5";
        LL1 = new DigitsLexer(input);
        try
        {
            LL1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "1 2 34  5  6";
        LL1 = new DigitsLexer(input);
        try
        {
            LL1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // Дополнительное задание 2
        System.Console.WriteLine("\nExtra Task 2:");

        input = "aa12c23dd1";
        Lexer LL2 = new DigitsLettersGroupsLexer(input);
        try
        {
            LL2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "4";
        LL2 = new DigitsLettersGroupsLexer(input);
        try
        {
            LL2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "4sd35r5d5t03ab";
        LL2 = new DigitsLettersGroupsLexer(input);
        try
        {
            LL2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "ab0k";
        LL2 = new DigitsLettersGroupsLexer(input);
        try
        {
            LL2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a4bcd5e";
        LL2 = new DigitsLettersGroupsLexer(input);
        try
        {
            LL2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "ab4c,d5e";
        LL2 = new DigitsLettersGroupsLexer(input);
        try
        {
            LL2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        // Дополнительное задание 3
        System.Console.WriteLine("\nExtra Task 3:");

        input = "1.0";
        Lexer LL3 = new DoubleLexer(input);
        try
        {
            LL3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "4";
        LL3 = new DoubleLexer(input);
        try
        {
            LL3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "55.";
        LL3 = new DoubleLexer(input);
        try
        {
            LL3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = ".3";
        LL3 = new DoubleLexer(input);
        try
        {
            LL3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "12.345";
        LL3 = new DoubleLexer(input);
        try
        {
            LL3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // Дополнительное задание 4
        System.Console.WriteLine("\nExtra Task 4:");

        input = "a'bcd'";
        Lexer LL4 = new StringLexer(input);
        try
        {
            LL4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "'a'";
        LL4 = new StringLexer(input);
        try
        {
            LL4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "'a'bcd";
        LL4 = new StringLexer(input);
        try
        {
            LL4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "'a123bcd0'";
        LL4 = new StringLexer(input);
        try
        {
            LL4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "'123";
        LL4 = new StringLexer(input);
        try
        {
            LL4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // Дополнительное задание 5
        System.Console.WriteLine("\nExtra Task 5:");

        input = "/*12345";
        Lexer LL5 = new CommentLexer(input);
        try
        {
            LL5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "12/*345*/";
        LL5 = new CommentLexer(input);
        try
        {
            LL5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "/*1234*/5";
        LL5 = new CommentLexer(input);
        try
        {
            LL5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "/*12345*/";
        LL5 = new CommentLexer(input);
        try
        {
            LL5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // Дополнительное сложное задание
        System.Console.WriteLine("\nHard Extra Task:");

        input = "a225";
        Lexer LLL = new IDListLexer(input);
        try
        {
            LLL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a225.abcd.e00e00.k";
        LLL = new IDListLexer(input);
        try
        {
            LLL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a225.abcd e00e00.k";
        LLL = new IDListLexer(input);
        try
        {
            LLL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a225..abcd.e00e00.k";
        LLL = new IDListLexer(input);
        try
        {
            LLL.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
    }
}