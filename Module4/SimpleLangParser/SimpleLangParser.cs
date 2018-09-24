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

        // EXTRA TASK 2
        public void Expr() 
        {
            T();
            A();
        }

        public void T()
        {
            M();
            B();
        }

        public void A()
        {
            if (l.LexKind == Tok.PLUS || l.LexKind == Tok.MINUS)
            {
                l.NextLexem();
                T();
                A();
            }
        }

        public void M()
        {
            if (l.LexKind == Tok.ID || l.LexKind == Tok.INUM)
                l.NextLexem();
            else if (l.LexKind == Tok.OPEN_BRACKET)
            {
                Expr();
                if (l.LexKind == Tok.CLOSE_BRACKET)
                    l.NextLexem();
                else
                    SyntaxError("bracket expected");
            }
            else
                SyntaxError("incorrect expression");
        }

        public void B()
        {
            if (l.LexKind == Tok.MULTIPLICATION || l.LexKind == Tok.DIVISION)
            {
                l.NextLexem();
                T();
                A();
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
            Expr();
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
            Expr();
            Statement();
        }

        // TASK 1
        public void While()
        {
            l.NextLexem();
            Expr();
            if (l.LexKind != Tok.DO)
                SyntaxError("do expected");
            l.NextLexem();
            Statement();
        }

        // TASK 2
        public void For()
        {
            l.NextLexem();

            if (l.LexKind != Tok.ID)
                SyntaxError("id expected");

            Assign();

            if (l.LexKind != Tok.TO)
                SyntaxError("to expected");
            l.NextLexem();

            Expr();

            if (l.LexKind != Tok.DO)
                SyntaxError("do expected");
            l.NextLexem();

            Statement();
        }

        // EXTRA TASK 1
        public void If()
        {
            l.NextLexem();
            Expr();
            if (l.LexKind != Tok.THEN)
                SyntaxError("then expected");
            l.NextLexem();
            Statement();

            if (l.LexKind == Tok.ELSE)
            {
                l.NextLexem();
                Statement();
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
