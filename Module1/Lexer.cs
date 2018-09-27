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
    //jh
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
        string s = "";

        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            s += currentCh;
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
            s += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            s += currentCh;
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        int an = Int32.Parse(s);
        System.Console.WriteLine("Integer is recognized");

    }
}

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

        System.Console.WriteLine("id");

    }
}

public class Int2Lexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public Int2Lexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string s = "";

        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            s += currentCh;
            NextCh();
        }

        if (char.IsDigit(currentCh) && currentCh != '0')
        {
            s += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            s += currentCh;
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        int an = Int32.Parse(s);
        System.Console.WriteLine(an);

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

        System.Console.WriteLine("right");

    }
}

public class LetLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public LetLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string s = "";

        NextCh();
        while (currentCharValue != -1)
        {
            if (char.IsLetter(currentCh))
            {
                s += currentCh;
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

        System.Console.WriteLine(s);

    }
}

public class T6Lexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public T6Lexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        
        List<int> a = new List<int>();

        NextCh();
        while (currentCharValue != -1)
        {
            if (char.IsDigit(currentCh))
            {
                a.Add(Int32.Parse(currentCh.ToString()));
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

        foreach (var v in a)
            System.Console.WriteLine(v);
        {

        }

    }
}

public class T7Lexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public T7Lexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string s = "";
        NextCh();
        while (currentCharValue != -1)
        {
            if (char.IsDigit(currentCh))
            {
                s += currentCh;
                NextCh();
                if (char.IsDigit(currentCh))
                {
                    s += currentCh;
                    NextCh();
                }
                if (char.IsDigit(currentCh))
                    Error();
            }
            if (char.IsLetter(currentCh))
            {
                s += currentCh;
                NextCh();
                if (char.IsLetter(currentCh))
                {
                    s += currentCh;
                    NextCh();
                }
                if (char.IsLetter(currentCh))
                    Error();
            }
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }
        System.Console.WriteLine(s);
        {

        }

    }
}

public class T8Lexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public T8Lexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string s = "";
        double res;

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

        while (currentCharValue != '.')
        {
            if (char.IsDigit(currentCh))
            {
                s += currentCh;
                NextCh();
            }
            else
            {
                Error();
            }
        }
        s += ',';
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

        while (currentCharValue != -1)
        {
            if (char.IsDigit(currentCh))
            {
                s += currentCh;
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


        res = (Double.Parse(s));
        System.Console.WriteLine(res);
        {

        }

    }
}

public class T9Lexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public T9Lexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string s = "";

        NextCh();
        if (currentCh.ToString() != "'")
        {
            Error();
        }
        NextCh();
        while (currentCh.ToString() != "'")
        {
            s += currentCh;
            NextCh();
            if (currentCharValue == -1)
                Error();
        }
        Console.WriteLine(s);
    }
}

public class T10Lexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public T10Lexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string s = "";

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
                s += currentCh;
                NextCh();
            }
            NextCh();
            if (currentCh != '/')
            {
                s += '*';
            }
        }
        Console.WriteLine(s);
    }
}

//additional task
public class TAddLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public TAddLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string s = "";
        List<string> a = new List<string>();

        NextCh();
        while (currentCharValue != -1)
        {
            if (char.IsLetter(currentCh))
            {
                s += currentCh;
                NextCh();
            }
            else
            {
                Error();
            }

            while (currentCh != '.')
            {
                if (currentCharValue == -1)
                    break;
                s += currentCh;
                NextCh();
            }
            if (currentCh == '.')
            {
                NextCh();
                if (currentCharValue == -1)
                    Error();
            }
            a.Add(s);
            s = "";

        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }
        foreach (var v in a)
            Console.WriteLine(v);
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
        //тесты сделаны с намеренными ошибками
        TestLexer(new IntLexer("325"));
        TestLexer(new IntLexer("+-325"));

        TestLexer(new IdLexer("qsdfja"));
        TestLexer(new IdLexer("1sdfja"));

        TestLexer(new Int2Lexer("-1222"));
        TestLexer(new Int2Lexer("01222"));

        TestLexer(new Task4("123x123122"));
        TestLexer(new Task4("a1b2c3d4"));

        TestLexer(new LetLexer("a,b;c,d"));
        TestLexer(new LetLexer("av.d"));

        TestLexer(new T6Lexer("1          2    3"));
        TestLexer(new T6Lexer("13     4"));

        TestLexer(new T7Lexer("aa11b12cc"));
        TestLexer(new T7Lexer("aaa2"));

        TestLexer(new T8Lexer("123.456"));
        TestLexer(new T8Lexer("1.2.3"));
        TestLexer(new T8Lexer("123."));
        TestLexer(new T8Lexer(".456"));

        TestLexer(new T9Lexer("'eeeeeee'"));
        TestLexer(new T9Lexer("'aweaeawe"));

        TestLexer(new T10Lexer("/*123*/"));
        TestLexer(new T10Lexer("qweqw"));

        TestLexer(new TAddLexer("id1.id2.id3"));
        TestLexer(new TAddLexer("id1."));

        Console.ReadKey();
    }
}


/*public class Program
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

    }
}*/
