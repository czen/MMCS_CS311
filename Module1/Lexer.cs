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

/**
Реализовать в программе семантические действия по накоплению в строке распознанного целого числа 
    и преобразованию его в целое в конце разбора (при встрече завершающего символа). 
    Семантические действия следует добавлять перед каждым вызовом NextCh кроме первого. 
     */
public class IntLexer : Lexer
{

    protected System.Text.StringBuilder intString;
    protected System.String numStr;
    protected int num;

    public IntLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public string GetString()
    {
        return numStr;
    }

    public int GetNum()
    {
        return num;
    }

    public override void Parse()
    {
        NextCh();
		numStr = "";
        numStr += currentCh;
        if (currentCh == '+' || currentCh == '-')
        {
            NextCh();
            numStr += currentCh;
        }

        if (char.IsDigit(currentCh))
        {
            NextCh();
            numStr += currentCh;
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
            NextCh();
            numStr += currentCh;
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }
		
		numStr = numStr.Remove(numStr.Length - 1);
		//System.Console.WriteLine("String is " + numStr);
		
        num = System.Convert.ToInt32(numStr); 
        //System.Console.WriteLine("Integer is " + num);
    }
}

/**
Идентификатор.
     */
public class IdLexer : Lexer
{

    protected System.Text.StringBuilder idString;
    protected bool isId;

    public IdLexer(string input)
        : base(input)
    {
        idString = new System.Text.StringBuilder();
    }

    public bool GetIsId()
    {
        return isId;
    }

    public override void Parse()
    {
        isId = false;

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

        isId = true;
    }
}

/**
Целое со знаком, начинающееся не с цифры 0.
     */
public class TrueIntLexer : Lexer
{

    protected System.Text.StringBuilder trueIntString;
    protected bool isTrueInt;

    public TrueIntLexer(string input)
        : base(input)
    {
        trueIntString = new System.Text.StringBuilder();
    }

    public bool GetIsTrueInt()
    {
        return isTrueInt;
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

        isTrueInt = true;
    }
}

/**
Чередующиеся буквы и цифры, начинающиеся с буквы. 
     */
public class LetterDegitLetterLexer : Lexer
{

    protected System.Text.StringBuilder inString;
    protected bool lastLetter;
    protected bool isLetterDigitLetter;

    public LetterDegitLetterLexer(string input)
        : base(input)
    {
        inString = new System.Text.StringBuilder();
    }

    public bool GetIsLetterDigitLetter()
    {
        return isLetterDigitLetter;
    }


    public override void Parse()
    {
        NextCh();

        if (char.IsLetter(currentCh))
        {
            NextCh();
        }
        else
            Error();

        lastLetter = true;

        while ((char.IsDigit(currentCh) && lastLetter) 
            || (char.IsLetter(currentCh) && !lastLetter))
        {
            NextCh();
            lastLetter = !lastLetter;
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        isLetterDigitLetter = true;
    }
}

/**
Список букв, разделенных символом , или ; 
    В качестве семантического действия должно быть накопление 
    списка букв в списке и вывод этого списка в конце программы.
     */
public class LetterCommaLetterLexer : Lexer
{

    protected System.Text.StringBuilder inString;
    protected bool lastLetter;
    protected bool isLetterCommaLetter;

    public LetterCommaLetterLexer(string input)
        : base(input)
    {
        inString = new System.Text.StringBuilder();
    }

    public bool GetIsLetterCommaLetter()
    {
        return isLetterCommaLetter;
    }


    public override void Parse()
    {
        NextCh();

        if (char.IsLetter(currentCh))
        {
            NextCh();
        }
        else
            Error();

        lastLetter = true;

        while (lastLetter && (currentCh == ',' || currentCh == ';')
            || (!lastLetter && char.IsLetter(currentCh)))
        {
            NextCh();
            lastLetter = !lastLetter;
        }

        if (!lastLetter || currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        isLetterCommaLetter = true;
    }
}

/**
Список цифр, разделенных одним или несколькими пробелами. 
    В качестве семантического действия должно быть накопление 
    списка цифр в списке и вывод этого списка в конце программы
     */
public class DigitSpacesDigitLexer : Lexer
{

