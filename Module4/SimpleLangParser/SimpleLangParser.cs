﻿using System;
using System.Collections.Generic;
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

        public void Expr() 
        {
            Term2();
            Term1();
        }

        private void Term1()
        {
            if (l.LexKind == Tok.PLUS || l.LexKind == Tok.MINUS)
            {
                l.NextLexem();
                Term2();
                Term1();
            }
        }
        
        private void Term2()
        {
            Factor();
            Term3();
        }
        
        private void Term3()
        {
            if (l.LexKind == Tok.MULT || l.LexKind == Tok.DIVISION)
            {
                l.NextLexem();
                Factor();
                Term3();
            }
        }

        private void Factor()
        {
            if (l.LexKind == Tok.ID || l.LexKind == Tok.INUM)
            {
                l.NextLexem();
            } else if (l.LexKind == Tok.LEFT_BRACKET)
            {
                l.NextLexem();
                Expr();
                if (l.LexKind != Tok.RIGHT_BRACKET)
                {
                    SyntaxError("expected: RIGHT_BRACKET");
                }
                l.NextLexem();
            } else
            {
                SyntaxError("expected: ID, INUM, LEFT_BRACKET");
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
                    Condition();
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
            if (l.LexKind != Tok.DO)
            {
                SyntaxError("keyword 'do' expected");
            }
            l.NextLexem();
            Statement();
        }

        public void For()
        {
            l.NextLexem();
            if (l.LexKind != Tok.ID)
            {
                SyntaxError("ID expected");
            }
            Assign();
            if (l.LexKind != Tok.TO)
            {
                SyntaxError("keyword 'to' expected");
            }
            l.NextLexem();
            Expr();
            if (l.LexKind != Tok.DO)
            {
                SyntaxError("keyword 'do' expected");
            }
            l.NextLexem();
            Statement();
        }

        public void Condition()
        {
            l.NextLexem();
            Expr();
            if (l.LexKind != Tok.THEN)
            {
                SyntaxError("keyword 'then' expected");
            }
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
