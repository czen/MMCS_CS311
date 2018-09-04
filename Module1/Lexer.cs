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


public class ListLexer : Lexer
{

    protected System.Text.StringBuilder intString;
    public int result;
    protected int sign;
	public List<char> chrs;
    public ListLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
		chrs = new List<char>();
    }

    public override bool Parse()
    {
		bool was_letter = false;
        NextCh();
		if(!char.IsLetter(currentCh))
			return false;
		while(char.IsLetter(currentCh) || currentCh == ','|| currentCh ==';'){
			if(char.IsLetter(currentCh) != was_letter){
				if(char.IsLetter(currentCh))
					chrs.Add(currentCh);
				was_letter = !was_letter;
			}
			else return false;
			NextCh();
		}
	    
		   
        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            
            return false;
        }

        return true;
    }
}

public class DigitListLexer : Lexer
{

    protected System.Text.StringBuilder intString;
    public int result;
    protected int sign;
	public List<int> ints;
    public DigitListLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
		ints = new List<int>();
    }

    public override bool Parse()
    {
        NextCh();
		if (currentCharValue == -1)
			return false;
        while (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
			if (char.IsWhiteSpace(currentCh)) {
				NextCh();
				continue;
			} 	
            else if (char.IsDigit(currentCh)){
				ints.Add((int)(currentCh - '0'));
				NextCh();
				if(char.IsDigit(currentCh))
					return false;
				else 
					continue;
			}
			else 
				return false;
			
            
        }
		if (ints.Count == 0) return false;
        return true;
    }
}

public class GroupLexer : Lexer
{

    protected System.Text.StringBuilder intString;
	public string result;
    public GroupLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override bool Parse()
    {
		bool letter_group = false;
		int cnt = 0;
	
        NextCh();
		if (currentCharValue == -1)
			return false;
		letter_group = char.IsLetter(currentCh);
		while(currentCharValue != -1){
			if(!char.IsLetterOrDigit(currentCh)) 
				return false;
			if(char.IsLetter(currentCh) == letter_group){
				cnt++;
				intString.Append(currentCh);
			}
			else{
				letter_group = !letter_group;
				cnt = 1;
				intString.Append(currentCh);
			}
			if (cnt > 2) 
				return false;
			NextCh();
		}
		result = intString.ToString();
		return true;
    }
}

public class RealLexer : Lexer
{

    protected System.Text.StringBuilder intString;
    public RealLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override bool Parse()
    {
		
        NextCh(); // at first char
        if (currentCh == '+' || currentCh == '-' )
        {
    
            NextCh();
			if(!char.IsDigit(currentCh))
				return false;
        } 
        else if (char.IsDigit(currentCh)) {
			NextCh();
		}
		else
			return false;
		
        while (char.IsDigit(currentCh))
        {
            NextCh();
        }
		
		
		if(currentCh == '.'){
			NextCh();
			while (char.IsDigit(currentCh))
            	NextCh();
		}

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            
            return false;
        }

        return true;
    }
}

public class StringLexer : Lexer
{

    public StringLexer(string input)
        : base(input)
    {
    }

    public override bool Parse()
    {
		
        NextCh(); // at first char
       	if(currentCh != '\'')
			return false;
		NextCh();
		while(currentCharValue != -1 && currentCh != '\'')
			NextCh();
		if (currentCh != '\'')
			return false;
		NextCh();
		if ( currentCharValue != -1)
			return false;
		
		return true;
		
    }
}

public class CommentLexer : Lexer
{

    public CommentLexer(string input)
        : base(input)
    {
    }
	
	private bool ParseStart(){
		if (currentCh != '/'){
			NextCh();	
			return false;}
		else {
			NextCh();
			if (currentCh != '*'){
				NextCh();
				return false;}
			NextCh();
			return true;
		}
	 	
	}
	private bool ParseEnd(){
		if (currentCh != '*'){
			NextCh();
			return false;
			}
		else {
			NextCh();
			if (currentCh != '/'){
				NextCh();
				return false;}
			NextCh();
			return true;
		}
		
	}
	
	

    public override bool Parse()
    {
		
        NextCh(); // at first char
		if (!ParseStart()) 
			return false;
		while(currentCharValue != -1)
			if (ParseEnd()){
				if(currentCharValue != -1)
					return false;
				return true;
			}
		
		return false;
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
		
		System.Console.WriteLine("Testing ListLexer:");
		List<string> test_ListLexer = new List<string> {"a","a,","a;b","a,b;","ab","",","," "};
        foreach (var str in test_ListLexer) {
			ListLexer L = new ListLexer(str);
        	System.Console.WriteLine(System.String.Format("{0} : {1} : {2}",str,L.Parse(),string.Join(",", L.chrs)));
		}
		
		
		System.Console.WriteLine("Testing DigitListLexer:");
		List<string> test_DigitListLexer = new List<string> {"1","  1 2    3","    ","","1 2 3 b"};
        foreach (var str in test_DigitListLexer) {
			DigitListLexer L = new DigitListLexer(str);
        	System.Console.WriteLine(System.String.Format("{0} : {1} : {2}",str,L.Parse(),string.Join(",", L.ints)));
		}
		
		System.Console.WriteLine("Testing GroupLexer:");
		List<string> test_GroupLexer = new List<string> {"aa12c23dd1","aaa12c23dd1","aa12c232dd1", " ", ""};
        foreach (var str in test_GroupLexer) {
			GroupLexer L = new GroupLexer(str);
        	System.Console.WriteLine(System.String.Format("{0} : {1} : {2}",str,L.Parse(),L.result));
		}
		
		
		System.Console.WriteLine("Testing RealLexer:");
		List<string> test_RealLexer = new List<string> {"+12","12","+0.1","123.b" ," ", ""};
        foreach (var str in test_RealLexer) {
			RealLexer L = new RealLexer(str);
        	System.Console.WriteLine(System.String.Format("{0} : {1}",str,L.Parse()));
		}
		
		System.Console.WriteLine("Testing StringLexer:");
		List<string> test_StringLexer = new List<string> {"'asd'","''","a","'aaa'a","'aaa" ," ", ""};
        foreach (var str in test_StringLexer) {
			StringLexer L = new StringLexer(str);
        	System.Console.WriteLine(System.String.Format("{0} : {1}",str,L.Parse()));
		}
		
		
		System.Console.WriteLine("Testing CommentLexer:");
		List<string> test_CommentLexer = new List<string> {"/*ddd*/","/**/","/*fff"," ", ""};
        foreach (var str in test_CommentLexer) {
			CommentLexer L = new CommentLexer(str);
        	System.Console.WriteLine(System.String.Format("{0} : {1}",str,L.Parse()));
		}
	}    
}