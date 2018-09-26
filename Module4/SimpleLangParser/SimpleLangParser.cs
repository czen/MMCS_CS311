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
        private Lexer l;

        public Parser(Lexer lexer)
        {
            l = lexer;
        }

        public void Progr()
        {
            Block();
        }

        public void Expr_M()
        {
            if (l.LexKind == Tok.ID || l.LexKind == Tok.INUM)
            {
                l.NextLexem();
            }
            else if (l.LexKind == Tok.OPENPAR)
            {
                l.NextLexem();
                Expr();
                if (l.LexKind == Tok.CLOSEPAR)
                    l.NextLexem();
                else SyntaxError("closing parenthesis was expected");
            }
            else
            {
                SyntaxError("id / number / (expression) expected");
            }
        }

        private void Expr_B()
        {
            if (l.LexKind == Tok.MULTIPLY || l.LexKind == Tok.DIVIDE)
            {
                l.NextLexem();
                partT();
            }
        }

        private void partT()
        {
            Expr_M();
            Expr_B();
        }

        private void partA()
        {
            if (l.LexKind == Tok.PLUS || l.LexKind == Tok.MINUS)
            {
                l.NextLexem();
                partT();
                partA();
            }
        }


        public void Expr()
        {
            partT();
            partA();
            
        }

        public void Assign()
        {
            l.NextLexem();  // пропуск id
            if (l.LexKind == Tok.ASSIGN)
            {
                l.NextLexem();
            }
            else
            {
                SyntaxError(":= expected");
            }
            Expr();
        }

        public void WhileDo()
        {
            l.NextLexem();      // skip while
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

        public void ForToDo()
        {
            l.NextLexem();      // skip for
            if (l.LexKind == Tok.ID)
            {
                Assign();
                if (l.LexKind == Tok.TO)
                {
                    l.NextLexem();
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
                else
                {
                    SyntaxError("to expected");
                }

            }
            else
            {
                SyntaxError("assign expected");
            }
        }

        public void IfThenElse()
        {
            l.NextLexem();      // skip IF
            Expr();
            if (l.LexKind == Tok.THEN)
            {
                l.NextLexem();
                Statement();
                if (l.LexKind == Tok.ELSE)
                {
                    l.NextLexem();
                    Statement();
                }
            }
            else
            {
                SyntaxError("then expected");
            }
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
                        WhileDo();
                        break;
                    }
                case Tok.FOR:
                    {
                        ForToDo();
                        break;
                    }
                case Tok.IF:
                    {
                        IfThenElse();
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