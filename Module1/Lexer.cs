public class LexerException : System.Exception{
    public LexerException(string msg): base(msg){
    }
}

public class Lexer{
    protected int position;
    protected char currentCh;  
    protected int currentCharValue;
    protected System.IO.StringReader inputReader;
    protected string inputString;
    public Lexer(string input){
        inputReader = new System.IO.StringReader(input);
        inputString = input;
    }
    public void Error(){
        System.Text.StringBuilder o = new System.Text.StringBuilder();
        o.Append(inputString + '\n');
        o.Append(new System.String(' ', position - 1) + "^\n");
        o.AppendFormat("Error in symbol {0}", currentCh);
        throw new LexerException(o.ToString());
    }
    protected void NextCh(){
        this.currentCharValue = this.inputReader.Read();
        this.currentCh = (char)currentCharValue;
        this.position += 1;
    }
    public virtual void Parse(){}
}

// 1.1)
public class IntLexer : Lexer
{
    protected System.Text.StringBuilder intString;
    public IntLexer(string input): base(input){
        intString = new System.Text.StringBuilder();
    }

    public override void Parse(){
        NextCh();
		string str = "";        
		if (currentCh == '+' || currentCh == '-'){
			str = str + currentCh;
            NextCh();
		}
		
        if (char.IsDigit(currentCh)){
			str = str + currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {
			str = str + currentCh;
            NextCh();
        }

        if (currentCharValue != -1){
            Error();
        }
		
		int integer = System.Int32.Parse(str);
		
        System.Console.WriteLine("IntLexer is recognized {0}", integer);
    }
}

// 1.2)
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
		string str = "";
		
		if (char.IsLetter(currentCh))
        {
			str = str + currentCh;
            NextCh();
        }
		else
        {
            Error();
        }        

        while (char.IsLetterOrDigit(currentCh) || currentCh == '_')
        {
			str = str + currentCh;
            NextCh();
        }

        if (currentCharValue != -1){
            Error();
        }
		
        System.Console.WriteLine("ID is recognized {0}", str);
    }
}

// 1.3)
public class IntwhLexer : Lexer
{
    protected System.Text.StringBuilder intString;

    public IntwhLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
		string str = "";
		
		if (currentCh == '+' || currentCh == '-'){
			str = str + currentCh;
            NextCh();
		}
		
		if (char.IsDigit(currentCh) && currentCh != '0')
        {
			str = str + currentCh;
            NextCh();
        }
		else
        {
            Error();
        }        

        while (char.IsDigit(currentCh))
        {
			str = str + currentCh;
            NextCh();
        }

        if (currentCharValue != -1){
            Error();
        }
		
		int intwh = System.Int32.Parse(str);
		
        System.Console.WriteLine("Int is recognized {0}", intwh);
    }
}

// 1.4)
public class LetDigLexer : Lexer
{
    protected System.Text.StringBuilder intString;

    public LetDigLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
		string str = "";
		if (char.IsLetter(currentCh))
        {
			str = str + currentCh;
			
            NextCh();
		}
		else
		{ 
			Error();
		}
		
		bool dig = true;
		
        while (char.IsDigit(currentCh) && dig || char.IsLetter(currentCh) && !dig)
        {
			str = str + currentCh;
			dig = !dig;
            NextCh();
        }

        if (currentCharValue != -1){
            Error();
        }
		
        System.Console.WriteLine("LetDig is recognized {0}", str);
    }
}

// 1.5)
public class DelimLexer : Lexer
{
    protected System.Text.StringBuilder intString;

    public DelimLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
		string str = "";

		if (char.IsLetter(currentCh)){
			str = str + currentCh;
            NextCh();
		}
		else{ 
			Error();
		}
		bool letter = false;
        while (currentCh == ',' || char.IsLetter(currentCh) || currentCh == ';' )
        {
			if(char.IsLetter(currentCh) && letter){			
				str = str + currentCh;
				NextCh();
			}
			else{
				if((currentCh == ',' || currentCh == ';') && !letter){
					NextCh();
					if(!char.IsLetter(currentCh)){
						Error();					
					}
				}
				else{
					Error();
				}
			}
			letter = !letter;	    
        }
        if (currentCharValue != -1){
            Error();
        }
		
        System.Console.WriteLine("DelimLexer is recognized {0}", str);
    }
}

// 2.1) 
public class TrimLexer : Lexer
{
    protected System.Text.StringBuilder intString;

    public TrimLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }
    public override void Parse(){
        NextCh();
		string str = "";

		if (char.IsDigit(currentCh)){
			str = str + currentCh;
            NextCh();
		}
		else{ 
			Error();
		}
		
		bool end = false;
		
        while (currentCh == ' ' || char.IsDigit(currentCh)){
			if(char.IsDigit(currentCh)){			
				str = str + currentCh;
				end = true;
			}
			else{
				end = false;
			}
			NextCh();
        }

        if (currentCharValue != -1 || !end){ 
            Error();
        }
		
        System.Console.WriteLine("TrimLexer is recognized {0}", str);
    }
}

// 2.2) 
public class SpecLexer : Lexer
{
    protected System.Text.StringBuilder intString;

