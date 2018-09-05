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

    public virtual void Parse() {}
}

public class IntLexer : Lexer
{
    protected System.Text.StringBuilder intString;
	public int res = 0;
	private int sign = 1;

    public IntLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+')
			NextCh();
		else if (currentCh == '-')
        {
            sign = -1;
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
			res = (int)(currentCh - '0');
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
			res = res * 10 + (int)(currentCh - '0');
            NextCh();
        }


        if (currentCharValue != -1) 
        {
            Error();
        }
		
		res *= sign;
		System.Console.WriteLine(System.String.Format("Integer is recognized: {0}", res));
    }
}

public class IdLexer : Lexer
{
    protected System.Text.StringBuilder idString;

    public IdLexer(string input) : base(input)
    {
        idString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (char.IsLetter(currentCh))
            NextCh();
        else
            Error();

        while (char.IsLetter(currentCh) || char.IsDigit(currentCh) || currentCh == '_')
            NextCh();

        if (currentCharValue != -1)
            Error();

        System.Console.WriteLine("Id is recognized");
    }
}

public class IntLexer1 : Lexer
{
    protected System.Text.StringBuilder intString;

    public IntLexer1(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
            NextCh();

        if (currentCh != '0' && char.IsDigit(currentCh))
            NextCh();
        else
            Error();

        while (char.IsDigit(currentCh))
            NextCh();

        if (currentCharValue != -1)
            Error();

        System.Console.WriteLine("Integer is recognized");
    }
}

public class ChIntLexer : Lexer
{
    protected System.Text.StringBuilder intString;

    public ChIntLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (char.IsLetter(currentCh))
            NextCh();
        else
            Error();

		bool ch = true;
		
        while ((char.IsDigit(currentCh) && ch) || (char.IsLetter(currentCh) && !ch)){
            NextCh();
			ch = !ch;
		}

        if (currentCharValue != -1)
            Error();

        System.Console.WriteLine("String is recognized");
    }
}

public class LettersLexer : Lexer{
	protected System.Text.StringBuilder intString;
	public List<char> chars;
	
	public LettersLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
		chars = new List<char>();
    }
	
	public override void Parse()
    {
		NextCh();
		if(char.IsLetter(currentCh))
		{
			chars.Add(currentCh);
			NextCh();
		}
		else
			Error();
		
		bool ch = true;
		while((char.IsLetter(currentCh) && !ch) || ((currentCharValue == ',' || currentCharValue == ';') && ch)){
			ch = !ch;
			if(char.IsLetter(currentCh))
				chars.Add(currentCh);
			NextCh();
		}
		
		if (currentCharValue != -1)
            Error();
			
		System.Console.WriteLine(System.String.Format("Letters are recognized: {0}", System.String.Join(" ", chars.ToArray())));
	} 
}


public class DigitsLexer : Lexer{
	protected System.Text.StringBuilder intString;
	public List<int> dig;
	
	public DigitsLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
		dig = new List<int>();
    }
	
	public override void Parse()
    {
		NextCh();
		bool ws = true;
		
		while(currentCharValue != -1){
			if(char.IsDigit(currentCh)){
				if (!ws) 
					Error();
				dig.Add((int)(currentCh - '0'));
				NextCh();
				ws = false;
			}
			else if(char.IsWhiteSpace(currentCh)){
				NextCh();
				ws = true;
				}
			else
				Error();
		}
		
		if (dig.Count == 0)
			Error();
		
		System.Console.WriteLine(System.String.Format("Digits are recognized: {0}", System.String.Join(" ", dig.ToArray())));
	}
}

public class GroupsLexer : Lexer{
	protected System.Text.StringBuilder intString;
	