    protected System.Text.StringBuilder inString;
    protected bool lastDigit;
    protected bool isDigitSpacesDigit;
    protected string listDigits;

    public DigitSpacesDigitLexer(string input)
        : base(input)
    {
        inString = new System.Text.StringBuilder();
    }

    public bool GetIsDigitSpacesDigit()
    {
        return isDigitSpacesDigit;
    }

    public string GetListDigits()
    {
        return listDigits;
    }

    public override void Parse()
    {
        NextCh();

        if (char.IsDigit(currentCh))
        {
            listDigits += currentCh.ToString() + ' ';
            NextCh();
        }
        else
            Error();

        lastDigit = true;

        while (char.IsDigit(currentCh) && !lastDigit || currentCh == ' ')
        {
            if (currentCh == ' ')
                lastDigit = false;
            if (char.IsDigit(currentCh))
            {
                lastDigit = true;
                listDigits += currentCh.ToString() + ' ';
            }
            NextCh();
        }

        if (!lastDigit || currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        isDigitSpacesDigit = true;
    } 
}

/**
Лексема вида aa12c23dd1, в которой чередуются группы букв и цифр, 
    в каждой группе не более 2 элементов. 
    В качестве семантического действия необходимо накопить данную лексему в виде строки
     */
public class DigitsLettersLess2Lexer : Lexer
{

    protected System.Text.StringBuilder inString;
    protected int cntDigits = 0;
    protected int cntLetters = 0;
    protected bool isDigitsLettersLess2;
    protected string digitsLetters;

    public DigitsLettersLess2Lexer(string input)
        : base(input)
    {
        inString = new System.Text.StringBuilder();
    }

    public bool GetIsDigitsLettersLess2()
    {
        return isDigitsLettersLess2;
    }

    public string GetDigitsLetters()
    {
        return digitsLetters;
    }

    public override void Parse()
    {
        NextCh();

        if (char.IsDigit(currentCh))
        {
            digitsLetters += currentCh;
            cntDigits = 1;
            NextCh();
        }
        else
            if (char.IsLetter(currentCh))
            {
                digitsLetters += currentCh;
                cntLetters = 1;
                NextCh();
            }
            else
                Error();

        while (char.IsDigit(currentCh) || char.IsLetter(currentCh))
        {
            if (char.IsDigit(currentCh))
            {
                digitsLetters += currentCh;
                cntDigits += 1;
                cntLetters = 0;
            }
            if (char.IsLetter(currentCh))
            {
                digitsLetters += currentCh;
                cntDigits = 0;
                cntLetters += 1;
            }

            if (cntLetters == 3 || cntDigits == 3)
                Error();

            NextCh();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        isDigitsLettersLess2 = true;
    }
}

/**
Вещественное с десятичной точкой 123.45678
     */
public class DoubleLexer : Lexer
{

    protected System.Text.StringBuilder inString;
    protected bool dotIsFound;
    protected bool digitAfterDot;
    protected bool isDouble;
    protected string Double;

    public DoubleLexer(string input)
        : base(input)
    {
        inString = new System.Text.StringBuilder();
    }

    public bool GetIsDouble()
    {
        return isDouble;
    }

    public string GetDouble()
    {
        return Double;
    }

    public override void Parse()
    {
        NextCh();

        if (char.IsDigit(currentCh))
        {
            Double += currentCh;
            NextCh();
        }
        else
            Error();

        while (char.IsDigit(currentCh) || currentCh == '.')
        {
            if (currentCh == '.' && dotIsFound)
            {
                Error();
            }
            if (currentCh == '.')
            {
                Double += currentCh;
                dotIsFound = true;
            }
            if (char.IsDigit(currentCh))
            {
                if (dotIsFound)
                    digitAfterDot = true;
                Double += currentCh;
            }
            
            NextCh();
        }

        if (dotIsFound && !digitAfterDot || !dotIsFound)
        {
            Error();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        isDouble = true;
    }
}

/**
Лексема вида 'строка', внутри апострофов отсутствует символ '
     */
public class ApostrophesLexer : Lexer
{

