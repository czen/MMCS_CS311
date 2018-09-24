using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Xml.Serialization;

// ReSharper disable All

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

public class IntLexer : Lexer
{
    protected System.Text.StringBuilder intString;
    protected int parseResult;
    
    public int ParseResult
    {
        get { return parseResult; }
    }
    
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
            intString.Append(currentCh);
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
            intString.Append(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            intString.Append(currentCh);
            NextCh();
        }


        if (currentCharValue != -1)
        {
            Error();
        }

        parseResult = int.Parse(intString.ToString());
    }

    public static bool IsCorrect()
    {
        string str1 = "12345";
        IntLexer l1 = new IntLexer(str1);
        l1.Parse();
        string str2 = "-10002";
        IntLexer l2 = new IntLexer(str2);
        l2.Parse();
        string str3 = "+01";
        IntLexer l3 = new IntLexer(str3);
        l3.Parse();
        try
        {
            string str4 = "123adsd**";
            IntLexer l4 = new IntLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {
            
        }
        try
        {
            string str4 = "-";
            IntLexer l4 = new IntLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {
            
        }

        return  l1.ParseResult == int.Parse(str1) &&
                l2.ParseResult == int.Parse(str2) &&
                l3.ParseResult == int.Parse(str3);
    }
}

public class IdentLexer : Lexer
{
    private string parseResult;
    protected StringBuilder builder;
    
    public string ParseResult
    {
        get { return parseResult; }
    }
    
    public IdentLexer(string input) : base(input)
    {
        builder = new StringBuilder();
    }

    public override void Parse()
    { 
        NextCh();
        if (char.IsLetter(currentCh) || currentCh=='_')
        {
            builder.Append(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsLetterOrDigit(currentCh) || currentCh=='_')
        {
            builder.Append(currentCh);
            NextCh();
        }
        if (currentCharValue != -1)
        {
            Error();
        }

        parseResult = builder.ToString();
    }
    
    public static bool IsCorrect()
    {
        string str1 = "variable";
        IdentLexer l1 = new IdentLexer(str1);
        l1.Parse();
        string str2 = "_a";
        IdentLexer l2 = new IdentLexer(str2);
        l2.Parse();
        string str3 = "asd_asd_1";
        IdentLexer l3 = new IdentLexer(str3);
        l3.Parse();
        try
        {
            string str4 = "1var";
            IdentLexer l4 = new IdentLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {
            
        }
        try
        {
            string str4 = "";
            IdentLexer l4 = new IdentLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {
            
        }

        return  l1.ParseResult == str1 &&
                l2.ParseResult == str2 &&
                l3.ParseResult == str3;
    }
}

public class ImprovedIntLexer : IntLexer
{
    public ImprovedIntLexer(string input)
        : base(input)
    {
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            intString.Append(currentCh);
            NextCh();
        }
        
        if (char.IsDigit(currentCh) && currentCh != '0')
        {
            intString.Append(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            intString.Append(currentCh);
            NextCh();
        }


        if (currentCharValue != -1)
        {
            Error();
        }

        parseResult = int.Parse(intString.ToString());
        //System.Console.WriteLine("Integer is recognized");
    }

    public new static bool IsCorrect()
    {
        string str1 = "12345";
        ImprovedIntLexer l1 = new ImprovedIntLexer(str1);
        l1.Parse();
        string str2 = "-10002";
        ImprovedIntLexer l2 = new ImprovedIntLexer(str2);
        l2.Parse();
        string str3 = "+12";
        ImprovedIntLexer l3 = new ImprovedIntLexer(str3);
        l3.Parse();
        try
        {
            string str4 = "-023";
            ImprovedIntLexer l4 = new ImprovedIntLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {
            
        }
        try
        {
            string str4 = "0123";
            ImprovedIntLexer l4 = new ImprovedIntLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {
            
        }

        return  l1.ParseResult == int.Parse(str1) &&
                l2.ParseResult == int.Parse(str2) &&
                l3.ParseResult == int.Parse(str3);
    }
}

public class LetterDigitLexer : Lexer
{
    protected StringBuilder builder;
    protected string parseResult;

    public string ParseResult
    {
        get { return parseResult; }
    }

    public LetterDigitLexer(string input)
        : base(input)
    {
        builder = new StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        while (true)
        {
            if (char.IsLetter(currentCh))
            {
                builder.Append(currentCh);
                NextCh();
            }
            else
            {
                Error();
            }

            if (currentCharValue == -1)
            {
                break;
            }

            if (char.IsDigit(currentCh))
            {
                builder.Append(currentCh);
                NextCh();
            }
            else
            {
                Error();
            }

            if (currentCharValue == -1)
            {
                break;
            }
        }

        parseResult = builder.ToString();
    }

    public static bool IsCorrect()
    {
        string str1 = "a1b2c3d";
        LetterDigitLexer l1 = new LetterDigitLexer(str1);
        l1.Parse();
        string str2 = "a";
        LetterDigitLexer l2 = new LetterDigitLexer(str2);
        l2.Parse();
        string str3 = "a1";
        LetterDigitLexer l3 = new LetterDigitLexer(str3);
        l3.Parse();
        try
        {
            string str4 = "1a2s";
            LetterDigitLexer l4 = new LetterDigitLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {
            
        }
        try
        {
            string str4 = "a1vc2d";
            LetterDigitLexer l4 = new LetterDigitLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {
            
        }

        return  l1.ParseResult == str1 &&
                l2.ParseResult == str2 &&
                l3.ParseResult == str3;
    }
}

public class LetterListLexer : Lexer
{
    protected List<char> parseResult;

    public List<char> ParseResult
    {
        get { return parseResult; }
    }

    public LetterListLexer(string input)
        : base(input)
    {
        parseResult = new List<char>();
    }

    public override void Parse()
    {
        NextCh();
        if (char.IsLetter(currentCh))
        {
            parseResult.Add(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }
        while (currentCh == ',' || currentCh == ';')
        {
            NextCh();
            if (char.IsLetter(currentCh))
            {
                parseResult.Add(currentCh);
                NextCh();
            }
            else
            {
                Error();
            }
        }
        if (currentCharValue != -1)
        {
            Error();
        }
    }

    public static bool IsCorrect()
    {
        string str1 = "a";
        LetterListLexer l1 = new LetterListLexer(str1);
        l1.Parse();
        string str2 = "a,b,c;d,e;f;g,h";
        LetterListLexer l2 = new LetterListLexer(str2);
        l2.Parse();
        string str3 = "y;n";
        LetterListLexer l3 = new LetterListLexer(str3);
        l3.Parse();
        try
        {
            string str4 = "a,b,c,";
            LetterListLexer l4 = new LetterListLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {
            
        }
        try
        {
            string str4 = "a,b,cd";
            LetterListLexer l4 = new LetterListLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {
            
        }

        return  l1.ParseResult.SequenceEqual(new List<char>{'a'}) &&
                l2.ParseResult.SequenceEqual(new List<char>{'a','b','c','d','e','f','g','h'}) &&
                l3.ParseResult.SequenceEqual(new List<char>{'y','n'});
    }
}

public class DigitListLexer : Lexer
{
    protected List<int> parseResult;

    public List<int> ParseResult
    {
        get { return parseResult; }
    }

    public DigitListLexer(string input)
        : base(input)
    {
        parseResult = new List<int>();
    }

    public override void Parse()
    {
        NextCh();
        while (true)
        {
            if (char.IsDigit(currentCh))
            {
                parseResult.Add((int)char.GetNumericValue(currentCh));
                NextCh();
            }
            else
            {
                Error();
            }
            
            if (currentCharValue == -1)
            {
                break;
            }

            if (currentCh != ' ')
            {
                Error();
            }
            while (currentCh == ' ')
            {
                NextCh();
            }
        }
    }

    public static bool IsCorrect()
    {
        string str1 = "1 2 3 4 5";
        DigitListLexer l1 = new DigitListLexer(str1);
        l1.Parse();
        string str2 = "0          0";
        DigitListLexer l2 = new DigitListLexer(str2);
        l2.Parse();
        string str3 = "0    1                   2 3 4 5 6 7 8            9                                    0";
        DigitListLexer l3 = new DigitListLexer(str3);
        l3.Parse();
        try
        {
            string str4 = "0 1 2 3  ";
            DigitListLexer l4 = new DigitListLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {
            
        }
        try
        {
            string str4 = "0           1         a";
            DigitListLexer l4 = new DigitListLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {
            
        }
        try
        {
            string str4 = "0           134        12 3 4 5";
            DigitListLexer l4 = new DigitListLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {
            
        }

        return  l1.ParseResult.SequenceEqual(new List<int>{1, 2, 3, 4, 5}) &&
                l2.ParseResult.SequenceEqual(new List<int>{0, 0}) &&
                l3.ParseResult.SequenceEqual(new List<int>{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0});
    }
}

public class ImprovedLetterDigitLexer : LetterDigitLexer
{
    public ImprovedLetterDigitLexer(string input)
        : base(input)
    {
    }

    private void digitPart()
    {
        if (char.IsDigit(currentCh))
        {
            builder.Append(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        if (char.IsDigit(currentCh))
        {
            builder.Append(currentCh);
            NextCh();
        }
    }

    private void letterPart()
    {
        if (char.IsLetter(currentCh))
        {
            builder.Append(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        if (char.IsLetter(currentCh))
        {
            builder.Append(currentCh);
            NextCh();
        }
    }

    public override void Parse()
    {
        NextCh();
        while (true)
        {
            letterPart();
            if (currentCharValue == -1)
            {
                break;
            }

            digitPart();
            if (currentCharValue == -1)
            {
                break;
            }
        }

        parseResult = builder.ToString();
    }

    public new static bool IsCorrect()
    {
        string str1 = "aa12c23dd1";
        ImprovedLetterDigitLexer l1 = new ImprovedLetterDigitLexer(str1);
        l1.Parse();
        string str2 = "a1b1c1de22gh33h2h1";
        ImprovedLetterDigitLexer l2 = new ImprovedLetterDigitLexer(str2);
        l2.Parse();
        string str3 = "a11";
        ImprovedLetterDigitLexer l3 = new ImprovedLetterDigitLexer(str3);
        l3.Parse();
        try
        {
            string str4 = "a1b2cde3a1";
            ImprovedLetterDigitLexer l4 = new ImprovedLetterDigitLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {

        }

        try
        {
            string str4 = "1b2d3a";
            ImprovedLetterDigitLexer l4 = new ImprovedLetterDigitLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {

        }

        try
        {
            string str4 = "aa1bb22e*33dd";
            ImprovedLetterDigitLexer l4 = new ImprovedLetterDigitLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {

        }

        return l1.ParseResult == str1 &&
               l2.ParseResult == str2 &&
               l3.ParseResult == str3;
    }
}

public class DoubleLexer : Lexer
{
    private StringBuilder builder;
    private double parseResult;

    public double ParseResult
    {
        get { return parseResult; }

    }

    public DoubleLexer(string input)
        : base(input)
    {
        builder = new StringBuilder();
    }

    private void unsignedIntPart()
    {
        if (char.IsDigit(currentCh))
        {
            builder.Append(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            builder.Append(currentCh);
            NextCh();
        }
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            builder.Append(currentCh);
            NextCh();
        }

        unsignedIntPart();
        if (currentCh == '.')
        {
            builder.Append(currentCh);
            NextCh();
            unsignedIntPart();
        }

        if (currentCharValue != -1)
        {
            Error();
        }

        parseResult = double.Parse(builder.ToString());
    }

    public static bool IsCorrect()
    {
        string str1 = "123215";
        DoubleLexer l1 = new DoubleLexer(str1);
        l1.Parse();
        string str2 = "+128372.000004";
        DoubleLexer l2 = new DoubleLexer(str2);
        l2.Parse();
        string str3 = "-0001.0001";
        DoubleLexer l3 = new DoubleLexer(str3);
        l3.Parse();
        try
        {
            string str4 = "-123.123.123";
            DoubleLexer l4 = new DoubleLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {

        }

        try
        {
            string str4 = "123.8j123";
            DoubleLexer l4 = new DoubleLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {

        }

        try
        {
            string str4 = "12321..787";
            DoubleLexer l4 = new DoubleLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {

        }

        return l1.ParseResult == Double.Parse(str1) &&
               l2.ParseResult == Double.Parse(str2) &&
               l3.ParseResult == Double.Parse(str3);
    }
}

public class StringLexer : Lexer
{
    private StringBuilder builder;
    private string parseResult;

    public string ParseResult
    {
        get { return parseResult; }

    }

    public StringLexer(string input)
        : base(input)
    {
        builder = new StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh != '\'')
        {
            Error();
        }

        NextCh();
        while (currentCh != '\'')
        {
            if (currentCharValue == -1)
            {
                Error();
            }

            builder.Append(currentCh);
            NextCh();
        }

        NextCh();
        if (currentCharValue != -1)
        {
            Error();
        }

        parseResult = builder.ToString();
    }

    public static bool IsCorrect()
    {
        string str1 = "'asdqwe'";
        StringLexer l1 = new StringLexer(str1);
        l1.Parse();
        string str2 = "'***asasda****'";
        StringLexer l2 = new StringLexer(str2);
        l2.Parse();
        string str3 = "''";
        StringLexer l3 = new StringLexer(str3);
        l3.Parse();
        try
        {
            string str4 = "'asdasd'q''";
            StringLexer l4 = new StringLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {

        }

        try
        {
            string str4 = "a'qwe'";
            StringLexer l4 = new StringLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {

        }

        try
        {
            string str4 = "'ashdh";
            StringLexer l4 = new StringLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {

        }

        return l1.ParseResult == "asdqwe" &&
               l2.ParseResult == "***asasda****" &&
               l3.ParseResult == "";
    }
}

public class CommentLexer : Lexer
{
    private StringBuilder builder;
    private string parseResult;

    public string ParseResult
    {
        get { return parseResult; }

    }

    public CommentLexer(string input)
        : base(input)
    {
        builder = new StringBuilder();
    }

    public override void Parse()
    {
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
        
        while (true)
        {
            if (currentCharValue == -1)
            {
                Error();
            }
            if (currentCh == '*')
            {
                NextCh();
                if (currentCharValue == -1)
                {
                    Error();
                }
                if (currentCh == '/')
                {
                    NextCh();
                    break;
                }

                builder.Append('*');
                builder.Append(currentCh);
                NextCh();
                continue;
            }
            builder.Append(currentCh);
            NextCh();
        }
        
        if (currentCharValue != -1)
        {
            Error();
        }

        parseResult = builder.ToString();
    }

    public static bool IsCorrect()
    {
        string str1 = "/*asdqwe*/";
        CommentLexer l1 = new CommentLexer(str1);
        l1.Parse();
        string str2 = "/****asa**s//da*****/";
        CommentLexer l2 = new CommentLexer(str2);
        l2.Parse();
        string str3 = "/**/";
        CommentLexer l3 = new CommentLexer(str3);
        l3.Parse();
        try
        {
            string str4 = "/*asdasdad";
            CommentLexer l4 = new CommentLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {

        }

        try
        {
            string str4 = "/*    *   */*/";
            CommentLexer l4 = new CommentLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {

        }

        try
        {
            string str4 = "/asasd*/";
            CommentLexer l4 = new CommentLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {

        }

        return l1.ParseResult == "asdqwe" &&
               l2.ParseResult == "***asa**s//da****" &&
               l3.ParseResult == "";
    }
}

public class IdentChainLexer : Lexer
{
    private StringBuilder builder;
    private List<string> parseResult;

    public List<string> ParseResult
    {
        get { return parseResult; }

    }

    public IdentChainLexer(string input)
        : base(input)
    {
        builder = new StringBuilder();
        parseResult = new List<string>();
    }

    private void parseIdent()
    {
        if (char.IsLetter(currentCh) || currentCh=='_')
        {
            builder.Append(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsLetterOrDigit(currentCh) || currentCh=='_')
        {
            builder.Append(currentCh);
            NextCh();
        }
        
        parseResult.Add(builder.ToString());
        builder.Clear();
    }
    
    public override void Parse()
    {
        NextCh();
        parseIdent();
        while (currentCh == '.')
        {
            NextCh();
            parseIdent();
        }

        if (currentCharValue != -1)
        {
            Error();
        }
    }

    public static bool IsCorrect()
    {
        string str1 = "id1.id2.id3";
        IdentChainLexer l1 = new IdentChainLexer(str1);
        l1.Parse();
        string str2 = "id1";
        IdentChainLexer l2 = new IdentChainLexer(str2);
        l2.Parse();
        string str3 = "_var_1.var_2._var3._____var444444.a.b.c.d.e.f";
        IdentChainLexer l3 = new IdentChainLexer(str3);
        l3.Parse();
        try
        {
            string str4 = "id1.id2.id3.";
            IdentChainLexer l4 = new IdentChainLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {

        }

        try
        {
            string str4 = ".id1.id2";
            IdentChainLexer l4 = new IdentChainLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {

        }

        try
        {
            string str4 = "id1..id2.id3";
            IdentChainLexer l4 = new IdentChainLexer(str4);
            l4.Parse();
            return false;
        }
        catch (LexerException e)
        {

        }

        return l1.ParseResult.SequenceEqual(new List<string> {"id1", "id2", "id3"}) &&
               l2.ParseResult.SequenceEqual(new List<string> {"id1"}) &&
               l3.ParseResult.SequenceEqual(
                   new List<string> {"_var_1", "var_2", "_var3", "_____var444444", "a", "b", "c", "d", "e", "f"});
    }
}

public class Program
{
    public static void CheckCorrect(bool isCorrect, Type classType)
    {
        if (isCorrect)
        {
            Console.WriteLine(classType + ": Tests passed");
        }
        else
        {
            Console.WriteLine(classType + ": Failed running tests");
        }
    }

    public static void Main()
    {
        /*
         * I. Основные задания
         */
        /*
         * 1) IntLexer tests
         */
        var isCorrect = IntLexer.IsCorrect();
        CheckCorrect(isCorrect, typeof(IntLexer));

        /*
         * 2) IdentLexer tests
         */
        isCorrect = IdentLexer.IsCorrect();
        CheckCorrect(isCorrect, typeof(IdentLexer));

        /*
         * 3) ImprovedIntLexer tests
         */
        isCorrect = ImprovedIntLexer.IsCorrect();
        CheckCorrect(isCorrect, typeof(ImprovedIntLexer));

        /*
         * 4) LetterDigitLexer tests
         */
        isCorrect = LetterDigitLexer.IsCorrect();
        CheckCorrect(isCorrect, typeof(LetterDigitLexer));

        /*
         * 5) LetterListLexer tests
         */
        isCorrect = LetterListLexer.IsCorrect();
        CheckCorrect(isCorrect, typeof(LetterListLexer));

        /*
         * II. Дополнительные задания
         */
        /*
         * 1) DigitListLexer tests
         */
        isCorrect = DigitListLexer.IsCorrect();
        CheckCorrect(isCorrect, typeof(DigitListLexer));

        /*
         * 2) ImprovedLetterDigitLexer tests
         */
        isCorrect = ImprovedLetterDigitLexer.IsCorrect();
        CheckCorrect(isCorrect, typeof(ImprovedLetterDigitLexer));
        
        /*
         * 3) DoubleLexer tests
         */
        isCorrect = DoubleLexer.IsCorrect();
        CheckCorrect(isCorrect, typeof(DoubleLexer));
        
        /*
         * 4) StringLexer tests
         */
        isCorrect = StringLexer.IsCorrect();
        CheckCorrect(isCorrect, typeof(StringLexer));
        
        /*
         * 5) CommentLexer tests
         */
        isCorrect = CommentLexer.IsCorrect();
        CheckCorrect(isCorrect, typeof(CommentLexer));
        
        /*
         * III. Дополнительные сложные задания
         */
        /*
         * 1) IdentChainLexer tests
         */
        isCorrect = IdentChainLexer.IsCorrect();
        CheckCorrect(isCorrect, typeof(IdentChainLexer));
    }
}