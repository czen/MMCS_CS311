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
            sum();
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

        public void While()
        {
            l.NextLexem();
            Expr();

            if (l.LexKind == Tok.DO)
            {
                l.NextLexem();
            }
            else
            {
                SyntaxError("do expected");
            }

            Statement();
        }

        public void For()
        {
            l.NextLexem();
            Assign();

            if (l.LexKind == Tok.TO)
            {
                l.NextLexem();
            }
            else
            {
                SyntaxError("to expected");
            }

            Expr();

            if (l.LexKind == Tok.DO)
            {
                l.NextLexem();
            }
            else
            {
                SyntaxError("do expected");
            }
            Statement();
        }

        public void If()
        {
            l.NextLexem();
            Expr();

            if (l.LexKind == Tok.THEN)
            {
                l.NextLexem();
            }
            else
            {
                SyntaxError("then expected");
            }
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

        public void brackets()
        {
            if (l.LexKind == Tok.LEFT_BRACKET)
            {
                l.NextLexem();
                Expr();
                if (l.LexKind != Tok.RIGHT_BRACKET)
                {
                    SyntaxError(") expected");
                }
                l.NextLexem();
            }
            else
            {
                if (l.LexKind == Tok.ID || l.LexKind == Tok.INUM)
                {
                    l.NextLexem();
                }
                else
                {
                    SyntaxError("expr expected");
                }
            }
        }

        public void prod()
        {
            brackets();
            while (l.LexKind == Tok.MULTIPLY || l.LexKind == Tok.DIVIDE) 
            {
                l.NextLexem();
                brackets();
            }
        }

        public void sum()
        {
            prod();
            while (l.LexKind == Tok.PLUS || l.LexKind == Tok.MINUS)
            {
                l.NextLexem();
                prod();
            }
        }

    }
}
