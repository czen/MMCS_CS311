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
            // eps or +TA or -TA
            if (l.LexKind == Tok.PLUS || l.LexKind == Tok.MINUS)
            {
                l.NextLexem();
                T();
                A();
            }
            //eps correct
        }

        public void M()
        {
            //id or num or (Expr)
            if (l.LexKind == Tok.ID || l.LexKind == Tok.INUM)
            {
                l.NextLexem();
            }
            else if (l.LexKind == Tok.LEFTROUNDBRACKET)
            {
                l.NextLexem();
                Expr();
                if (l.LexKind == Tok.RIGHTROUNDBRACKET)
                    l.NextLexem();
                else
                    SyntaxError("')' expected");
            }
            else
            {
                SyntaxError("id or num or '(' expected");
            }
        }

        public void B()
        {
            //eps or *MB or /MB
            if (l.LexKind == Tok.MULTIPLICATION || l.LexKind == Tok.DIVISION)
            {
                l.NextLexem();
                M();
                B();
            }
            //eps correct
        }

        public void While()
        {
            l.NextLexem();  // пропуск while
            Expr();
            if (l.LexKind == Tok.DO)
            {
                l.NextLexem();
                StatementList();
            }
            else
            {
                SyntaxError("do expected");
            }
        }

        public void For()
        {
            l.NextLexem();  // пропуск for
            if (l.LexKind == Tok.ID)
                Assign();
            else
                SyntaxError("ID expected");

            if (l.LexKind == Tok.TO)
            {
                l.NextLexem();
                Expr();
            }
            else
                SyntaxError("to expected");
            if (l.LexKind == Tok.DO)
            {
                l.NextLexem();
                StatementList();
            }
            else
            {
                SyntaxError("do expected");
            }
        }

        public void If()
        {
            l.NextLexem();
            Expr();
            if (l.LexKind == Tok.THEN)
            {
                l.NextLexem();
                StatementList();
            }
            else
            {
                SyntaxError("then expected");
            }
            if (l.LexKind == Tok.ELSE)
            {
                l.NextLexem();
                StatementList();
            }
        }

        /*public void Expr() 
        {
            if (l.LexKind == Tok.ID || l.LexKind == Tok.INUM)
            {
                l.NextLexem();
            }
            else
            {
                SyntaxError("expression expected");
            }
        }*/

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
