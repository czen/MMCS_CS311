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
StrApostr  \'[^']*\'

// ����� ����� ������ �������� �����, ���������� � ������� - ��� �������� � ����� Scanner
%{
  public int LexValueInt;
  public double LexValueDouble;
  public int cntID = 0;
  private int sumID = 0;
  public int minID = 0;
  public int maxID = 0;
  public int sumInt = 0;
  public double sumDouble = 0;
  public bool flag = true;
  public List<string> lst = new List<string>();
%}

%x COMMENT

%%
{INTNUM} { 
  LexValueInt = int.Parse(yytext);      // yytext - ��������� ������������� ������� �������	
  return (int)Tok.INUM;
}

{REALNUM} { 
  LexValueDouble = double.Parse(yytext);
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

"{" { 
  // ������� � ��������� COMMENT
  BEGIN(COMMENT);
  flag = false;
  return (int)Tok.MULT_COMM;
}

<COMMENT> "}" { 
  // ������� � ��������� INITIAL
  BEGIN(INITIAL);
  flag = true;
}

{OneLineCmnt} {
	return (int)Tok.COMM;
}

{StrApostr} {
	return (int)Tok.STR_IN_APOSTROPHES;
}

<COMMENT>{ID} {
  lst.Add(yytext);
}

[^ \r\n] {
	LexError();
	return 0; // ����� �������
}

%%

// ����� ����� ������ �������� ���������� � ������� - ��� ���� �������� � ����� Scanner

public void LexError()
{
	Console.WriteLine("({0},{1}): ����������� ������ {2}", yyline, yycol, yytext);
}

public void SynError()
{
	Console.WriteLine("Syntax error");
}

public string TokToString(Tok tok)
{
	switch (tok)
	{
		case Tok.ID:
		{
			cntID += 1;
			sumID += yyleng;
			if(minID == 0 || minID > yyleng)
				minID = yyleng;
			if(maxID == 0 || maxID < yyleng)
				maxID = yyleng;
			return tok + " " + yytext;
		}
		case Tok.INUM:
		{
			sumInt += LexValueInt;
			return tok + " " + LexValueInt;
		}
		case Tok.RNUM:
		{
			sumDouble += LexValueDouble;
			return tok + " " + LexValueDouble;
		}
		default:
			return tok + "";
	}
}

public double AverageID(){
	if(cntID != 0)
		return (double)sumID / cntID;
	else
		return 0;
}

