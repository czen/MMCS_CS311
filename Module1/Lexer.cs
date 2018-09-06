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
    protected char currentCh;       // ��������� ��������� ������
    protected int currentCharValue; // ����� �������� ���������� ���������� �������
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


/// ����������� � ��������� ������������� �������� �� ���������� � ������ ������������� ������ ����� � �������������� ��� � ����� 
/// � ����� ������� (��� ������� ������������ �������). ������������� �������� ������� ��������� ����� ������ ������� NextCh ����� 
/// �������. 
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
        string res = ""; //������ ��� ���������� �����
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            res += currentCh;
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
            res += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            res += currentCh;
            NextCh();
        }


        if (currentCharValue != -1) // StringReader ������ -1 � ����� ������
        {
            Error();
        }

        int d = System.Int32.Parse(res); //������� ������ � �����
        Console.WriteLine(d);
    }


}

//�������������
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

        while (char.IsDigit(currentCh) || currentCh == '_' || char.IsLetter(currentCh))
        {

            NextCh();
        }


        if (currentCharValue != -1)
        {
            Error();
        }

        System.Console.WriteLine("Identify: IsCorrect");

    }

}

//����� �� ������, ������������ �� � ����� 0.
public class HardIntLexer : Lexer
{
    protected System.Text.StringBuilder intString;
    public HardIntLexer(string input)
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
       

        if (char.IsDigit(currentCh) && currentCh != '0' )
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


//������������ ����� � �����, ������������ � �����.
public class altDigLets : Lexer
{
    protected System.Text.StringBuilder aString;
    public altDigLets(string input)
        : base(input)
    {
        aString = new System.Text.StringBuilder();
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

        bool digits = true; // ����, ������������  ������ �� ����. ������ ���� ������

        while (char.IsDigit(currentCh) && digits || char.IsLetter(currentCh) && !digits)
        {
            NextCh();
            digits = !digits;
        }


        if (currentCharValue != -1)
        {
            Error();
        }

        System.Console.WriteLine("Letters and numbers alternate correctly!");
    }
}



/// ������ ����, ����������� �������� , ��� ; � �������� �������������� �������� ������ ���� ���������� ������ 
/// ���� � ������ � ����� ����� ������ � ����� ���������
public class ListLetters : Lexer
{
    protected System.Text.StringBuilder lString;
    public ListLetters(string input)
        : base(input)
    {
        lString = new System.Text.StringBuilder();
    }

    //����� ������ �������� �� �������
    private static void PrintList(List<char> l)
    {
        foreach (char c in l) //�������� �� ������ ��� ������ ��� ���������
        {
            Console.Write(c + " ");
        }
        Console.WriteLine();
    }

