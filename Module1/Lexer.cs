using System;
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
        string str = "";

        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            str += currentCh;
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
            str += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            str += currentCh;
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        int res = Convert.ToInt32(str);

        System.Console.WriteLine("Задание 1 выполняется правильно");
        System.Console.WriteLine("Результат: {0}", res);
        System.Console.WriteLine("");
    }
}

public class Task2 : Lexer
{

    protected System.Text.StringBuilder intString;

    public Task2(string input)
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

        System.Console.WriteLine("Задание 2 выполняется правильно");
        System.Console.WriteLine("");
    }
}

public class Task3 : Lexer
{

    protected System.Text.StringBuilder intString;

    public Task3(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string str = "";

        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            str += currentCh;
            NextCh();
        }

        if (char.IsDigit(currentCh) && currentCh != '0')
        {
            str += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            str += currentCh;
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        int res = Convert.ToInt32(str);

        System.Console.WriteLine("Задание 3 выполняется правильно");
        System.Console.WriteLine("Результат: {0}", res);
        System.Console.WriteLine("");

    }
}

public class Task4 : Lexer
{

    protected System.Text.StringBuilder intString;

    public Task4(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {

        NextCh();
        while (currentCharValue != -1)
        {
            if (char.IsLetter(currentCh))
            {
                NextCh();
            }
            else
            {
                Error();
            }

            if (currentCharValue == -1)
                break;

            if (char.IsDigit(currentCh))
            {
                NextCh();
            }
            else
            {
                Error();
            }

        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Задание 4 выполняется правильно");
        System.Console.WriteLine("");
    }
}

public class Task5 : Lexer
{

    protected System.Text.StringBuilder intString;

    public Task5(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string str = "";

        NextCh();
        while (currentCharValue != -1)
        {
            if (char.IsLetter(currentCh))
            {
                str += currentCh;
                NextCh();
            }
            else
            {
                Error();
            }

            if (currentCharValue == -1)
                break;
            if (currentCh == ',' || currentCh == ';')
            {
                NextCh();
            }
            else
            {
                Error();
            }

            if (currentCharValue == -1)
                Error();

        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }


        System.Console.WriteLine("Задание 5 выполняется правильно");
        System.Console.WriteLine("Результат: {0}", str);
        System.Console.WriteLine("");

    }
}


public class Task6 : Lexer
{

    protected System.Text.StringBuilder intString;

    public Task6(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        List<int> lit = new List<int>();

        NextCh();
        while (currentCharValue != -1)
        {
            if (char.IsDigit(currentCh))
            {
                lit.Add(Convert.ToInt32(currentCh.ToString()));
                NextCh();
            }

            if (currentCharValue == -1)
                break;

            if (currentCh != ' ')
                Error();
            else
                while (currentCh == ' ')
                {
                    if (currentCharValue == -1)
                        break;
                    NextCh();
                }
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Задание 6 выполняется правильно");
        System.Console.WriteLine("Результат:");

        foreach (var v in lit)
        {
            System.Console.WriteLine(v);
        }
        System.Console.WriteLine("");
    }
}

public class Task7 : Lexer
{

    protected System.Text.StringBuilder intString;

    public Task7(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string str = "";
        NextCh();
        while (currentCharValue != -1)
        {
            if (char.IsDigit(currentCh))
            {
                str += currentCh;
                NextCh();
                if (char.IsDigit(currentCh))
                {
                    str += currentCh;
                    NextCh();

                    if (char.IsDigit(currentCh))
                        Error();
                }
            }
            if (char.IsLetter(currentCh))
            {
                str += currentCh;
                NextCh();
                if (char.IsLetter(currentCh))
                {
                    str += currentCh;
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

        System.Console.WriteLine("Задание 7 выполняется правильно");
        System.Console.WriteLine("Результат: {0}", str);
        System.Console.WriteLine("");
    }
}

public class Task8 : Lexer
{

    protected System.Text.StringBuilder intString;

    public Task8(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string str = "";
        double res;

        NextCh();

        if (char.IsDigit(currentCh))
        {
            str += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }


        while (currentCharValue != '.')
        {
            if (char.IsDigit(currentCh))
            {
                str += currentCh;
                NextCh();
            }
            else
            {
                Error();
            }
        }
        str += ',';
        NextCh();

        if (char.IsDigit(currentCh))
        {
            str += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        while (currentCharValue != -1)
        {
            if (char.IsDigit(currentCh))
            {
                str += currentCh;
                NextCh();
            }
            else
            {
                Error();
            }
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }


        res = (Convert.ToDouble(str));
        System.Console.WriteLine("Задание 8 выполняется правильно");
        System.Console.WriteLine("Результат: {0}", res);
        System.Console.WriteLine("");

    }
}

public class Task9 : Lexer
{

    protected System.Text.StringBuilder intString;

    public Task9(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string str = "";

        NextCh();
        if (currentCh.ToString() != "'")
        {
            Error();
        }
        NextCh();
        while (currentCh.ToString() != "'")
        {
            str += currentCh;
            NextCh();
            if (currentCharValue == -1)
                Error();
        }
        System.Console.WriteLine("Задание 9 выполняется правильно");
        System.Console.WriteLine("Результат: {0}", str);
        System.Console.WriteLine("");
    }
}

public class Task10 : Lexer
{

    protected System.Text.StringBuilder intString;

    public Task10(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string str = "";

        NextCh();
        if (currentCh != '/')
        {
            Error();
        }
        NextCh();
        if (currentCh != '*')
        {
            Error();
        }
        NextCh();
        while (currentCh != '/')
        {
            while (currentCh != '*')
            {
                str += currentCh;
                NextCh();
            }
            NextCh();
            if (currentCh != '/')
            {
                str += '*';
            }
        }
        System.Console.WriteLine("Задание 10 выполняется правильно");
        System.Console.WriteLine("Результат: {0}", str);
        System.Console.WriteLine("");
    }
}


public class Program
{
    public static void TestLexer(Lexer l)
    {
        try
        {
            l.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
    }
    public static void Main()
    {
        /*----------1 задание-------------------*/
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

        /*----------2 задание------------------*/
        input = "hggasasa";
        L = new Task2(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        /*----------3 задание------------------*/
        input = "-9976";
        L = new Task3(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        /*----------4 задание------------------*/
        input = "d4f5g6h7j8";
        L = new Task4(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        /*----------5 задание------------------*/
        input = "d;u;b,j";
        L = new Task5(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        /*----------6 задание------------------*/
        input = "2 4 5 6";
        L = new Task6(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        /*----------7 задание------------------*/
        input = "aa11b12cc";
        L = new Task7(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        /*----------8 задание------------------*/
        input = "1.456";
        L = new Task8(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        /*----------9 задание------------------*/
        input = "'jijijij'";
        L = new Task9(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        /*----------10 задание------------------*/
        input = "/*789876*/";
        L = new Task10(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        Console.ReadKey();
    }
}
