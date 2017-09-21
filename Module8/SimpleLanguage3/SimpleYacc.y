%{
// Ёти объ€влени€ добавл€ютс€ в класс GPPGParser, представл€ющий собой парсер, генерируемый системой gppg
    public BlockNode root; //  орневой узел синтаксического дерева 
    public Parser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
	private bool InDefSect = false;
%}

%output = SimpleYacc.cs

%union { 
			public double dVal; 
			public int iVal; 
			public string sVal; 
			public Node nVal;
			public ExprNode eVal;
			public StatementNode stVal;
			public BlockNode blVal;
       }

%using System.IO;
%using ProgramTree;

%namespace SimpleParser

%start progr

%token BEGIN END CYCLE ASSIGN ASSIGNPLUS ASSIGNMINUS ASSIGNMULT SEMICOLON WRITE VAR PLUS MINUS MULT DIV LPAREN RPAREN COLUMN
%token <iVal> INUM 
%token <dVal> RNUM 
%token <sVal> ID

%type <eVal> expr ident T F 
%type <stVal> statement assign block cycle write empty var varlist 
%type <blVal> stlist block

%%

progr   : block { root = $1; }
		;

stlist	: statement 
			{ 
				$$ = new BlockNode($1); 
			}
		| stlist SEMICOLON statement 
			{ 
				$1.Add($3); 
				$$ = $1; 
			}
		;

statement: assign { $$ = $1; }
		| block   { $$ = $1; }
		| cycle   { $$ = $1; }
		| write   { $$ = $1; }
		| var     { $$ = $1; }
		| empty   { $$ = $1; }
		;

empty	: { $$ = new EmptyNode(); }
		;
	
ident 	: ID 
		{
			if (!InDefSect)
				if (!SymbolTable.vars.ContainsKey($1))
					throw new Exception("("+@1.StartLine+","+@1.StartColumn+"): ѕеременна€ "+$1+" не описана");
			$$ = new IdNode($1); 
		}	
	;
	
assign 	: ident ASSIGN expr { $$ = new AssignNode($1 as IdNode, $3); }
		;

expr	: expr PLUS T { $$ = new BinOpNode($1,$3,'+'); }
		| expr MINUS T { $$ = new BinOpNode($1,$3,'-'); }
		| T { $$ = $1; }
		;
		
T 		: T MULT F { $$ = new BinOpNode($1,$3,'*'); }
		| T DIV F { $$ = new BinOpNode($1,$3,'/'); }
		| F { $$ = $1; }
		;
		
F 		: ident  { $$ = $1 as IdNode; }
		| INUM { $$ = new IntNumNode($1); }
		| LPAREN expr RPAREN { $$ = $2; }
		;

block	: BEGIN stlist END { $$ = $2; }
		;

cycle	: CYCLE expr statement { $$ = new CycleNode($2,$3); }
		;
		
write	: WRITE LPAREN expr RPAREN { $$ = new WriteNode($3); }
		;
		
var		: VAR { InDefSect = true; } varlist 
		{ 
			foreach (var v in ($3 as VarDefNode).vars)
				SymbolTable.NewVarDef(v.Name, type.tint);
			InDefSect = false;	
		}
		;

varlist	: ident 
		{ 
			$$ = new VarDefNode($1 as IdNode); 
		}
		| varlist COLUMN ident 
		{ 
			($1 as VarDefNode).Add($3 as IdNode);
			$$ = $1;
		}
		;
	
%%