    protected System.Text.StringBuilder inString;
    protected int cntApostrophes;
    protected bool lastApostroph;
    protected bool isApostrophes;
    protected string stringBetween;

    public ApostrophesLexer(string input)
        : base(input)
    {
        inString = new System.Text.StringBuilder();
    }

    public bool GetIsApostrophes()
    {
        return isApostrophes;
    }

    public string GetStringBetween()
    {
        return stringBetween;
    }

    public override void Parse()
    {
        NextCh();

        if (currentCh.ToString() == "'")
        {
            cntApostrophes = 1;
            lastApostroph = true;
            NextCh();
        }
        else
        {
            Error();
        }

        while (currentCharValue != -1)
        {
            if (currentCh.ToString() == "'")
            {
                cntApostrophes += 1;
                lastApostroph = true;
            }
            else
            {
                stringBetween += currentCh;
                lastApostroph = false;
            }

            if (cntApostrophes == 2 && !lastApostroph)
                Error();

            NextCh();
        }

        if (cntApostrophes != 2 || !lastApostroph)
            Error();

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        isApostrophes = true;
    }
}


///Лексема вида /*комментарий*/, 
///внутри комментария не может встретиться последовательность символов*/ 
public class CommentLexer : Lexer
{

    protected System.Text.StringBuilder inString;
    protected bool lastStar;
    protected bool lastSlash;
    protected bool isComment;
    protected string stringBetween;

    public CommentLexer(string input)
        : base(input)
    {
        inString = new System.Text.StringBuilder();
    }

    public bool GetIsComment()
    {
        return isComment;
    }

    public string GetStringBetween()
    {
        return stringBetween;
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

        while (currentCharValue != -1)
        {
            if (lastStar && lastSlash)
                Error();

            if (currentCh == '*')
            {
                lastStar = true;
                lastSlash = false;
                NextCh();
                continue;
            }
            if (currentCh == '/' && lastStar)
            {
                lastSlash = true;
                NextCh();
                continue;
            }

            if (lastStar)
                stringBetween += '*';

            lastStar = false;
            lastSlash = false;
            stringBetween += currentCh;

            NextCh();
        }

        if (!lastStar || !lastSlash)
            Error();

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        isComment = true;
    }
}

/**
Лексема вида Id1.Id2.Id3 (количество идентификаторов может быть произвольным).
     */
public class IdDotIdLexer : Lexer
{

    protected System.Text.StringBuilder inString;
    protected bool lastDot;
    protected bool lastLetter;
    protected bool isIdDotId;

    public IdDotIdLexer(string input)
        : base(input)
    {
        inString = new System.Text.StringBuilder();
    }

    public bool GetIsIdDotId()
    {
        return isIdDotId;
    }

    public override void Parse()
    {
        NextCh();

        if (char.IsLetter(currentCh))
        {
            NextCh();
            lastLetter = true;
        }
        else
        {
            Error();
        }

        while (currentCharValue != -1)
        {
            if (lastDot)
            {
                if (currentCh == '.')
                    Error();
                if (!char.IsLetter(currentCh))
                    Error();

                lastLetter = true;
                lastDot = false;
                NextCh();
                continue;
            }

            if (currentCh == '.')
            {
                lastDot = true;
                NextCh();
                continue;
            }

            NextCh();
        }

        if (lastDot)
            Error();

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        isIdDotId = true;
    }
}


public class Program
{
	public static void TestIntLexer()
	{
        System.Console.WriteLine("              --------------- TestIntLexer ---------------");

        string[] strings = { "0", "10", "100523", "1g3", "a", "" };

		foreach (string str in strings)
		{
			string input = str;
            IntLexer L = new IntLexer(input);
			try
			{
                System.Console.WriteLine("String is <" + input + ">");
                L.Parse();
                System.Console.WriteLine("    Integer is " + L.GetNum());
            }
			catch (LexerException e)
			{
				System.Console.WriteLine(e.Message);
			}
            System.Console.WriteLine("------------------------------");
        }
        //System.Threading.Thread.Sleep(10000);
	}

