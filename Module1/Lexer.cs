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
    public String numberString;
    public int number;

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
            numberString += currentCh;
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
            numberString += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            numberString += currentCh;
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {            
            Error();
        }

        number = Convert.ToInt32(numberString);
        System.Console.WriteLine("Integer is recognized " + number);

    }
}

public class IdLexer : Lexer
{

    protected System.Text.StringBuilder idString;
    public String id;

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
            id += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh) || char.IsLetter(currentCh))
        {
            id += currentCh;
            NextCh();
        }

        if (currentCharValue != -1)
        {
            Error();
        }
        
        System.Console.WriteLine("id is recognized " + id);

    }
}

public class IntZeroLexer : Lexer
{

    protected System.Text.StringBuilder intString;
    public String numberString;
    public int number;

    public IntZeroLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            numberString += currentCh;
            NextCh();
        }

        if (char.IsDigit(currentCh) && currentCh != '9')
        {
            numberString += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            numberString += currentCh;
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        number = Convert.ToInt32(numberString);
        System.Console.WriteLine("Integer without 0 recognized " + number);

    }
}

public class AlternateCharDigitLexer: Lexer
{
    protected System.Text.StringBuilder mesString;
    public String message;

    public AlternateCharDigitLexer(string input)
        : base(input)
    {
        mesString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();

        if (char.IsLetter(currentCh))
        {
            message += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }
        
        while (true)
        {
            if (currentCharValue == -1)
                break;

            if (!char.IsDigit(currentCh)) {
                Error();
            }
            message += currentCh;
            NextCh();

            if (currentCharValue == -1)
                break;

            if (!char.IsLetter(currentCh)){
                Error();
            }
            message += currentCh;
            NextCh();
        }

        if (currentCharValue != -1)
        {
            Error();
        }

        System.Console.WriteLine("string with alternate chars and digits is recognized " + message);
    }
}

public class SeparatedCharsLexer : Lexer
{
    protected System.Text.StringBuilder mesString;
    public String message;

    public SeparatedCharsLexer(string input)
        : base(input)
    {
        mesString = new System.Text.StringBuilder();
        message = "";
    }

    public override void Parse()
    {
        NextCh();

        while (true)
        {
            if (char.IsLetter(currentCh))
            {
                message += currentCh;
                NextCh();
            } else if (currentCh == ',' || currentCh == ';')
            {
                NextCh();
                break;
            }
            else
            {
                Error();
            }
        }

        while (char.IsLetter(currentCh))
        {
            message += currentCh;
            NextCh();
        }

        if (currentCharValue != -1)
        {
            Error();
        }

        System.Console.WriteLine("string with separated chars by ; or , is recognised " + message);
    }

    public static void Testing()
    {
        var tests = new Dictionary<string, string>{
            { "a;", "a" },
            { ";fr", "fr"},
            { "abg,abg", "abgabg"},
            { "abg;;", "error"},
            { ",,", "error"},
            { "tl;dr", "tldr"},
            { ",glO", "glO"}
        };

        foreach (var t in tests)
        {
            var L = new SeparatedCharsLexer(t.Key);
            bool passed = false;
            try
            {
                L.Parse();
                passed = L.message.Equals(t.Value);                
            }
            catch (LexerException e)
            {
                passed = true;
            }

            if (passed)
            {
                System.Console.WriteLine("Test is passed");
            }
            else
            {
                System.Console.WriteLine("Test is not passed");
            }
        }
        
    }
}

public class Program
{
    public static void Main()
    {
        SeparatedCharsLexer.Testing();
        /*
        string input = "-154216";
        Lexer L = new IntLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }*/
        // Console.ReadLine();
    }
}