	public GroupsLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }
	
	public override void Parse()
    {
		int cnt = 0;
		bool gr;
		NextCh();
		
		if(currentCharValue == -1)
			Error();
		
		gr = char.IsLetter(currentCh);
		
		while(currentCharValue != -1){
			if(!char.IsLetterOrDigit(currentCh))
				Error();
			if(char.IsLetter(currentCh) == gr){
				++cnt;
				if(cnt > 2)
					Error();
			}
			else{
				cnt = 1;
				gr = !gr;
			}
			intString.Append(currentCh);
			NextCh();
		}
		
		System.Console.WriteLine(System.String.Format("Groups are recognized: {0}", intString.ToString()));
	}
	
}

public class RealLexer : Lexer{
	protected System.Text.StringBuilder intString;
	
	public RealLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }
	
	public override void Parse()
    {
		NextCh();
        if (currentCh == '+' || currentCh == '-')
            NextCh();
			
        if (char.IsDigit(currentCh))
            NextCh();
        else
            Error();

        while (char.IsDigit(currentCh))
            NextCh();
		
		if(currentCh == '.'){
			NextCh();
			if(!char.IsDigit(currentCh))
				Error();
			
			while(char.IsDigit(currentCh))
				NextCh();
			
			if (currentCharValue != -1) 
           		Error();
		}
		else if (currentCharValue != -1) 
            Error();
		
        System.Console.WriteLine("Real is recognized");
	}
}

public class StringLexer : Lexer{
	protected System.Text.StringBuilder intString;
	
	public StringLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }
	
	public override void Parse()
    {
		NextCh();
		if(currentCh != '\'')
			Error();
		
		NextCh();
		while(currentCh != '\'' && currentCharValue != -1)
			NextCh();
			
		if(currentCh != '\'')
			Error();
		
		NextCh();
		if(currentCharValue != -1)
			Error();
			
		System.Console.WriteLine("String is recognized");
	}
}

public class CommentLexer : Lexer{
	protected System.Text.StringBuilder intString;
	
	public CommentLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }
	
	public override void Parse()
    {
		NextCh();
		if(currentCh != '/')
			Error();
		
		NextCh();
		if(currentCh != '*')
			Error();
		
		NextCh();
		while(currentCharValue != -1){
			if(currentCh == '*'){
				NextCh();
				if(currentCh == '/'){
					NextCh();
					if(currentCharValue != -1)
						Error();
					else break;
				}
			}
			NextCh();
		}
			
		System.Console.WriteLine("Comment is recognized");
	}
}


public class Program
{
	