    public override void Parse()
    {
        List<char> letters = new List<char>();

        NextCh();

        if (char.IsLetter(currentCh))
        {
            letters.Add(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        bool letter = false; //�������� �� ����. ������ ������

        while (char.IsLetter(currentCh) && letter || (currentCh == ',' || currentCh == ';') && !letter) //�������, ��� ������ ����� �.�. ������� �� ������� . ��� ;
        {
            if (char.IsLetter(currentCh))
                letters.Add(currentCh);

            letter = !letter;
            NextCh();
        }

        if (currentCharValue != -1 || letter)
        {
            Error();
        }

        PrintList(letters); //�������� ������ �� �������
    }
}


///----------------------------------------------------------------------------------------------------------
///                                               ���. �������
///----------------------------------------------------------------------------------------------------------


///������ ����, ����������� ����� ��� ����������� ���������. � �������� �������������� �������� ������ ���� 
///���������� ������ ���� � ������ � ����� ����� ������ � ����� ��������� 
public class ListDigits : Lexer
{
    protected System.Text.StringBuilder lString;
    public ListDigits(string input)
        : base(input)
    {
        lString = new System.Text.StringBuilder();
    }

    //������ ������ ����
    private static void PrintList(List<int> l)
    {
        foreach (int c in l)
        {
            Console.Write(c + " ");
        }
        Console.WriteLine();
    }

    public override void Parse()
    {
        List<int> digits = new List<int>();

        NextCh();

        if (char.IsDigit(currentCh))
        {
            digits.Add(System.Int32.Parse(currentCh.ToString()));
            NextCh();
        }
        else
        {
            Error();
        }

        bool digit = false; //����� �� ����. ������ ���� ������

        while (char.IsDigit(currentCh) && digit || currentCh == ' ') //�������, ��� ��� ����� ��������� ����� ����� ���������
        {
            if (char.IsDigit(currentCh))
            {
                digits.Add(System.Int32.Parse(currentCh.ToString()));
                digit = false;
            }

            if (currentCh == ' ')
                digit = true;
            NextCh();
        }

        if (currentCharValue != -1 || digit)
        {
            Error();
        }

        PrintList(digits); //������ ������ �� �������

    }
}



/// ������� ���� aa12c23dd1, � ������� ���������� ������ ���� � ����, � ������ ������ �� ����� 2 ���������. 
/// � �������� �������������� �������� ���������� �������� ������ ������� � ���� ������ 
public class DigLetterGroups2 : Lexer
{
    protected System.Text.StringBuilder DLString;
    public DigLetterGroups2(string input)
        : base(input)
    {
        DLString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string res = "";
        bool isDig = true; //��� �� ����. ������� ������
        int cntGroup = 0; //���-�� ��������� � ������, ������� � 0

        NextCh();

        //�������, ��� ������� ����� ���������� ��� � �����, ��� � � �����
        if (char.IsDigit(currentCh))
        {
            res += currentCh;
            NextCh();
        }
        else
            if (char.IsLetter(currentCh))
        {
            res += currentCh;
            NextCh();
            isDig = false;
        }
        else
        {
            Error();
        }


        while (currentCharValue != -1)
        {
            if (char.IsLetter(currentCh))
            {
                if (isDig)
                {
                    isDig = false;
                    cntGroup = 0; // �������� ���-�� ��-�� � ������, �.�. ��� ��������� 
                }
                else
                {
                    cntGroup += 1; // ���� ���������� ������ ��� ������, �� ����. �������
                    if (cntGroup >= 2)
                        Error();
                }

                res += currentCh;
                NextCh();
            }
            else
                if (char.IsDigit(currentCh))
            {
                if (!isDig)
                {
                    isDig = true;
                    cntGroup = 0;
                }
                else
                {
                    cntGroup += 1;
                    if (cntGroup > 1)
                        Error();
                }

                res += currentCh;
                NextCh();
            }
            else
                Error();

        }

        Console.WriteLine(res);
        Console.WriteLine();
    }
}


///������������ � ���������� ������ 123.45678
public class DoubleLexer : Lexer
{
    protected System.Text.StringBuilder DString;
    public DoubleLexer(string input) : base(input)
    {
        DString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        bool point = false; //��� �� ����. ������ ������
        bool end_point = false; //��� �� ����. ������ ������

        if (char.IsDigit(currentCh))
        {
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh) || currentCh == '.' && !point)
        {
            if (currentCh == '.')
                point = true;
            else
                if (point)
                end_point = true;

            NextCh();
        }

        if (currentCharValue != -1 || !point || !end_point) //���� ����� �� ���� �� ����, �� ������
        {
            Error();
        }

        Console.WriteLine("Double: IsCorrect");
        Console.WriteLine();
    }
}


/// ������� ���� '������', ������ ���������� ����������� ������ ' 
public class StringLexer : Lexer
{
    protected System.Text.StringBuilder SLString;
    public StringLexer(string input) : base(input)
    {
        SLString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        bool apst = false; //��� �� ��� ������ ��������

        if (currentCh == '\'')
        {
            NextCh();
        }
        else
        {
            Error();
        }

        while (currentCh != '\'' && currentCharValue != -1)
        {
            NextCh();
        }

        if (currentCh == '\'')
        {
            apst = true;
            NextCh();
        }

        if (currentCharValue != -1 || !apst)
        {
            Error();
        }

        Console.WriteLine("String: IsCorrect");
        Console.WriteLine();
    }
}


/// ������� ���� /*�����������*/, ������ ����������� �� ����� ����������� ������������������ �������� */
public class CommentLexer : Lexer
{
    protected System.Text.StringBuilder ComString;
    public CommentLexer(string input) : base(input)
    {
        ComString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        int st = 0; //��� �������� ���� �� ������ * � /

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

        while (currentCharValue != -1)
        {
            if (currentCh == '*')
            {
                if (st == 0) //���� ��������� �� ���� �����, �� ����. ������� 
                    st = 1;
            }
            else
                if (currentCh == '/')
            {
                if (st == 1)
                {
                    st = 2;
                    NextCh();
                    break; //���� ������ ��������� * � /, �� �������
                }
            }
            else
                if (st != 0)
            {
                st = 0;
            }

            NextCh();
        }

        if (currentCharValue != -1 || st != 2)
        {
            Error();
        }

        Console.WriteLine("Comment: IsCorrect");
        Console.WriteLine();
    }
}

// ������� ���. �������


///������� ���� Id1.Id2.Id3 (���������� ��������������� ����� ���� ������������).
//���������, ��� ��������������� > 0.
public class IdentifiersLexer : Lexer
{
    protected System.Text.StringBuilder IdString;

