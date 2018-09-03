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


public class Program
{
	public static void TestIntLexer()
	{
        System.Console.WriteLine("--------------- TestIntLexer ---------------");

        string[] strings = { "0", "10", "100523", "a", "" };

		foreach (string str in strings)
		{
			string input = str;
            IntLexer L = new IntLexer(input);
			try
			{
                System.Console.WriteLine("String is <" + input + ">");
                L.Parse();
                System.Console.WriteLine("Integer is " + L.GetNum());
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
                System.Console.WriteLine("String is id = " + L.GetIsId());
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

        string[] strings = { "-1", "+1", "1", "123523", "-213556", "-0", "0", "+0", "abc" };

        foreach (string str in strings)
        {
            string input = str;
            TrueIntLexer L = new TrueIntLexer(input);
            try
            {
                System.Console.WriteLine("String is <" + input + ">");
                L.Parse();
                System.Console.WriteLine("String is true int = " + L.GetIsTrueInt());
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
    }
}