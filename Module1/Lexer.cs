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
    protected char currentCh;       // î÷åðåäíîé ñ÷èòàííûé ñèìâîë
    protected int currentCharValue; // öåëîå çíà÷åíèå î÷åðåäíîãî ñ÷èòàííîãî ñèìâîëà
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

        if (currentCharValue != -1) 
        {
            Error();
        }

        System.Console.WriteLine("Integer is recognized");
    }
}

public class Accumulate_Lexer : Lexer
{
    protected System.Text.StringBuilder integerString;
    public Accumulate_Lexer(string input) : base(input)
    {
        integerString = new System.Text.StringBuilder();
    }
    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            integerString.Append(currentCh);
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
            integerString.Append(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            integerString.Append(currentCh);
            NextCh();
        }


        if (currentCharValue != -1)
        {
            Error();
        }
        System.Console.WriteLine("result:" + integerString);
    }
}

public class Identifier : Lexer
{
    protected System.Text.StringBuilder intString;
    public Identifier(string input)
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

        while (char.IsLetter(currentCh) || char.IsDigit(currentCh))
            NextCh();

        if (currentCharValue != -1) 
            Error();
        System.Console.WriteLine("лексема вида: б(б|ц)*");
    }
}

public class IntLexer_without0 : Lexer
{
    protected System.Text.StringBuilder intString;

    public IntLexer_without0(string input)
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

        System.Console.WriteLine("OK");
    }
}

public class Letters_and_digits : Lexer
{
    protected System.Text.StringBuilder intString;
    char prev;

    public Letters_and_digits(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {       
        NextCh();
        if (char.IsLetter(currentCh))
        {
            prev = currentCh;
            NextCh();            
        }      
        else
            if (currentCharValue == -1)
            ;
        else
            Error();

        while (char.IsLetter(currentCh) && char.IsDigit(prev) || char.IsDigit(currentCh) && char.IsLetter(prev))
        {
            prev = currentCh;
            NextCh();
        }

        if (currentCharValue != -1)
            Error();

        System.Console.Write("Letters and digits, start with letter are recognized");
        System.Console.WriteLine();
    }
}

public class Letters_list : Lexer
{
    char prev = ';';
    protected System.Text.StringBuilder accum_string;

    public Letters_list(string input)
        : base(input)
    {
        accum_string = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();

        while (char.IsLetter(currentCh) && (prev == ';' || prev == ',')
                || (currentCh == ';' || currentCh == ',') && char.IsLetter(prev))
        {
            if (char.IsLetter(currentCh))
                accum_string.Append(currentCh);
            prev = currentCh;
            NextCh();
        }

        if (!char.IsLetter(prev) || currentCharValue != -1) 
            Error();


        System.Console.WriteLine("List of letters is recognized");
        System.Console.WriteLine(accum_string);
    }
}

public class List_of_digits : Lexer
{
    char prev;
    protected System.Text.StringBuilder intString;
    public List_of_digits(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (char.IsDigit(currentCh))
        {
            prev = currentCh;
            intString.Append(currentCh);        
            NextCh();
        }
        else
        {
            Error();
        }

        while (currentCharValue != -1)
        {
            if (char.IsDigit(currentCh) && prev == ' ')
                intString.Append(currentCh);
            else
            if (currentCh != ' ')
                Error();

            prev = currentCh;
            NextCh();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }


        System.Console.WriteLine("Integers are recognized");
        System.Console.WriteLine(intString);
        System.Console.WriteLine();
    }
}

public class digits_letters_groups : Lexer
{
    protected System.Text.StringBuilder intString;

    public digits_letters_groups(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();

        while (char.IsDigit(currentCh) || char.IsLetter(currentCh))
        {
            if (char.IsDigit(currentCh))
            {
                intString.Append(currentCh);
                NextCh();
                if (char.IsDigit(currentCh))
                {
                    intString.Append(currentCh);
                    NextCh();
                    if (char.IsDigit(currentCh))
                        Error();
                }
            }

            if (char.IsLetter(currentCh))
            {
                intString.Append(currentCh);
                NextCh();
                if (char.IsLetter(currentCh))
                {
                    intString.Append(currentCh);
                    NextCh();
                    if (char.IsLetter(currentCh))
                        Error();
                }
            }
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine();
        System.Console.WriteLine("result:" + intString);
    }
}

public class double_Lexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public double_Lexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
            NextCh();

        if (char.IsDigit(currentCh))
            NextCh();
        else
            Error();

        while (char.IsDigit(currentCh))
            NextCh();

        if (currentCh != '.') //StringReader вернет -1 в конце строки        
            Error();

        NextCh();
        if (!char.IsDigit(currentCh))
            Error();
        while (char.IsDigit(currentCh))
            NextCh();

        if (currentCharValue != -1)
            Error();

        System.Console.WriteLine("Integer is recognized");
    }
}

public class single_quote : Lexer
{
    protected System.Text.StringBuilder intString;

    public single_quote(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '\'')
            NextCh();
        else
            Error();


        while (currentCh != '\'' && currentCharValue != -1)
            NextCh();

        if (currentCharValue == -1) // StringReader вернет -1 в конце строки
            Error();

        NextCh();
        if (currentCharValue != -1)
            Error();

        System.Console.WriteLine("recognized");
    }
}

public class comment : Lexer
{
    char prev;
    protected System.Text.StringBuilder intString;
    public comment(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
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

        while (!(currentCh == '/' && prev == '*'))
        {            
            prev = currentCh;
            NextCh();
            if (currentCharValue == -1)
                Error();
        }
        NextCh();

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
            Error();

        System.Console.WriteLine("comment is recognized");
    }
}