    public IdentifiersLexer(string input) : base(input)
    {
        IdString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        bool point = false; // ���  �� ����. ������ ������
        NextCh();

        if (char.IsLetter(currentCh))
        {
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsLetter(currentCh) || char.IsDigit(currentCh) || currentCh == '.' || currentCh == '_')
        {
            if ((char.IsDigit(currentCh) || currentCh == '.' || currentCh == '_') && point)
            {
                Error();
            }

            if (currentCh == '.')
                point = true;
            else
                if (point)
            {
                point = false;
            }

            NextCh();
        }

        if (currentCharValue != -1 || point)
        {
            Error();
        }

        Console.WriteLine("Identifiers are correct");
        Console.WriteLine();
    }

}

//             ����� + �������� ���������

public class Program
{
    public static void test1()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Task1: test");
        Console.WriteLine();

        string input1 = "154216";
        string input2 = "+12";
        string input3 = "1542r123";

        Lexer L1 = new IntLexer(input1);
        Lexer L2 = new IntLexer(input2);
        Lexer L3 = new IntLexer(input3);

        try
        {
            L1.Parse();
            L2.Parse();
            L3.Parse();

        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input4 = "";
        Lexer L4 = new IntLexer(input4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        Console.WriteLine();

    }

    public static void test2()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Task2: test");
        Console.WriteLine();

        string input1 = "e154216";
        string input2 = "e1_ss2d";
        string input3 = "d";
        string input4 = "ddddd";
        string input5 = "";

        Lexer L1 = new IdLexer(input1);
        Lexer L2 = new IdLexer(input2);
        Lexer L3 = new IdLexer(input3);
        Lexer L4 = new IdLexer(input4);
        Lexer L5 = new IdLexer(input5);


        try
        {
            L1.Parse();
            L2.Parse();
            L3.Parse();
            L4.Parse();
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        Console.WriteLine();
    }

    public static void test3()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Task3: test");
        Console.WriteLine();

        string input1 = "+154216";
        string input2 = "-12";
        string input3 = "1542123";

        Lexer L1 = new HardIntLexer(input1);
        Lexer L2 = new HardIntLexer(input2);
        Lexer L3 = new HardIntLexer(input3);

        try
        {
            L1.Parse();
            L2.Parse();
            L3.Parse();

        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input4 = "";
        Lexer L4 = new HardIntLexer(input4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        Console.WriteLine();

    }

    public static void test4()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Task4: test");
        Console.WriteLine();

        string input1 = "s5s6a1w3y";
        string input2 = "w1d4";
        string input3 = "";

        Lexer L1 = new altDigLets(input1);
        Lexer L2 = new altDigLets(input2);
        Lexer L3 = new altDigLets(input3);

        try
        {
            L1.Parse();
            L2.Parse();
            L3.Parse();

        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input4 = "w3e4r6tt7y";
        Lexer L4 = new altDigLets(input4);
        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        Console.WriteLine();

        string input5 = "6t5q0w0";
        Lexer L5 = new altDigLets(input5);
        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
        Console.WriteLine();

    }

    public static void test5()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Task5: test");
        Console.WriteLine();


        string input1 = "m,r,t,y,u,l";
        string input2 = "w,r,t;y,u;l";
        string input3 = "a,r,t;";


        Lexer L1 = new ListLetters(input1);
        Lexer L2 = new ListLetters(input2);
        Lexer L3 = new ListLetters(input3);


        try
        {
            L1.Parse();
            L2.Parse();
            L3.Parse();

        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        string input4 = "e,r,1,y,3,l";
        Lexer L4 = new ListLetters(input4);

        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input5 = "";
        Lexer L5 = new ListLetters(input5);

        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            Console.WriteLine(e.Message);
        }
        Console.WriteLine();
    }



    //����� ��� ��� �������


    public static void add_test1()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Additional task 1.");
        Console.WriteLine();


        string input1 = "1  2 3   4             5";
        string input2 = "2 3 1 2 0";
        string input3 = "2 1  3 ";


        Lexer L1 = new ListDigits(input1);
        Lexer L2 = new ListDigits(input2);
        Lexer L3 = new ListDigits(input3);


        try
        {
            L1.Parse();
            L2.Parse();
            L3.Parse();

        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        string input4 = "1  3 e  1  4";
        Lexer L4 = new ListDigits(input4);

        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input5 = "";
        Lexer L5 = new ListDigits(input5);

        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            Console.WriteLine(e.Message);
        }

        string input6 = "1  3 4 21  4";
        Lexer L6 = new ListDigits(input6);

        try
        {
            L6.Parse();
        }
        catch (LexerException e)
        {
            Console.WriteLine(e.Message);
        }

        Console.WriteLine();
    }


