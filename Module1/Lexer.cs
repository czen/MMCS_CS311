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
                if (currentCharValue == -1)
                    Error();
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
        System.Console.WriteLine("Letters " + s + " with deliminators are recognized");

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
                if (currentCharValue == -1)
                    Error();
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
        System.Console.WriteLine("\n--- Tests for NumLexer --- ");
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

        string input1_1 = "00124";
        Lexer L1_1 = new NumLexer(input1_1);
        try
        {
            L1_1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        // 2
        System.Console.WriteLine("\n--- Tests for IDLexer --- ");
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

        string input2_1 = "bbbbb";
        Lexer L2_1 = new IDLexer(input2_1);
        try
        {
            L2_1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input2_2 = "3f67gb";
        Lexer L2_2 = new IDLexer(input2_2);
        try
        {
            L2_2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // 3
        System.Console.WriteLine("\n--- Tests for SignLexer --- ");
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

        string input3_1 = "+4561";
        Lexer L3_1 = new SignLexer(input3_1);
        try
        {
            L3_1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input3_2 = "-004";
        Lexer L3_2 = new SignLexer(input3_2);
        try
        {
            L3_2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // 4
        System.Console.WriteLine("\n--- Tests for ALtLexer --- ");
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

        string input4_1 = "b5hh7m";
        Lexer L4_1 = new AltLexer(input4_1);
        try
        {
            L4_1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input4_2 = "4a6n7";
        Lexer L4_2 = new AltLexer(input4_2);
        try
        {
            L4_2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // 5
        System.Console.WriteLine("\n--- Tests for DelimLexer --- ");
        string input5 = "a;a,b;z,r,q";
        Lexer L5 = new DelimLexer(input5);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input5_1 = "b;a,t,,r,w;w";
        Lexer L5_1 = new DelimLexer(input5_1);
        try
        {
            L5_1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input5_2 = "a;";
        Lexer L5_2 = new DelimLexer(input5_2);
        try
        {
            L5_2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // дополнительные
        // 1 
        System.Console.WriteLine("\n--- Tests for NuSpacesLexer --- ");
        string input6 = "1  5 3 3    84";
        Lexer L6 = new NumSpacesLexer(input6);
        try
        {
            L6.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input6_1 = " 5  788  1     3           9";
        Lexer L6_1 = new NumSpacesLexer(input6_1);
        try
        {
            L6_1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input6_2 = "5 3   1 6    ";
        Lexer L6_2 = new NumSpacesLexer(input6_2);
        try
        {
            L6_2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // 2
        System.Console.WriteLine("\n--- Tests for AltElemsLexer --- ");
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

        string input7_1 = "m6a123nm8";
        Lexer L7_1 = new AltElemsLexer(input7_1);
        try
        {
            L7_1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        string input7_2 = "5pp8l12a";
        Lexer L7_2 = new AltElemsLexer(input7_2);
        try
        {
            L7_2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // 3
        System.Console.WriteLine("\n--- Tests for RealLexer --- ");
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

        string input8_1 = "45.124.13";
        Lexer L8_1 = new RealLexer(input8_1);
        try
        {
            L8_1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input8_2 = "012.45";
        Lexer L8_2 = new RealLexer(input8_2);
        try
        {
            L8_2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // 4
        System.Console.WriteLine("\n--- Tests for QuoteLexer --- ");
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

        string input9_1 = "'nn'ehe'srh";
        Lexer L9_1 = new QuoteLexer(input9_1);
        try
        {
            L9_1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input9_2 = "gkdgbk'aqfe'";
        Lexer L9_2 = new QuoteLexer(input9_2);
        try
        {
            L9_2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input9_3 = "'xgfm'fgm";
        Lexer L9_3 = new QuoteLexer(input9_3);
        try
        {
            L9_3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        // 5
        System.Console.WriteLine("\n--- Tests for CommentLexer --- ");
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

        string input10_1 = "/*smrjr*/hjtej*/";
        Lexer L10_1 = new CommentLexer(input10_1);
        try
        {
            L10_1.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input10_2 = "/**/";
        Lexer L10_2 = new CommentLexer(input10_2);
        try
        {
            L10_2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input10_3 = "/**/sndfy";
        Lexer L10_3 = new CommentLexer(input10_3);
        try
        {
            L10_3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

    }
}
