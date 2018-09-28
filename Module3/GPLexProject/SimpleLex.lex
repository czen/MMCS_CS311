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
InApostrophies \'[^']*\'
%x COMMENT

// Çäåñü ìîæíî äåëàòü îïèñàíèÿ òèïîâ, ïåðåìåííûõ è ìåòîäîâ - îíè ïîïàäàþò â êëàññ Scanner
%{
  public int LexValueInt;
  public double LexValueDouble;
  public int IdsCount;
  public int MaxIdLength;
  private int TotalIdsLength;
  public int MinIdLength = int.MaxValue;
  public double AverageIdsLength { get {return (double)TotalIdsLength / IdsCount;}}
  public int IntSum;
  public double RealSum;
  public List<string> CommentsIds = new List<string>();
%}

%%
"{" { 
  // переход в состояние COMMENT
  BEGIN(COMMENT);
}

<COMMENT> "}" { 
  // переход в состояние INITIAL
  Console.WriteLine("IDs in multiline comment: ");
  foreach (var comment in CommentsIds)
    Console.WriteLine("{0}, ", comment);
  Console.WriteLine("end of IDs from multiline comment. Next are usual tokens");
  BEGIN(INITIAL);
}

<COMMENT>{ID} {
  // обрабатывается ID внутри комментария
  CommentsIds.Add(yytext);
}

{InApostrophies} {
    return (int)Tok.INAPOSTROPHIES;
}

{OneLineCmnt} {
    return (int)Tok.ONELINECOMMENT;
}

{INTNUM} { 
  LexValueInt = int.Parse(yytext);
  IntSum += LexValueInt;
  return (int)Tok.INUM;
}

{REALNUM} { 
  LexValueDouble = double.Parse(yytext);
  RealSum += LexValueDouble;
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
  ++IdsCount;
  if (yytext.Length < MinIdLength)
    MinIdLength = yytext.Length;
  if (yytext.Length > MaxIdLength)
    MaxIdLength = yytext.Length;
  TotalIdsLength += yytext.Length;
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
	return 0; // êîíåö ðàçáîðà
}

%%

// Çäåñü ìîæíî äåëàòü îïèñàíèÿ ïåðåìåííûõ è ìåòîäîâ - îíè òîæå ïîïàäàþò â êëàññ Scanner

public void LexError()
{
	Console.WriteLine("({0},{1}): Íåèçâåñòíûé ñèìâîë {2}", yyline, yycol, yytext);
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
        case Tok.INAPOSTROPHIES:
            return tok + " " + yytext;
		default:
			return tok + "";
	}
}