    public static void add_test2()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Additional task 2.");
        Console.WriteLine();


        string input1 = "a12sd65f41";
        string input2 = "12yr5t67iu";
        string input3 = "12er5tyu1";


        Lexer L1 = new DigLetterGroups2(input1);
        Lexer L2 = new DigLetterGroups2(input2);
        Lexer L3 = new DigLetterGroups2(input3);


        try
        {
            L1.Parse();
            L2.Parse();
            L3.Parse();

        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        string input4 = "er56t789";
        Lexer L4 = new DigLetterGroups2(input4);

        try
        {
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input5 = "";
        Lexer L5 = new DigLetterGroups2(input5);

        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            Console.WriteLine(e.Message);
        }

        Console.WriteLine();
    }


    public static void add_test3()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Additional task 3.");
        Console.WriteLine();


        string input1 = "12.234";
        string input2 = "0.14";
        string input3 = "12.4";
        string input4 = "123";

        Lexer L1 = new DoubleLexer(input1);
        Lexer L2 = new DoubleLexer(input2);
        Lexer L3 = new DoubleLexer(input3);
        Lexer L4 = new DoubleLexer(input4);

        try
        {
            L1.Parse();
            L2.Parse();
            L3.Parse();
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        string input5 = "12.345e678";
        Lexer L5 = new DoubleLexer(input5);

        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input6 = "";
        Lexer L6 = new DoubleLexer(input6);

        try
        {
            L6.Parse();
        }
        catch (LexerException e)
        {
            Console.WriteLine(e.Message);
        }

        string input7 = "1.";
        Lexer L7 = new DoubleLexer(input7);

        try
        {
            L7.Parse();
        }
        catch (LexerException e)
        {
            Console.WriteLine(e.Message);
        }

        Console.WriteLine();
    }


