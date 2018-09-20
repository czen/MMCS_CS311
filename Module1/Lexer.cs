using System.Collections.Generic;

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

// Task 1
public class IntLexerToInt : Lexer
{
    protected System.Text.StringBuilder intString;
    protected string strResult;

    public IntLexerToInt(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            strResult += currentCh;
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
            strResult += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            strResult += currentCh;
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        int result = 0;
        if (int.TryParse(strResult, out result))
        {
            System.Console.WriteLine("Integer is {0}", result);
        }
        else
        {
            Error();
        }
    }
}

// Task 2
public class IdLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public IdLexer(string input)
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

// Task 3
public class CorrectIntLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public CorrectIntLexer(string input)
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

// Task 4
public class LetDigLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public LetDigLexer(string input)
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

        bool prevIsLetter = true;

        while ((char.IsDigit(currentCh) && prevIsLetter) || (char.IsLetter(currentCh) && !prevIsLetter))
        {
            prevIsLetter = !prevIsLetter;
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
public class LettersLexer : Lexer
{

    protected System.Text.StringBuilder intString;
    protected List<char> resultList = new List<char>();

    public LettersLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    protected bool IsDelim(char ch)
    {
        return (ch == ';' || ch == ',');
    }

    public override void Parse()
    {
        NextCh();
        if (char.IsLetter(currentCh))
        {
            resultList.Add(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        bool prevIsDelim = false;

        if (IsDelim(currentCh))
        {
            prevIsDelim = true;
            NextCh();
        }

        while ((char.IsLetter(currentCh) && prevIsDelim) || (IsDelim(currentCh) && !prevIsDelim))
        {
            if (prevIsDelim)
                resultList.Add(currentCh);

            prevIsDelim = !prevIsDelim;
            NextCh();
        }

        if (prevIsDelim)
        {
            Error();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("List of letters is:");
        resultList.ForEach(System.Console.WriteLine);

    }
}

// ExtraTask 1
public class DigitsLexer : Lexer
{

    protected System.Text.StringBuilder intString;
    protected List<char> resultList = new List<char>();

    public DigitsLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (char.IsDigit(currentCh))
        {
            resultList.Add(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        bool prevIsDig = true;
   
        while ((char.IsDigit(currentCh) && !prevIsDig) || currentCh == ' ')
        {
            if (char.IsDigit(currentCh))
            {
                resultList.Add(currentCh);
                prevIsDig = true;
            }
            else
            {
                prevIsDig = false;
            }

            NextCh();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("List of digits is:");
        resultList.ForEach(System.Console.WriteLine);

    }
}

// ExtraTask 2          //
public class LettersDigitsLexer : Lexer
{

    protected System.Text.StringBuilder intString;
    protected string resultStr;

    public LettersDigitsLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (char.IsLetter(currentCh))
        {
            resultStr += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        bool prevIsLet = true;
        bool isSecond = false;
        
        while (char.IsLetter(currentCh) || char.IsDigit(currentCh))
        {
            if (char.IsLetter(currentCh))
            {
                if (prevIsLet)
                {
                    if (!isSecond)
                    {
                        isSecond = true;
                        resultStr += currentCh;
                    }
                    else
                    {
                        Error();    // third letter
                    }
                }
                else // prev is Digit
                {
                    prevIsLet = true;
                    isSecond = false;
                    resultStr += currentCh;
                }
            
            }
            
            if (char.IsDigit(currentCh))
            {
                if (!prevIsLet) // prev is Digit
                {
                    if (!isSecond)
                    {
                        isSecond = true;
                        resultStr += currentCh;
                    }
                    else
                    {
                        Error();
                    }
                }
                else // prev is Letter
                {
                    prevIsLet = false;
                    isSecond = false;
                    resultStr += currentCh;
                }
            }
            NextCh();
        }

        // если нельзя, чтобы строка заканчивалась на группу букв
        if (prevIsLet)
        {
            Error();
        }

         if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("String is recognized: {0}", resultStr);

    }
}


// ExtraTask 3          //
public class FloatLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public FloatLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
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

        bool dotOccured = false;
        bool prevIsDot = false;

        while (char.IsDigit(currentCh) || (currentCh == '.' && !dotOccured))
        {
            if (currentCh == '.')
            {
                dotOccured = true;
                prevIsDot = true;
            }
            else prevIsDot = false;
            NextCh();
        }

        if (currentCharValue != -1 || !dotOccured || prevIsDot) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Float is recognized");

    }
}

// ExtraTask 4
public class StringLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public StringLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
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

        while (currentCh != '\'')
        {
            NextCh();
            if (currentCharValue == -1)
            {
                Error();
            }
        }

        if (currentCh == '\'')
        {
            NextCh();
        }
        else
        {
            Error();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("String is recognized");

    }
}

// ExtraTask 5
public class CommentLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public CommentLexer(string input)
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

        bool prevIsAsterisk = false;

        while (!(currentCh == '/' && prevIsAsterisk))
        {
            if (currentCh == '*')
                prevIsAsterisk = true;
            else
                prevIsAsterisk = false;

            NextCh();
            if (currentCharValue == -1)
            {
                Error();
            }
        }

        if (currentCh == '/' && prevIsAsterisk)
        {
            NextCh();
        }
        else
        {
            Error();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("String is recognized");

    }
}


public class Program
{
    public static void Test1()
    {
        // Task 1 // "" 0 a 12312+ 
        System.Console.WriteLine("Tests for Task 1");
        string input = "154216";
        Lexer L = new IntLexerToInt(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "-154";
        L = new IntLexerToInt(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "+1216";
        L = new IntLexerToInt(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "0";
        L = new IntLexerToInt(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("\nThere should be errors:");

        input = "-12acv3";
        L = new IntLexerToInt(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "";
        L = new IntLexerToInt(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a";
        L = new IntLexerToInt(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "1235+";
        L = new IntLexerToInt(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "12-35";
        L = new IntLexerToInt(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
    }

    public static void Test2()
    {
        System.Console.WriteLine("\n------------------");
        System.Console.WriteLine("\nTests for Task 2");
        string input = "ab23";
        Lexer L = new IdLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a65hf56g";
        L = new IdLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "d21f3D";
        L = new IdLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("\nThere should be errors:");

        input = "007";
        L = new IdLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "";
        L = new IdLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
    }

    public static void Test3()
    {
        System.Console.WriteLine("\n------------------");
        System.Console.WriteLine("\nTests for Task 3");
        string input = "154216";
        Lexer L = new CorrectIntLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "-154";
        L = new CorrectIntLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "+1210";
        L = new CorrectIntLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("\nThere should be errors:");

        input = "-12acv3";
        L = new CorrectIntLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "014";
        L = new CorrectIntLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "-0156";
        L = new CorrectIntLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
    }

    public static void Test4()
    {
        System.Console.WriteLine("\n------------------");
        System.Console.WriteLine("\nTests for Task 4");
        string input = "a1b2b3";
        Lexer L = new LetDigLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a1b4c";
        L = new LetDigLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a";
        L = new LetDigLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("\nThere should be errors:");

        input = "4a2s";
        L = new LetDigLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "aa1d2d3";
        L = new LetDigLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "b2h345";
        L = new LetDigLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "";
        L = new LetDigLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "0";
        L = new LetDigLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }




    }

    public static void Test5()
    {
        System.Console.WriteLine("\n------------------");
        System.Console.WriteLine("\nTests for Task 5");
        string input = "a;b,c;d,f";
        Lexer L = new LettersLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a";
        L = new LettersLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("\nThere should be errors:");

        input = "a;;b";
        L = new LettersLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a;b,c;d;";
        L = new LettersLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = ";a,b,c";
        L = new LettersLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a,b,5,c";
        L = new LettersLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "0";
        L = new LettersLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "";
        L = new LettersLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
    }

    public static void TestExtra1()
    {
        System.Console.WriteLine("\n------------------");
        System.Console.WriteLine("\nTests for ExtraTask 1");
        string input = "1 2   4  5        7     ";
        Lexer L = new DigitsLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "1 2   1";
        L = new DigitsLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "0";
        L = new DigitsLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("\nThere should be errors:");

        input = "1 2 41";
        L = new DigitsLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "1 2 b 1";
        L = new DigitsLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a";
        L = new DigitsLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "";
        L = new DigitsLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
    }

    public static void TestExtra2()
    {
        System.Console.WriteLine("\n------------------");
        System.Console.WriteLine("\nTests for ExtraTask 2");
        string input = "aa12c23dd1";
        Lexer L = new LettersDigitsLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "as2";
        L = new LettersDigitsLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a24d6";
        L = new LettersDigitsLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("\nThere should be errors:");

        input = "22ac6";
        L = new LettersDigitsLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "aaa2c222";
        L = new LettersDigitsLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "aa55dc";
        L = new LettersDigitsLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "";
        L = new LettersDigitsLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
    }

    public static void TestExtra3()
    {
        System.Console.WriteLine("\n------------------");
        System.Console.WriteLine("\nTests for ExtraTask 3");
        string input = "123.45678";
        Lexer L = new FloatLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "0.1234";
        L = new FloatLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("\nThere should be errors:");

        input = "12.";
        L = new FloatLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "12";
        L = new FloatLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = ".12";
        L = new FloatLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a";
        L = new FloatLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "";
        L = new FloatLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
    }

    public static void TestExtra4()
    {
        System.Console.WriteLine("\n------------------");
        System.Console.WriteLine("\nTests for ExtraTask 4");
        string input = "'abc2'";
        Lexer L = new StringLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "'a'";
        L = new StringLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "''";
        L = new StringLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("\nThere should be errors:");

        input = "'as'fg'";
        L = new StringLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "'a";
        L = new StringLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a'";
        L = new StringLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a";
        L = new StringLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "";
        L = new StringLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
    }

    public static void TestExtra5()
    {
        System.Console.WriteLine("\n------------------");
        System.Console.WriteLine("\nTests for ExtraTask 5");
        string input = "/*ab3s*/";
        Lexer L = new CommentLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "/*a*/";
        L = new CommentLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "/**/";
        L = new CommentLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        System.Console.WriteLine("\nThere should be errors:");

        input = "/*abc*/adf";
        L = new CommentLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "/*abc*/adf*/";
        L = new CommentLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "*/a";
        L = new CommentLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a*/";
        L = new CommentLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "a";
        L = new CommentLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        input = "";
        L = new CommentLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
    }

    public static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Test5();
        TestExtra1();
        TestExtra2();
        TestExtra3();
        TestExtra4();
        TestExtra5();
    }
}