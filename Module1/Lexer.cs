// Пацеев Андрей. Лабараторная №1. Задания №6 - №10
// Задачи №1 - №5 вы уже оценили, они были выполнены в dotnetfiddle и загружены в мудл.

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
    protected char currentCh;
    protected int currentCharValue;
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

// Digits separated with one or more spaces.
public class DigitsWithSpaces : Lexer
{

    protected System.Text.StringBuilder intString;

    protected List<int> digits = new List<int>();

    public DigitsWithSpaces(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        bool spaceNext = false;
        NextCh();


        if (currentCharValue != -1)
        {
            while (char.IsDigit(currentCh) || currentCh == ' ')
            {
                if (char.IsDigit(currentCh))
                {
                    if (spaceNext) Error();
                    spaceNext = true;
                    int digit = (int)(char.GetNumericValue(currentCh));
                    digits.Add(digit);
                }
                else
                    spaceNext = false;
                NextCh();
            }

            if (currentCharValue != -1)
                Error();
        }

        System.Console.WriteLine("Parsed digits:");

        foreach (int digit in digits)
        {
            System.Console.WriteLine(digit);
        }

    }
}

// Alternate groups of digits and letters with length < 2
public class AlternateGroups : Lexer
{

    protected System.Text.StringBuilder intString;

    protected string result = "";

    public AlternateGroups(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        int digits = 0;
        int letters = 0;
        NextCh();

        while (currentCharValue != -1)
        {
            if (char.IsDigit(currentCh))
            {
                letters = 0;
                digits++;
                if (digits > 2) Error();
            }
            else if (char.IsLetter(currentCh))
            {
                digits = 0;
                letters++;
                if (letters > 2) Error();
            }
            else
                Error();

            NextCh();
        }

        System.Console.WriteLine("Parsed string:" + result);

    }
}

// Floating point
public class FloatingPoint : Lexer
{

    protected System.Text.StringBuilder intString;

    public FloatingPoint(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();


        if (!char.IsDigit(currentCh))
            Error();

        while (char.IsDigit(currentCh))
            NextCh();

        if (currentCh != '.')
            Error();

        NextCh();

        if (!char.IsDigit(currentCh))
            Error();

        while (char.IsDigit(currentCh))
            NextCh();

        if (currentCharValue != -1)
            Error();

    }
}

// String surrounded by ' symbol. Doesn't allow ' as part of the string
public class SingleString : Lexer
{

    protected System.Text.StringBuilder intString;

    public SingleString(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();

        if (currentCh != '\'')
            Error();

        NextCh();

        while (currentCh != '\'')
        {
            if (currentCharValue == -1)
                Error();
            NextCh();
        }

        NextCh();

        if (currentCharValue != -1)
            Error();
    }
}

// /*comment*/. Doesn't allow /* or */ inside.
public class MultilineComment : Lexer
{

    protected System.Text.StringBuilder intString;

    public MultilineComment(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();

        if (currentCh != '/')
            Error();

        NextCh();

        if (currentCh != '*')
            Error();

        NextCh();


        while (true)
        {
            if (currentCharValue == -1)
                Error();

            if (currentCh == '*')
            {
                NextCh();
                if (currentCh == '/')
                {
                    NextCh();
                    break;
                }
            }
            else
                NextCh();
        }

        if (currentCharValue != -1)
            Error();


    }
}

public class Program
{
    public static void Main()
    {
        // tests for digits with spaces
        string digitsSeparatedTrue = "1  4 2     1 6";
        string digitsSeparatedFalse = "1  22 ";
        Lexer digitsTrue = new DigitsWithSpaces(digitsSeparatedTrue);
        Lexer digitsFalse = new DigitsWithSpaces(digitsSeparatedFalse);

        // tests for alternate groups
        string alternateTrue = "aa42aa5bb1";
        string alternateFalse = "b22aaa";
        string alternateFalse2 = "4ds2212d4";
        Lexer altTrue = new AlternateGroups(alternateTrue);
        Lexer altFalse = new AlternateGroups(alternateFalse);
        Lexer altFalse2 = new AlternateGroups(alternateFalse2);

        // tests for floating points
        string floatingTrue = "321321.3123123";
        string floatingFalse = ".412";
        Lexer floatTrue = new FloatingPoint(floatingTrue);
        Lexer FloatFalse = new FloatingPoint(floatingFalse);

        // tests for single string;
        string stringTrue = "'dasdasd'";
        string stringFalse = "'dasda'dasd'";
        Lexer strTrue = new SingleString(stringTrue);
        Lexer strFalse = new SingleString(stringFalse);

        // tests for multiline comment
        string commentTrue = "/* sadsadsa */";
        string commentFalse = "/*gfdgf*/agfdgdfgs*/";
        string commentFalse2 = "/*dsadas";
        Lexer commTrue = new MultilineComment(commentTrue);
        Lexer commFalse = new MultilineComment(commentFalse);
        Lexer commFalse2 = new MultilineComment(commentFalse2);

        try
        {
            // расскоментировать строчку для проверки
            // то, что помечено True - должно работать
            // то, что помечено False - должно выдавать ошибку

            //digitsTrue.Parse();
            //digitsFalse.Parse();
            //altTrue.Parse();
            //altFalse.Parse();
            //altFalse2.Parse();
            //floatTrue.Parse();
            //FloatFalse.Parse();
            //strTrue.Parse();
            //strFalse.Parse();
            //commTrue.Parse();
            commFalse.Parse();
            //commFalse2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

    }
}