    public static void add_test4()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Additional task 4.");
        Console.WriteLine();


        string input1 = "'string'";
        string input2 = "'cv12..56e'";
        string input3 = "'re/f.,fw  jfb,'";
        string input4 = "'cvfdrg''";

        Lexer L1 = new StringLexer(input1);
        Lexer L2 = new StringLexer(input2);
        Lexer L3 = new StringLexer(input3);
        Lexer L4 = new StringLexer(input4);

        try
        {
            L1.Parse();
            L2.Parse();
            L3.Parse();
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        string input5 = ",'dvgb'";
        Lexer L5 = new StringLexer(input5);

        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input6 = "";
        Lexer L6 = new StringLexer(input6);

        try
        {
            L6.Parse();
        }
        catch (LexerException e)
        {
            Console.WriteLine(e.Message);
        }

        string input7 = "'";
        Lexer L7 = new StringLexer(input7);

        try
        {
            L7.Parse();
        }
        catch (LexerException e)
        {
            Console.WriteLine(e.Message);
        }

        Console.WriteLine();
    }


    public static void add_test5()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Additional task 5.");
        Console.WriteLine();


        string input1 = "/*gddjhsgfhjdgfgs*/";
        string input2 = "/*etert2f78.//**/";
        string input3 = "/*rteyre***n*/";
        string input4 = "/*hffdjsr**";

        Lexer L1 = new CommentLexer(input1);
        Lexer L2 = new CommentLexer(input2);
        Lexer L3 = new CommentLexer(input3);
        Lexer L4 = new CommentLexer(input4);

        try
        {
            L1.Parse();
            L2.Parse();
            L3.Parse();
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        string input5 = "/*ertyw*/jf";
        Lexer L5 = new CommentLexer(input5);

        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input6 = "";
        Lexer L6 = new CommentLexer(input6);

        try
        {
            L6.Parse();
        }
        catch (LexerException e)
        {
            Console.WriteLine(e.Message);
        }

        string input7 = "4/*lkjkj*/";
        Lexer L7 = new CommentLexer(input7);

        try
        {
            L7.Parse();
        }
        catch (LexerException e)
        {
            Console.WriteLine(e.Message);
        }

        Console.WriteLine();
    }


    public static void hard_task_test()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Hard additional task");
        Console.WriteLine();


        string input1 = "A12_34.Tfjh.T.a_12.T_.Edh";
        string input2 = "a1r_23";
        string input3 = "Aikur4.tew12r";
        string input4 = "asEr.1kdc.Efu5";

        Lexer L1 = new IdentifiersLexer(input1);
        Lexer L2 = new IdentifiersLexer(input2);
        Lexer L3 = new IdentifiersLexer(input3);
        Lexer L4 = new IdentifiersLexer(input4);

        try
        {
            L1.Parse();
            L2.Parse();
            L3.Parse();
            L4.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        string input5 = "Ert3.rtuw.";
        Lexer L5 = new IdentifiersLexer(input5);

        try
        {
            L5.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        string input6 = "";
        Lexer L6 = new IdentifiersLexer(input6);

        try
        {
            L6.Parse();
        }
        catch (LexerException e)
        {
            Console.WriteLine(e.Message);
        }

        Console.WriteLine();
    }


    public static void Main()
    {
        test1();
        test2();
        test3();
        test4();
        test5();

        //���. �������
        add_test1();
        add_test2();
        add_test3();
        add_test4();
        add_test5();

        //������� ���. �������
        hard_task_test();
    }
}