	static void test(string input, Lexer L){
		try
        {
            L.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
	}
	
    public static void Main()
    {
		System.Console.WriteLine("-------------- Tests for IntLexer --------------");
		string input1 = "", input2 = "abc", input3 = "67-4", input4 = "-42", input5 = "1298";
		test(input1, new IntLexer(input1));
		test(input2, new IntLexer(input2));
		test(input3, new IntLexer(input3));
		test(input4, new IntLexer(input4));
		test(input5, new IntLexer(input5));
		
		System.Console.WriteLine("-------------- Tests for IdLexer --------------");
		input2 = "666";
		input3 = "1ab_c";
		input4 = "d";
		input5 = "hi_555";
		string input6 = "nc_+l";
		test(input1, new IdLexer(input1));
		test(input2, new IdLexer(input2));
		test(input3, new IdLexer(input3));
		test(input4, new IdLexer(input4));
		test(input5, new IdLexer(input5));
		test(input6, new IdLexer(input6));
		
		System.Console.WriteLine("-------------- Tests for IntLexer1 --------------");
		input2 = "032";
		input3 = "-023";
		input4 = "6aa5a";
		input5 = "117804";
		input6 = "-3222";
		test(input1, new IntLexer1(input1));
		test(input2, new IntLexer1(input2));
		test(input3, new IntLexer1(input3));
		test(input4, new IntLexer1(input4));
		test(input5, new IntLexer1(input5));
		test(input6, new IntLexer1(input6));
		
		System.Console.WriteLine("-------------- Tests for ChIntLexer --------------");
		input2 = "1";
		input3 = "a99bc";
		input4 = "a5_v6";
		input5 = "w";
		input6 = "r2d2";
		test(input1, new ChIntLexer(input1));
		test(input2, new ChIntLexer(input2));
		test(input3, new ChIntLexer(input3));
		test(input4, new ChIntLexer(input4));
		test(input5, new ChIntLexer(input5));
		test(input6, new ChIntLexer(input6));
		
		System.Console.WriteLine("-------------- Tests for LettersLexer --------------");
		input2 = "42";
		input3 = ",c,";
		input4 = "a,a,aaa";
		input5 = "a,b;c;,d";
		input6 = "a,b,c;d,e";
		string input7 = "c;a,t;s,";
		test(input1, new LettersLexer(input1));
		test(input2, new LettersLexer(input2));
		test(input3, new LettersLexer(input3));
		test(input4, new LettersLexer(input4));
		test(input5, new LettersLexer(input5));
		test(input6, new LettersLexer(input6));
		test(input7, new LettersLexer(input7));
		
		System.Console.WriteLine("-------------- Tests for DigitsLexer --------------");
		input2 = "  ";
		input3 = "k9";
		input4 = "5, 6";
		input5 = "6 71 2";
		input6 = " 5   0 8  2 ";
		input7 = "1 2 3";
		test(input1, new DigitsLexer(input1));
		test(input2, new DigitsLexer(input2));
		test(input3, new DigitsLexer(input3));
		test(input4, new DigitsLexer(input4));
		test(input5, new DigitsLexer(input5));
		test(input6, new DigitsLexer(input6));
		test(input7, new DigitsLexer(input7));
		
		System.Console.WriteLine("-------------- Tests for GroupsLexer --------------");
		input2 = "111";
		input3 = "kkkk";
		input4 = "2 a 2";
		input5 = "11aa222b";
		input6 = "cc33t5";
		input7 = "11aa2bb33c";
		test(input1, new GroupsLexer(input1));
		test(input2, new GroupsLexer(input2));
		test(input3, new GroupsLexer(input3));
		test(input4, new GroupsLexer(input4));
		test(input5, new GroupsLexer(input5));
		test(input6, new GroupsLexer(input6));
		test(input7, new GroupsLexer(input7));
		
		System.Console.WriteLine("-------------- Tests for RealLexer --------------");
		input2 = "45.";
		input3 = ".1";
		input4 = "-54.e";
		input5 = "9.123";
		input6 = "-777";
		input7 = "0.05";
		test(input1, new RealLexer(input1));
		test(input2, new RealLexer(input2));
		test(input3, new RealLexer(input3));
		test(input4, new RealLexer(input4));
		test(input5, new RealLexer(input5));
		test(input6, new RealLexer(input6));
		test(input7, new RealLexer(input7));
		
		System.Console.WriteLine("-------------- Tests for StringLexer --------------");
		input2 = "ccc";
		input3 = "'i'm'";
		input4 = "'abc";
		input5 = "''";
		input6 = "'hello'";
		input7 = "'  42  '";
		test(input1, new StringLexer(input1));
		test(input2, new StringLexer(input2));
		test(input3, new StringLexer(input3));
		test(input4, new StringLexer(input4));
		test(input5, new StringLexer(input5));
		test(input6, new StringLexer(input6));
		test(input7, new StringLexer(input7));
		
		System.Console.WriteLine("-------------- Tests for CommentLexer --------------");
		input2 = "/*111*/1*/";
		input3 = "abc/**/";
		input4 = "/*mrk*/100";
		input5 = "/*nic/*e*/";
		input6 = "/* abcd */";
		test(input1, new CommentLexer(input1));
		test(input2, new CommentLexer(input2));
		test(input3, new CommentLexer(input3));
		test(input4, new CommentLexer(input4));
		test(input5, new CommentLexer(input5));
		test(input6, new CommentLexer(input6));
    }
}