public class Program
{ 
    public static void Accumulate_Lexer_Test()
    {
        string[] input = { "hello", "", "+144", "156", "-555", "www", "-", "+" };
        foreach (string str in input)
        {
            Lexer L = new Accumulate_Lexer(str);
            try
            {
                System.Console.WriteLine("input:" + str);
                L.Parse();
                System.Console.WriteLine();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine();
            }
        }
    }

    public static void Identifier_test()
    {
        string[] input = {"b", "b1", "nnnn", "n1n", "n1n1", "", "5", "5g" };
        foreach (string str in input)
        {
            Lexer L = new Identifier(str);
            try
            {
                System.Console.WriteLine("input:" + str);
                L.Parse();
                System.Console.WriteLine();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine();
            }
        }
    }

    public static void IntLexer_without0_test()
    {
        string[] input = { "hello", "", "+144", "156", "-555", "054", "-0", "+0" };
        foreach (string str in input)
        {
            Lexer L = new IntLexer_without0(str);
            try
            {
                System.Console.WriteLine("input:" + str);
                L.Parse();
                System.Console.WriteLine();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine();
            }
        }
    }

    public static void Letters_and_digits_test()
    {
        string[] input = { "n", "n1n1", "n1n", "n1", "1", "1n", "+", "", "nn", "n1n11"};
        foreach (string str in input)
        {
            Lexer L = new Letters_and_digits(str);
            try
            {
                System.Console.WriteLine("input:" + str);
                L.Parse();
                System.Console.WriteLine();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine();
            }
        }
    }

    public static void Letters_list_test()
    {
        string[] input = { "", "n", "n,n", "n;n", "n,", "1,n", "n,m;h"};
        foreach (string str in input)
        {
            Lexer L = new Letters_list(str);
            try
            {
                System.Console.WriteLine("input:" + str);
                L.Parse();
                System.Console.WriteLine();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine();
            }
        }
    }

    public static void Digits_list_test()
    {
        string[] input = { "1", "12", "1 22", "1 2", "1   4", "", "2 2 n" };
        foreach (string str in input)
        {
            Lexer L = new List_of_digits(str);
            try
            {
                System.Console.WriteLine("input:" + str);
                L.Parse();
                System.Console.WriteLine();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine();
            }
        }
    }

    public static void digits_letters_groups_test()
    {
        string[] input = { "aa12c23dd1", "12", "1 a", "aaa", "b", "45gg"};
        foreach (string str in input)
        {
            Lexer L = new digits_letters_groups(str);
            try
            {
                System.Console.WriteLine("input:" + str);
                L.Parse();
                System.Console.WriteLine();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine();
            }
        }
    }

    public static void double_Lexer_test()
    {
        string[] input = { "11.4", "12.", "", ".", ".45" };
        foreach (string str in input)
        {
            Lexer L = new double_Lexer(str);
            try
            {
                System.Console.WriteLine("input:" + str);
                L.Parse();
                System.Console.WriteLine();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine();
            }
        }
    }

    public static void single_quote_test()
    {
        string[] input = { "'hello'", "''", "'hel'lo'", "'", "'hello", "hello" };
        foreach (string str in input)
        {
            Lexer L = new single_quote(str);
            try
            {
                System.Console.WriteLine("input:" + str);
                L.Parse();
                System.Console.WriteLine();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine();
            }
        }
    }

    public static void comment_test()
    {
        string[] input = { "/*aaaa*/", "/*sdf*", "/**/", "", "/*dsa", "/*dsa*/asd*/" };
        foreach (string str in input)
        {
            Lexer L = new comment(str);
            try
            {
                System.Console.WriteLine("input:" + str);
                L.Parse();
                System.Console.WriteLine();
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine();
            }
        }
    }

    public static void Main()
    {      
        System.Console.WriteLine("////////////////////////////Task1\\\\\\\\\\\\\\\\\\\\\\\\\\");
        Accumulate_Lexer_Test();
        System.Console.WriteLine("////////////////////////////Task2\\\\\\\\\\\\\\\\\\\\\\\\\\");
        Identifier_test();
        System.Console.WriteLine("////////////////////////////Task3\\\\\\\\\\\\\\\\\\\\\\\\\\");
        IntLexer_without0_test();
        System.Console.WriteLine("////////////////////////////Task4\\\\\\\\\\\\\\\\\\\\\\\\\\");
        Letters_and_digits_test();
        System.Console.WriteLine("////////////////////////////Task5\\\\\\\\\\\\\\\\\\\\\\\\\\");
        Letters_list_test();

        System.Console.WriteLine("////////////////////////////ADD1\\\\\\\\\\\\\\\\\\\\\\\\\\");
        Digits_list_test();
        System.Console.WriteLine("////////////////////////////ADD2\\\\\\\\\\\\\\\\\\\\\\\\\\");
        digits_letters_groups_test();
        System.Console.WriteLine("////////////////////////////ADD3\\\\\\\\\\\\\\\\\\\\\\\\\\");
        double_Lexer_test();
        System.Console.WriteLine("////////////////////////////ADD4\\\\\\\\\\\\\\\\\\\\\\\\\\");
        single_quote_test();
        System.Console.WriteLine("////////////////////////////ADD5\\\\\\\\\\\\\\\\\\\\\\\\\\");
        comment_test();
    }
}