    public static void TestIdLexer()
    {
        System.Console.WriteLine("              --------------- TestIdLexer ---------------");

        string[] strings = { "g", "a0", "a10", "b1b0b0b5ASAS2AS45465456SDSAD3", "0", "" };

        foreach (string str in strings)
        {
            string input = str;
            IdLexer L = new IdLexer(input);
            try
            {
                System.Console.WriteLine("String is <" + input + ">");
                L.Parse();
                System.Console.WriteLine("    isId = " + L.GetIsId());
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
            System.Console.WriteLine("------------------------------");
        }
        //System.Threading.Thread.Sleep(10000);
    }

    public static void TestTrueIntLexer()
    {
        System.Console.WriteLine("              --------------- TestTrueIntLexer ---------------");

        string[] strings = { "-1", "+1", "1", "123523", "-213556", "-0678", "0899", "+098", "abc" };

        foreach (string str in strings)
        {
            string input = str;
            TrueIntLexer L = new TrueIntLexer(input);
            try
            {
                System.Console.WriteLine("String is <" + input + ">");
                L.Parse();
                System.Console.WriteLine("    isTrueInt = " + L.GetIsTrueInt());
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
            System.Console.WriteLine("------------------------------");
        }
        //System.Threading.Thread.Sleep(10000);
    }

    public static void TestLetterDigitLetterLexer()
    {
        System.Console.WriteLine("              --------------- TestLetterDigitLetterLexer ---------------");

        string[] strings = { "a", "a0", "a0b", "a1b2", "a12bf123ccc", "0", "1", "1a", "" };

        foreach (string str in strings)
        {
            string input = str;
            LetterDegitLetterLexer L = new LetterDegitLetterLexer(input);
            try
            {
                System.Console.WriteLine("String is <" + input + ">");
                L.Parse();
                System.Console.WriteLine("    isLetterDigitLetter = " + L.GetIsLetterDigitLetter());
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
            System.Console.WriteLine("------------------------------");
        }
        //System.Threading.Thread.Sleep(10000);
    }

    public static void TestLetterCommaLetterLexer()
    {
        System.Console.WriteLine("              --------------- TestLetterCommaLetterLexer ---------------");

        string[] strings = { "a", "a,a", "a;b", "a;b,c", "a,", "a,b;", ",", "" };

        foreach (string str in strings)
        {
            string input = str;
            LetterCommaLetterLexer L = new LetterCommaLetterLexer(input);
            try
            {
                System.Console.WriteLine("String is <" + input + ">");
                L.Parse();
                System.Console.WriteLine("    isLetterCommaLetter = " + L.GetIsLetterCommaLetter());
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
            System.Console.WriteLine("------------------------------");
        }
        //System.Threading.Thread.Sleep(10000);
    }

    public static void TestDigitSpacesDigitLexer()
    {
        System.Console.WriteLine("              --------------- TestDigitSpacesDigitLexer ---------------");

        string[] strings = { "1", "1 2", "2  5", "4  6    9", "4   ", "1 2   ", " ", "" };

        foreach (string str in strings)
        {
            string input = str;
            DigitSpacesDigitLexer L = new DigitSpacesDigitLexer(input);
            try
            {
                System.Console.WriteLine("String is <" + input + ">");
                L.Parse();
                System.Console.WriteLine("    isDigitSpacesDigit = " + L.GetIsDigitSpacesDigit());
                System.Console.WriteLine("    ListDigits = " + L.GetListDigits());
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
            System.Console.WriteLine("------------------------------");
        }
        //System.Threading.Thread.Sleep(10000);
    }

    public static void TestDigitsLettersLess2Lexer()
    {
        System.Console.WriteLine("              --------------- TestDigitsLettersLess2Lexer ---------------");

        string[] strings = { "1", "1a1a1a", "1a2bn55", "FG52VV3G6HH", "fgh52gg", "a121an23", "a1b2hhm", "" };

        foreach (string str in strings)
        {
            string input = str;
            DigitsLettersLess2Lexer L = new DigitsLettersLess2Lexer(input);
            try
            {
                System.Console.WriteLine("String is <" + input + ">");
                L.Parse();
                System.Console.WriteLine("    isDigitsLettersLess2 = " + L.GetIsDigitsLettersLess2());
                System.Console.WriteLine("    DigitsLetters = " + L.GetDigitsLetters());
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
            System.Console.WriteLine("------------------------------");
        }
        //System.Threading.Thread.Sleep(10000);
    }

    public static void TestDoubleLexer()
    {
        System.Console.WriteLine("              --------------- TestDoubleLexer ---------------");

        string[] strings = { "1.0", "10.3", "136.96", "963.125", "12.56.3", ".16", "45.", "25", "" };

        foreach (string str in strings)
        {
            string input = str;
            DoubleLexer L = new DoubleLexer(input);
            try
            {
                System.Console.WriteLine("String is <" + input + ">");
                L.Parse();
                System.Console.WriteLine("    isDouble = " + L.GetIsDouble());
                System.Console.WriteLine("    Double = " + L.GetDouble());
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
            System.Console.WriteLine("------------------------------");
        }
        //System.Threading.Thread.Sleep(10000);
    }

    public static void TestApostrophesLexer()
    {
        System.Console.WriteLine("              --------------- TestApostrophesLexer ---------------");

        string[] strings = { "''", "'a'", "'ff555'", "'f5fd5dff5'", "'fddfsfd'5d5", "'fggdfff", "fgfgfg'", "dfdf", "" };

        foreach (string str in strings)
        {
            string input = str;
            ApostrophesLexer L = new ApostrophesLexer(input);
            try
            {
                System.Console.WriteLine("String is <" + input + ">");
                L.Parse();
                System.Console.WriteLine("    isApostrophes = " + L.GetIsApostrophes());
                System.Console.WriteLine("    StringBetween = " + L.GetStringBetween());
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
            System.Console.WriteLine("------------------------------");
        }
        //System.Threading.Thread.Sleep(10000);
    }

    public static void TestCommentLexer()
    {
        System.Console.WriteLine("              --------------- TestCommentLexer ---------------");

        string[] strings = { "/**/", "/*a/*b*/", "/*a52g*/", "/*qw/e*r/ty*/", "/*qwerty", "qwerty*/", "qwerty", "/*qwer*/ty*/", "" };

        foreach (string str in strings)
        {
            string input = str;
            CommentLexer L = new CommentLexer(input);
            try
            {
                System.Console.WriteLine("String is <" + input + ">");
                L.Parse();
                System.Console.WriteLine("    isApostrophes = " + L.GetIsComment());
                System.Console.WriteLine("    StringBetween = " + L.GetStringBetween());
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
            System.Console.WriteLine("------------------------------");
        }
        //System.Threading.Thread.Sleep(10000);
    }

    public static void TestIdDotIdLexer()
    {
        System.Console.WriteLine("              --------------- TestIdDotIdLexer ---------------");

        string[] strings = { "a12.ff4.gg55gg", "df56", "qw34.rty456", "q.w.e.r.t.y", "qw.", "qw.12.as", "1w.qw34",
            "qw12.er34.56", ".qw", "" };

        foreach (string str in strings)
        {
            string input = str;
            IdDotIdLexer L = new IdDotIdLexer(input);
            try
            {
                System.Console.WriteLine("String is <" + input + ">");
                L.Parse();
                System.Console.WriteLine("    isIdDotId = " + L.GetIsIdDotId());
            }
            catch (LexerException e)
            {
                System.Console.WriteLine(e.Message);
            }
            System.Console.WriteLine("------------------------------");
        }
        //System.Threading.Thread.Sleep(10000);
    }

    public static void Main()
    {
        TestIntLexer();
        TestIdLexer();
        TestTrueIntLexer();
        TestLetterDigitLetterLexer();
        TestLetterCommaLetterLexer();

        TestDigitSpacesDigitLexer();
        TestDigitsLettersLess2Lexer();
        TestDoubleLexer();
        TestApostrophesLexer();
        TestCommentLexer();

        TestIdDotIdLexer();
    }
}