    public SpecLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
		string str = "";

		if (char.IsLetter(currentCh))
        {
			str = str + currentCh;
            NextCh();
		}
		else
		{ 
			Error();
		}
		
		int letter = 1, digit = 0;
		
        while (char.IsLetter(currentCh) || char.IsDigit(currentCh))
        {
			if(char.IsLetter(currentCh) && letter < 2){
				str = str + currentCh;
				digit = 0;				
				letter+=1;
			}
			else{
				if(char.IsDigit(currentCh) && digit < 2){
					str = str + currentCh;
					letter = 0;				
					digit+=1;
				}
				else{
					Error();	
				}
			}
			NextCh();
        }

        if (currentCharValue != -1){
            Error();
        }
		
        System.Console.WriteLine("SpecLexer is recognized {0}", str);
    }	
}

// 2.3)
public class DoubleLexer : Lexer
{
    protected System.Text.StringBuilder intString;

    public DoubleLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
		string str = "";

		if (char.IsDigit(currentCh))
        {
			str = str + currentCh;
            NextCh();
		}
		else
		{ 
			Error();
		}
		
		bool dot = false, end = false;
		
        while (char.IsDigit(currentCh) || currentCh == '.')
        {
			if (currentCh == '.'){
				if(!dot){
					dot = true;
				}
				else{
					Error();
				}			
			}
			str = str + currentCh;
			NextCh();
			if(dot && char.IsDigit(currentCh)){
				end = true;
			}
        }

        if (currentCharValue != -1 || !dot || !end){
            Error();
        }
        System.Console.WriteLine("DoubleLexer is recognized {0}", str);
    }	
}

// 2.4)
public class StrLexer : Lexer
{
    protected System.Text.StringBuilder intString;

    public StrLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
		string str = "";

		if (currentCh == '\''){
			str = str + currentCh;
            NextCh();
		}
		else{ 
			Error();
		}
        while (currentCh != '\''){
			str = str + currentCh;
			NextCh();
			if(currentCharValue == -1){
				Error();
			}
        }
		str = str + currentCh;
		NextCh();

        if (currentCharValue != -1){
            Error();
        }
		
        System.Console.WriteLine("ComLexer is recognized {0}", str);

    }	
}

// 2.5)
public class ComLexer : Lexer
{
    protected System.Text.StringBuilder intString;

    public ComLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
		string str = "";
		
		if (currentCh == '/'){
			str = str + currentCh;
            NextCh();
		}
		else{ 
			Error();
		}
		
		if (currentCh == '*'){
			str = str + currentCh;
		}
		else{ 
			Error();
		}
		
		bool notend = true;
		
        while (notend){
			NextCh(); 
			if(currentCharValue == -1){
				Error();
			}
			if(currentCh == '*'){
				str = str + currentCh;
				NextCh();
				if(currentCh == '/'){
					str = str + currentCh;
					notend = false;
					NextCh();
				}
				else{
					str = str + currentCh;
				}
			}
			else{
				str = str + currentCh;
			}			
        }

        if (currentCharValue != -1){
            Error();
        }
        System.Console.WriteLine("ComLexer is recognized {0}", str);
    }	
}

//
public class CombIDLexer : Lexer
{
    protected System.Text.StringBuilder intString;
    public CombIDLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse(){
		NextCh();
		string str = "";
		while (char.IsLetter(currentCh)){
			str +=currentCh;
			NextCh();
			while(currentCh != '.'){
				while (char.IsLetterOrDigit(currentCh) || currentCh == '_'){
					str = str + currentCh;
            		NextCh();
        		}
				if (currentCh == '.' || currentCharValue == -1){
					if(currentCh == '.'){
						str+=currentCh;
					}
					break;				
				}
				else{
					Error();				
				}
			}
			if(currentCh == '.'){
				NextCh();
				if (currentCharValue == -1){
					Error();
				}
			}
		}
		
		if (currentCharValue != -1 || str == ""){
            Error();
        }
		
        System.Console.WriteLine("CombIDLexer is recognized {0}", str);
    }	
}

public class Program{
	// 1.1)
	public static void test11(){
		System.Console.WriteLine("Work IntLexer:");
		string[] testInput = {"156745", "1", "-256", "+42", "", "skdufg", "1234jkdsfv"};
		Lexer L;
		for(int i = 0; i < testInput.Length; ++i){
			L = new IntLexer(testInput[i]);
			try{
            	L.Parse();
        	}
        	catch (LexerException e){
            	System.Console.WriteLine(e.Message);
        	}		
		}
	}
	
	// 1.2)
	public static void test12(){
		System.Console.WriteLine("Work IdLexer:");
		string[] testInput = {"F", "f2134sdf764", "asdfg", "wdof_ad1", "", "sd.?", "."};
		Lexer L;
		for(int i = 0; i < testInput.Length; ++i){
			L = new IdLexer(testInput[i]);
			try{
            	L.Parse();
        	}
        	catch (LexerException e){
            	System.Console.WriteLine(e.Message);
        	}		
		}
	}
	
