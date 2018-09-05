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

public class Program
{
	public static void TestIntLexer()
	{
        System.Console.WriteLine("--------------- TestIntLexer ---------------");

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
			
		}
        //System.Threading.Thread.Sleep(10000);
	}

    public static void TestIdLexer()
    {
        System.Console.WriteLine("--------------- TestIdLexer ---------------");

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

        }
        //System.Threading.Thread.Sleep(10000);
    }

    public static void TestTrueIntLexer()
    {
        System.Console.WriteLine("--------------- TestTrueIntLexer ---------------");

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

        }
        //System.Threading.Thread.Sleep(10000);
    }

    public static void TestLetterDigitLetterLexer()
    {
        System.Console.WriteLine("--------------- TestLetterDigitLetterLexer ---------------");

        string[] strings = { "a", "a0", "a0b", "a1b2", "0", "1", "1a", "" };

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

        }
        //System.Threading.Thread.Sleep(10000);
    }

    public static void TestLetterCommaLetterLexer()
    {
        System.Console.WriteLine("--------------- TestLetterCommaLetterLexer ---------------");

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

        }
        //System.Threading.Thread.Sleep(10000);
    }

    public static void TestDigitSpacesDigitLexer()
    {
        System.Console.WriteLine("--------------- TestDigitSpacesDigitLexer ---------------");

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
    }
}