using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleLangLexer;

namespace SimpleLangParser
{
    public class ParserException : System.Exception
    {
        public ParserException(string msg)
            : base(msg)
        {
        }

    }

    public class Parser
    {
        private SimpleLangLexer.Lexer l;

        public Parser(SimpleLangLexer.Lexer lexer)
        {
            l = lexer;
        }

        public void Progr()
        {
            Block();
        }

        public void Expr() 
        {
            if (l.LexKind == Tok.ID || l.LexKind == Tok.INUM)
            {
                l.NextLexem();
            }
            else
            {
                SyntaxError("expression expected");
            }
        }

        public void Assign() 
        {
            l.NextLexem();  // пропуск id
            if (l.LexKind == Tok.ASSIGN)
            {
                l.NextLexem();
            }
            else {
                SyntaxError(":= expected");
            }
            E();
        }

        public void StatementList() 
        {
            Statement();
            while (l.LexKind == Tok.SEMICOLON)
            {
                l.NextLexem();
                Statement();
            }
        }

        public void Statement() 
        {
            switch (l.LexKind)
            {
                case Tok.BEGIN:
                    {
                        Block(); 
                        break;
                    }
                case Tok.CYCLE:
                    {
                        Cycle(); 
                        break;
                    }
                case Tok.ID:
                    {
                        Assign();
                        break;
                    }
                case Tok.WHILE:
                    {
                        While();
                        break;
                    }
                case Tok.FOR:
                    {
                        For();
                        break;
                    }
                case Tok.IF:
                    {
                        If();
                        break;
                    }
                default:
                    {
                        SyntaxError("Operator expected");
                        break;
                    }
            }
        }

        public void Block() 
        {
            l.NextLexem();    // пропуск begin
            StatementList();
            if (l.LexKind == Tok.END)
            {
                l.NextLexem();
            }
            else
            {
                SyntaxError("end expected");
            }

        }

        public void Cycle() 
        {
            l.NextLexem();  // пропуск cycle
            E();
            Statement();
        }
        
        public void While()   // WHILE expr DO statement
        {
            l.NextLexem();
            E();
            if (l.LexKind == Tok.DO)
                l.NextLexem();
            else
                SyntaxError("do expected");
            Statement();
        }

        public void For()   // FOR ID := expr TO expr DO statement 
        {
            l.NextLexem();
            Assign();

            if (l.LexKind == Tok.TO)
                l.NextLexem();
            else
                SyntaxError("to expected");
            E();

            if (l.LexKind == Tok.DO)
                l.NextLexem();
            else
                SyntaxError("do expected");
            Statement();
        }

        public void If()   // IF expr THEN stat ELSE stat 
        {
            l.NextLexem();
            E();

            if (l.LexKind == Tok.THEN)
                l.NextLexem();
            else
                SyntaxError("then expected");
            Statement();

            if (l.LexKind == Tok.ELSE)
            {
                l.NextLexem();
                Statement();
            }
        }

        public void E()    // E ::= T A
        {
            T();
            A();
        }

        public void A()   // A ::= ε | + T A | - T A
        {
            if(l.LexKind == Tok.PLUS || l.LexKind == Tok.MINUS)
            {
                l.NextLexem();
                T();
                A();
            }
        }

        public void T()   // T ::= M B
        {
            M();
            B();
        }

        public void B()  // B ::= ε | * M B | / M B
        {
            if(l.LexKind == Tok.MULTIPLY || l.LexKind == Tok.DIVISION)
            {
                l.NextLexem();
                M();
                B();
            }
        }

        public void M()  // M ::= id | num | (E)
        {
            switch (l.LexKind)
            {
                case Tok.ID:
                    {
                        l.NextLexem();
                        break;
                    }
                case Tok.INUM:
                    {
                        l.NextLexem();
                        break;
                    }
                case Tok.OPEN_BRACKET:
                    {
                        l.NextLexem();
                        E();
                        if (l.LexKind == Tok.CLOS_BRACKET)
                            l.NextLexem();
                        else
                            SyntaxError(") expected");
                        break;
                    }
                default:
                    {
                        SyntaxError("Incorrect M");
                        break;
                    }
            }
        }

        public void SyntaxError(string message) 
        {
            var errorMessage = "Syntax error in line " + l.LexRow.ToString() + ":\n";
            errorMessage += l.FinishCurrentLine() + "\n";
            errorMessage += new String(' ', l.LexCol - 1) + "^\n";
            if (message != "")
            {
                errorMessage += message;
            }
            throw new ParserException(errorMessage);
        }
   
    }
}
