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

    public virtual bool Parse()
    {
        return false;
    }
}

public class IntLexer : Lexer
{

    protected System.Text.StringBuilder intString;
    public int result;
    protected int sign;
    public IntLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override bool Parse()
    {
        NextCh();
        if (currentCh == '+' || char.IsDigit(currentCh))
        {
            sign = 1;
            result = char.IsDigit(currentCh) ? (int)(currentCh - '0'): 0;
            NextCh();
        } 
        else if (currentCh == '-' || currentCharValue != -1)
        {
            sign = -1;
            result = 0;
            NextCh();
        }
        else
        {
            Error();
        }



        while (char.IsDigit(currentCh))
        {
            result = result*10 + sign * (int)(currentCh - '0');
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
            return false;
        }

        System.Console.WriteLine("Integer is recognized");
        return true;
    }
}


public class Program
{
    public static void Main()
    {
        string input = "-";
        IntLexer L = new IntLexer(input);
        try
        {
            System.Console.WriteLine(L.Parse()); 
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

    }
}