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
                Statement();
                if (l.LexKind == Tok.ELSE)
                {
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