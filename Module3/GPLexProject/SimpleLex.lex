%using ScannerHelper;
%namespace SimpleScanner

Alpha 	[a-zA-Z_]
Digit   [0-9] 
AlphaDigit {Alpha}|{Digit}
INTNUM  {Digit}+
REALNUM {INTNUM}\.{INTNUM}
ID {Alpha}{AlphaDigit}* 
DotChr [^\r\n]
OneLineCmnt  \/\/{DotChr}*
SingleApString \'[^']*\'

// «десь можно делать описани€ типов, переменных и методов - они попадают в класс Scanner
%{
  public int LexValueInt;
  public int LexCount = 0;
  public int LexAllLength = 0;
  public int LexMinLength = 0;
  public int LexMaxLength = 0;
  public int LexIntSum = 0;
  public double LexDoubleSum = 0;
  public double LexValueDouble;
  private bool incomment = false;
  public List<string> IdList;
%}

%x COMMENT

%%

"{" { 
  // переход в состо€ние COMMENT
  incomment = true;
  BEGIN(COMMENT);
  
}

<COMMENT> "}" { 
  // переход в состо€ние INITIAL
  BEGIN(INITIAL);
  incomment = false;
  return (int)Tok.MLCOMMENT;
}

{INTNUM} { 
  LexValueInt = int.Parse(yytext);
  return (int)Tok.INUM;
}

{REALNUM} { 
  LexValueDouble = double.Parse(yytext);
  return (int)Tok.RNUM;
}

{OneLineCmnt} {
	return (int)Tok.LINECOMMENT;
}

{SingleApString} {
	return (int)Tok.SASTRING;
}

begin { 
  return (int)Tok.BEGIN;
}

end { 
  return (int)Tok.END;
}

cycle { 
  return (int)Tok.CYCLE;
}

{ID}  { 
  return (int)Tok.ID;
}

":" { 
  return (int)Tok.COLON;
}

":=" { 
  return (int)Tok.ASSIGN;
}

";" { 
  return (int)Tok.SEMICOLON;
}

[^ \r\n] {
	LexError();
	return 0; // конец разбора
}
<COMMENT>{
  <<EOF>>   {LexError(); return 0;}          
  {ID} { IdList.Add(yytext); }
}

%%

// «десь можно делать описани€ переменных и методов - они тоже попадают в класс Scanner

public void LexError()
{
	Console.WriteLine("({0},{1}): Ќеизвестный символ {2}", yyline, yycol, yytext);
}

public double LexAvgLength(){
	return (double)LexAllLength/LexCount;
}



public string TokToString(Tok tok)
{
	

	switch (tok)
	{
		case Tok.ID:
			LexCount++;
			LexAllLength+=yyleng;
			LexMaxLength = yyleng > LexMaxLength ? yyleng  : LexMaxLength;
			LexMinLength = (yyleng < LexMinLength || LexMinLength == 0)? yyleng  : LexMinLength;
			return tok + " " + yytext;
		case Tok.INUM:
			LexIntSum +=LexValueInt;
			return tok + " " + LexValueInt;
		case Tok.RNUM:
			LexDoubleSum +=LexValueDouble;
			return tok + " " + LexValueDouble;
		default:
			return tok + "";
	}
}

