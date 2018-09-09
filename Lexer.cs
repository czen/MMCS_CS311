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
        if (char.IsLetter(currentCh))
        {
            intString.Append(currentCh);
            NextCh();
        }
        else
        {
            Error();
        }

        while (currentCh != ';')
        {

            if (currentCh == ',')
            {
                intString.Append('\n');
                NextCh();
                if (!char.IsLetter(currentCh))
                    break;
            }
            else
            {
                intString.Append(currentCh);
                NextCh();
            }
            
        }

        NextCh();
        if (currentCharValue != -1) // StringReader ������ -1 � ����� ������
        {
            Error();
        }

        System.Console.WriteLine("letters are recognized");
        System.Console.WriteLine(intString);
    }
}


public class Program
{
    public static void Main()
    {
        string input = System.Console.ReadLine();
        Lexer L = new IntLexer(input);
        try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

    }
}