	// 1.3)
	public static void test13(){
		System.Console.WriteLine("Work IntwhLexer:");
		string[] testInput = {"8", "123", "+42", "-256", "", "0", "sdif", "+", "123sdfhb"};
		Lexer L;
		for(int i = 0; i < testInput.Length; ++i){
			L = new IntwhLexer(testInput[i]);
			try{
            	L.Parse();
        	}
        	catch (LexerException e){
            	System.Console.WriteLine(e.Message);
        	}		
		}
	}
	
	// 1.4)
	public static void test14(){
		System.Console.WriteLine("Work LetDigLexer:");
		string[] testInput = {"f8s7s0n0", "s", "s0", "", "0", "sdif", "+", "s12hb"};
		Lexer L;
		for(int i = 0; i < testInput.Length; ++i){
			L = new LetDigLexer(testInput[i]);
			try{
            	L.Parse();
        	}
        	catch (LexerException e){
            	System.Console.WriteLine(e.Message);
        	}		
		}
	}
	
	// 1.5)
	public static void test15(){
		System.Console.WriteLine("Work DelimLexer:");
		string[] testInput = {"a,b,c,d;s", "s", "s;e", "", "a,f,d;", "sdif", "+", "s12hb", "3,2,3", ";f,s"};
		Lexer L;
		for(int i = 0; i < testInput.Length; ++i){
			L = new DelimLexer(testInput[i]);
			try{
            	L.Parse();
        	}
        	catch (LexerException e){
            	System.Console.WriteLine(e.Message);
        	}		
		}
	}
	
	// 2.1)
	public static void test21(){
		System.Console.WriteLine("Work TrimLexer:");
		string[] testInput = {"1 2  3   4 5", "1", "1                      2", " ", " 1 2", "d", "", "1 2    "};
		Lexer L;
		for(int i = 0; i < testInput.Length; ++i){
			L = new TrimLexer(testInput[i]);
			try{
            	L.Parse();
        	}
        	catch (LexerException e){
            	System.Console.WriteLine(e.Message);
        	}		
		}
	}
	
	// 2.2)
	public static void test22(){
		System.Console.WriteLine("Work SpecLexer:");
		string[] testInput = {"a1", "aa12c23dd1", "aa1", "1a", "aa111", "aaa1", "", "as12ag231s12dd"};
		Lexer L;
		for(int i = 0; i < testInput.Length; ++i){
			L = new SpecLexer(testInput[i]);
			try{
            	L.Parse();
        	}
        	catch (LexerException e){
            	System.Console.WriteLine(e.Message);
        	}		
		}
	}
	
	// 2.3)
	public static void test23(){
		System.Console.WriteLine("Work DoubleLexer:");
		string[] testInput = {"123.45678", "12", ".12", "12.", "", "asdd", "12.213.2"};
		Lexer L;
		for(int i = 0; i < testInput.Length; ++i){
			L = new DoubleLexer(testInput[i]);
			try{
            	L.Parse();
        	}
        	catch (LexerException e){
            	System.Console.WriteLine(e.Message);
        	}		
		}
	}
	
	// 2.4)
	public static void test24(){
		System.Console.WriteLine("Work StrLexer:");
		string[] testInput = {"'dfasfg'", "'af'b'", "'a", "a'", "''", "'ascv'd", "â"};
		Lexer L;
		for(int i = 0; i < testInput.Length; ++i){
			L = new StrLexer(testInput[i]);
			try{
            	L.Parse();
        	}
        	catch (LexerException e){
            	System.Console.WriteLine(e.Message);
        	}		
		}
	}
	
	// 2.5)
	public static void test25(){
		System.Console.WriteLine("Work ComLexer:");
		string[] testInput = {"/*dfasfg*/", "/*af*bd/f*/", "/*a", "a*/", "/**/", "d", "/*askjdfb*/sdf"};
		Lexer L;
		for(int i = 0; i < testInput.Length; ++i){
			L = new ComLexer(testInput[i]);
			try{
            	L.Parse();
        	}
        	catch (LexerException e){
            	System.Console.WriteLine(e.Message);
        	}		
		}
	}
	
	//
	public static void test3(){
		System.Console.WriteLine("Work CombIDLexer:");
		string[] testInput = {"bsdfb.scvb.d", "b1111.s11.d1.g123", "b", ".f", "b..d", "sdf.111.d", ""};
		Lexer L;
		for(int i = 0; i < testInput.Length; ++i){
			L = new CombIDLexer(testInput[i]);
			try{
            	L.Parse();
        	}
        	catch (LexerException e){
            	System.Console.WriteLine(e.Message);
        	}		
		}
	}
	
	public static void Main(){
		test11();
		
		System.Console.WriteLine("");
		test12();
	
		System.Console.WriteLine("");
		test13();
		
		System.Console.WriteLine("");
		test14();
		
		System.Console.WriteLine("");
		test15();
		
		System.Console.WriteLine("");
		test21();
		
		System.Console.WriteLine("");
		test22();
		
		System.Console.WriteLine("");
		test23();
		
		System.Console.WriteLine("");
		test24();
		
		System.Console.WriteLine("");
		test25();
		
		System.Console.WriteLine("");
		test3();
    }
}
