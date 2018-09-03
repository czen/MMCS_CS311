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
		result = 0;
		sign = 0;
        NextCh(); // at first char
        if (currentCh == '+' || currentCh == '-' )
        {
            sign = currentCh == '-' ? -1 : 1;
            NextCh();
			if(char.IsDigit(currentCh))
			 result = sign * (int)(currentCh - '0');
			else 
				return false;
        } 
        else if (char.IsDigit(currentCh)) {
			sign = 1;
			result  = (int)(currentCh - '0');
			NextCh();
		}
		else
			return false;
		
        while (char.IsDigit(currentCh))
        {
            result = result*10 + sign * (int)(currentCh - '0');
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            
            return false;
        }

        //System.Console.WriteLine("Integer is recognized");
        return true;
    }
}

public class NoZeroIntLexer : Lexer
{

    protected System.Text.StringBuilder intString;
    public int result;
    protected int sign;
    public NoZeroIntLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override bool Parse()
    {
		result = 0;
		sign = 0;
        NextCh(); // at first char
        if (currentCh == '+' || currentCh == '-' )
        {
            sign = currentCh == '-' ? -1 : 1;
            NextCh();
			if(char.IsDigit(currentCh) && currentCh != '0')
			 result = sign * (int)(currentCh - '0');
			else 
				return false;
        } 
        else if (char.IsDigit(currentCh) && currentCh != '0' ) {
			sign = 1;
			result  = (int)(currentCh - '0');
			NextCh();
		}
		else
			return false;
        while (char.IsDigit(currentCh))
        {
            result = result*10 + sign * (int)(currentCh - '0');
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            
            return false;
        }

       
        return true;
    }
}

public class IdLexer : Lexer
{

    protected System.Text.StringBuilder intString;
    public int result;
    protected int sign;
    public IdLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override bool Parse()
    {
		
        NextCh();
        if (char.IsLetter(currentCh))
			NextCh();
		else 
			return false;
        
		if (currentCharValue == -1) 
		 	return true;
		
		while(char.IsLetterOrDigit(currentCh))
			NextCh();
		   
        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            
            return false;
        }

        return true;
    }
}

public class LetterDigitLexer : Lexer
{

    protected System.Text.StringBuilder intString;
    public int result;
    protected int sign;
    public LetterDigitLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override bool Parse()
    {
		bool was_letter = false;
        NextCh();
        if (char.IsLetter(currentCh)){
			NextCh();
			was_letter = true;
		}
		else 
			return false;
        		
		while(char.IsLetterOrDigit(currentCh)){
			if (char.IsDigit(currentCh) == was_letter){
				was_letter = !was_letter; 
				NextCh();
			}else 
				return false;
		}
		   
        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            
            return false;
        }

        return true;
    }
}



public class Program
{
    public static void Main()
    {	
		System.Console.WriteLine("Testing IntLexer:");
		List<string> test_int = new List<string> {"1","123","+123","-123","+a","+","+1233f"," "};
        foreach (var str in test_int) {
			IntLexer L = new IntLexer(str);
        	System.Console.WriteLine(System.String.Format("{0} : {1}",str,L.Parse()));
		}
        
		System.Console.WriteLine("Testing IDLexer:");
		List<string> test_id = new List<string> {"a","a1ad","a___","1",""," "};
        foreach (var str in test_id) {
			IdLexer L = new IdLexer(str);
        	System.Console.WriteLine(System.String.Format("{0} : {1}",str,L.Parse()));
		}
		
		System.Console.WriteLine("Testing NoZeroIntLexer:");
		List<string> test_nozeroint = new List<string> {"1","123","+123","-123","+0","0","-0"," "};
        foreach (var str in test_nozeroint) {
			NoZeroIntLexer L = new NoZeroIntLexer(str);
        	System.Console.WriteLine(System.String.Format("{0} : {1}",str,L.Parse()));
		}
		
		
		System.Console.WriteLine("Testing LetterDigitLexer:");
		List<string> test_LetterDigitLexer = new List<string> {"a","a1","a1a","1","a11","a1aa",""," "};
        foreach (var str in test_LetterDigitLexer) {
			LetterDigitLexer L = new LetterDigitLexer(str);
        	System.Console.WriteLine(System.String.Format("{0} : {1}",str,L.Parse()));
		}
		
	}    
}