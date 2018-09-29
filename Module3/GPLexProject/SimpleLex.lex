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
Apostr \'[^']*\'
%x COMMENT

// «десь можно делать описани€ типов, переменных и методов - они попадают в класс Scanner
%{
  public int LexValueInt;
  public double LexValueDouble;

  public int Count;
  public int MinLen = int.MaxValue;
  public int MaxLen = int.MinValue;
  public double SumLen;
  public double AvgLen  => SumLen / Count;
  public double DSum;
  public int ISum;
  public List<String> IDS = new List<String>();
%}

%%

<COMMENT>{ID} {IDS.Add(yytext);}

"{" { 
  // переход в состо€ние COMMENT
  BEGIN(COMMENT);
}

<COMMENT> "}" { 
  // переход в состо€ние INITIAL
  BEGIN(INITIAL);
}

{Apostr} {
    return (int)Tok.APOSTR;
}

{OneLineCmnt} {
    return (int)Tok.ONELINECOMMENT;
}

{INTNUM} { 
  LexValueInt = int.Parse(yytext);
  ISum += LexValueInt;
  return (int)Tok.INUM;
}

{REALNUM} { 
  LexValueDouble = double.Parse(yytext);
  DSum += LexValueDouble;
  return (int)Tok.RNUM;
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
  SumLen += yytext.Length;
  Count++;
  if (yytext.Length < MinLen)
    MinLen = yytext.Length;
  if (yytext.Length > MaxLen)
    MaxLen = yytext.Length;
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

%%

// «десь можно делать описани€ переменных и методов - они тоже попадают в класс Scanner

public void LexError()
{
	Console.WriteLine("({0},{1}): Ќеизвестный символ {2}", yyline, yycol, yytext);
}

public string TokToString(Tok tok)
{
	switch (tok)
	{
		case Tok.ID:
			return tok + " " + yytext;
		case Tok.INUM:
			return tok + " " + LexValueInt;
		case Tok.RNUM:
			return tok + " " + LexValueDouble;
		default:
			return tok + "";
	}
}

