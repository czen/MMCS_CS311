%using ScannerHelper;
%namespace SimpleScanner

%x COMMENT

Alpha 	[a-zA-Z_]
Digit   [0-9] 
AlphaDigit {Alpha}|{Digit}
INTNUM  {Digit}+
REALNUM {INTNUM}\.{INTNUM}
ID {Alpha}{AlphaDigit}*
DotChr [^\r\n]
OneLineCmnt  \/\/{DotChr}*
StringInApostrophes \'[^']*\'

// «десь можно делать описани€ типов, переменных и методов - они попадают в класс Scanner
%{
  public int LexValueInt;
  public double LexValueDouble;
  public int idCount = 0;
  public int idSumLengths = 0;
  public double idAverageLength;
  public int idMinLength = 10000;
  public int idMaxLength = 0;
  public int intsSum = 0;
  public double doublesSum = 0;
  public string idInComments;
%}

%%
<COMMENT>{ID} {
  // обрабатываетс€ ID внутри комментари€
  idInComments += yytext + "; ";
}

"{" { 
  // переход в состо€ние COMMENT
  BEGIN(COMMENT);
}

<COMMENT> "}" { 
  // переход в состо€ние INITIAL
  BEGIN(INITIAL);
  return (int)Tok.MULTILINECOMMENT;
}

{OneLineCmnt} { 
  return (int)Tok.ONELINECMNT;
}

{StringInApostrophes} { 
  return (int)Tok.STRINGINAPOSTROPHES;
}

{INTNUM} { 
  LexValueInt = int.Parse(yytext);
  intsSum += LexValueInt;
  return (int)Tok.INUM;
}

{REALNUM} { 
  LexValueDouble = double.Parse(yytext);
  doublesSum += LexValueDouble;
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
	++idCount;
	idSumLengths += yyleng;
	if (yyleng < idMinLength)
		idMinLength = yyleng;
	if (yyleng > idMaxLength)
		idMaxLength = yyleng;

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

public string IdInfo()
{
	idAverageLength = (double)idSumLengths / idCount;
	return "Count = " + idCount + "\nAverage length = " + idAverageLength + 
		"\nMin length = " + idMinLength + "\nMax length = " + idMaxLength + '\n';
}

public string NumsInfo()
{
	return "Sum ints = " + intsSum + "\nSum doubles = " + doublesSum + '\n';
}

public string IdInCommentsInfo()
{
	return "List id = " + idInComments + '\n';
}