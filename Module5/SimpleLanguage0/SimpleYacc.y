%{
// Ёти объ€влени€ добавл€ютс€ в класс GPPGParser, представл€ющий собой парсер, генерируемый системой gppg
    public Parser(AbstractScanner<int, LexLocation> scanner) : base(scanner) { }
%}

%output = SimpleYacc.cs

%namespace SimpleParser

%token BEGIN END CYCLE INUM RNUM ID ASSIGN SEMICOLON  WHILE DO REPEAT UNTIL FOR TO WRITE OPENPAR CLOSEPAR

%%

progr   : block
		;

stlist	: statement 
		| stlist SEMICOLON statement 
		;

statement: assign
		| block  
		| cycle  
		| whiledo
		| repeatuntil
		| forcycle
		| write
		;

ident 	: ID 
		;
	
assign 	: ident ASSIGN expr 
		;

expr	: ident  
		| INUM 
		;

block	: BEGIN stlist END 
		;

cycle	: CYCLE expr statement 
		;

whiledo : WHILE expr DO statement
		;

repeatuntil : 
		REPEAT stlist UNTIL expr
		;

forcycle : FOR assign TO expr DO statement
		;

write : WRITE OPENPAR expr CLOSEPAR
		;

	
%%
