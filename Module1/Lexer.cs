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

//Реализовать в программе семантические действия по накоплению в строке распознанного
//целого числа и преобразованию его в целое в конце разбора(при встрече завершающего символа).
public class NumLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public NumLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string x = "";
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {

            x += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {

            x += currentCh;
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        int xx = Convert.ToInt32(x, 10);

        System.Console.WriteLine("Integer " + xx + " is recognized");

    }
}

// Идентификатор
public class IDLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public IDLexer(string input)
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

        while (char.IsLetterOrDigit(currentCh))
        {
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("ID is recognized");

    }
}

// Целое со знаком, начинающееся не с цифры 0
public class SignLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public SignLexer(string input)
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

        System.Console.WriteLine("Integer with sign is recognized");

    }
}

// Чередующиеся буквы и цифры, начинающиеся с буквы
public class AltLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public AltLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        bool was_num = false; 

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
            if (char.IsDigit(currentCh) && !was_num)
            {
                NextCh();
                was_num = true;
            }
            else
                if (char.IsLetter(currentCh) && was_num)
            {
                NextCh();
                was_num = false;
            }
            else
            {
                Error();
            }

        }

        System.Console.WriteLine("Alternating letters/numbers are recognized");

    }
}

// Список букв, разделенных символом , или ; В качестве семантического действия должно быть 
// накопление списка букв в списке и вывод этого списка в конце программы
public class DelimLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public DelimLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        bool was_letter = true;
        List<char> res = new List<char>();

        if (char.IsLetter(currentCh))
        {
            res.Add(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        while (currentCharValue != -1)
        {
            if ((currentCh == ',' || currentCh == ';') && was_letter)
            {
                NextCh();
                was_letter = false;
            } 
            else
                if (char.IsLetter(currentCh) && !was_letter)
            {
                res.Add(currentCh);
                NextCh();
                was_letter = true;
            }
            else
            {
                Error();
            }

        }

        string s = new string(res.ToArray());
        System.Console.WriteLine("Letters " + s + " with deliminators are recognized" );

    }
}

// Список цифр, разделенных одним или несколькими пробелами.
public class NumSpacesLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public NumSpacesLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        List<char> res = new List<char>();
        if (char.IsDigit(currentCh))
        {
            res.Add(currentCh);
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
                res.Add(currentCh);
                NextCh();
            }
            else if (currentCh == ' ')
            {
                NextCh();
            }
            else
            {
                Error();
            }
        }

        string s = new string(res.ToArray());
        System.Console.WriteLine("Integers with spaces " + s + " are recognized");

    }
}

// Лексема вида aa12c23dd1, в которой чередуются группы букв и цифр, в каждой группе не более 2 элементов
public class AltElemsLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public AltElemsLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        int cntc = 0;
        int cntd = 0;
        List<char> res = new List<char>();

        if (char.IsLetter(currentCh))
        {
            res.Add(currentCh);
            NextCh();
            cntc++;
        }
        else
        {
            Error();
        }

        while (currentCharValue != -1)
        {
            if (char.IsDigit(currentCh) && cntd < 2)
            {
                res.Add(currentCh);
                NextCh();
                cntd++;
                cntc = 0;
            }
            else
                if (char.IsLetter(currentCh) && cntc < 2)
            {
                res.Add(currentCh);
                NextCh();
                cntc++;
                cntd = 0;
            }
            else
            {
                Error();
            }

        }

        string s = new string(res.ToArray());
        System.Console.WriteLine("Alternating letters/numbers groups " + s + " are recognized");

    }
}

// Вещественное с десятичной точкой
public class RealLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public RealLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        bool dot = false;
        if (char.IsDigit(currentCh))
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
            {
                NextCh();
            }
            else if (currentCh == '.' && !dot)
            {
                NextCh();
                dot = true;
            }
            else
            {
                Error();
            }
        }

        System.Console.WriteLine("Real with dot is recognized");

    }
}

// Лексема вида 'строка', внутри апострофов отсутствует символ '
public class QuoteLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public QuoteLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if ((int)currentCh == 39)
        {
            NextCh();
        }
        else
        {
            Error();
        }

        while (currentCharValue != -1)
        {
            if ((int)currentCh != 39)
            {
                NextCh();
            }
            else 
            {
                NextCh();
                if (currentCharValue != -1)
                    Error();
            }

            
        }

        System.Console.WriteLine("String between quotes is recognized");

    }
}

// Лексема вида /*комментарий*/, внутри комментария не может встретиться последовательность символов */ 
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
            if (currentCharValue == '*')
                NextCh();
        }
        else
        {
            Error();
        }

        while (currentCharValue != -1)
        {
            if (currentCh != '*')
            {
                NextCh();
            }
            else
            {
                NextCh();
                if (currentCh == '/')
                {
                    NextCh();
                    if (currentCharValue != -1)
                        Error();
                }
            }
         
        }

        System.Console.WriteLine("Comment is recognized");

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

        // 1
        string input1 = "654721";
        Lexer L1 = new NumLexer(input1);
        try
        {
            L1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        // 2
        string input2 = "a154q216";
        Lexer L2 = new IDLexer(input2);
        try
        {
            L2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // 3
        string input3 = "-721";
        Lexer L3 = new SignLexer(input3);
        try
        {
            L3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // 4
        string input4 = "a4a7b4";
        Lexer L4 = new AltLexer(input4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // 5
        string input5 = "a;a,b;z,r,q;";
        Lexer L5 = new DelimLexer(input5);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // дополнительные
        // 1 
        string input6 = "1  5 3 3    84 ";
        Lexer L6 = new NumSpacesLexer(input6);
        try
        {
            L6.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // 2
        string input7 = "aa12c23dd1";
        Lexer L7 = new AltElemsLexer(input7);
        try
        {
            L7.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // 3
        string input8 = "21.455";
        Lexer L8 = new RealLexer(input8);
        try
        {
            L8.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // 4
        string input9 = "'meow42'";
        Lexer L9 = new QuoteLexer(input9);
        try
        {
            L9.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // 5
        string input10 = "/*pu pu* pu*/";
        Lexer L10 = new CommentLexer(input10);
        try
        {
            L10.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

    }
}
