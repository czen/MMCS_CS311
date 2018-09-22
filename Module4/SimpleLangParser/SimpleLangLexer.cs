﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SimpleLangLexer
{

    public class LexerException : System.Exception
    {
        public LexerException(string msg)
            : base(msg)
        {
        }

    }

    public enum Tok
    {
        EOF,
        ID,
        INUM,
        COLON,
        SEMICOLON,
        ASSIGN,
        BEGIN,
        END,
        CYCLE,
        COMMA,
        PLUS,
        MINUS,
        MULTIPLE,
        DIVISION, 
        DIV,
        MOD, 
        AND,
        OR,
        NOT,
        PLUSASSIGN,
        MINUSASSIGN,
        MULTIPLEASSIGN,
        DIVISIONASSIGN,
        MORE,
        LESS,
        MOREEQUAL,
        LESSEQUAL,
        EQUAL,
        NOTEQUAL,
        COMMENT,
        LONGCOMMENT
    }

    public class Lexer
    {
        private int position;
        private char currentCh;                      // Текущий символ
        public int LexRow, LexCol;                  // Строка-столбец начала лексемы. Конец лексемы = LexCol+LexText.Length
        private int row, col;                        // текущие строка и столбец в файле
        private TextReader inputReader;
        private Dictionary<string, Tok> keywordsMap; // Словарь, сопоставляющий ключевым словам константы типа TLex. Инициализируется процедурой InitKeywords 
        public Tok LexKind;                         // Тип лексемы
        public string LexText;                      // Текст лексемы
        public int LexValue;                        // Целое значение, связанное с лексемой LexNum

        private string CurrentLineText;  // Накапливает символы текущей строки для сообщений об ошибках
        

        public Lexer(TextReader input)
        {
            CurrentLineText = "";
            inputReader = input;
            keywordsMap = new Dictionary<string, Tok>();
            InitKeywords();
            row = 1; col = 0;
            NextCh();       // Считать первый символ в ch
            NextLexem();    // Считать первую лексему, заполнив LexText, LexKind и, возможно, LexValue
        }

        public void Init() {

        }

        private void PassSpaces()
        {
            while (char.IsWhiteSpace(currentCh))
            {
                NextCh();
            }
        }

        private void InitKeywords()
        {
            keywordsMap["begin"] = Tok.BEGIN;
            keywordsMap["end"] = Tok.END;
            keywordsMap["cycle"] = Tok.CYCLE;
            keywordsMap["div"] = Tok.DIV;
            keywordsMap["mod"] = Tok.MOD;
            keywordsMap["and"] = Tok.AND;
            keywordsMap["or"] = Tok.OR;
            keywordsMap["not"] = Tok.NOT;
        }

        public string FinishCurrentLine()
        {
            return CurrentLineText + inputReader.ReadLine();
        }

        private void LexError(string message)
        {
            System.Text.StringBuilder errorDescription = new System.Text.StringBuilder();
            errorDescription.AppendFormat("Lexical error in line {0}:", row);
            errorDescription.Append("\n");
            errorDescription.Append(FinishCurrentLine());
            errorDescription.Append("\n");
            errorDescription.Append(new String(' ', col - 1) + '^');
            errorDescription.Append('\n');
            if (message != "")
            {
                errorDescription.Append(message);
            }
            throw new LexerException(errorDescription.ToString());
        }

        private void NextCh()
        {
            // В LexText накапливается предыдущий символ и считывается следующий символ
            LexText += currentCh;
            var nextChar = inputReader.Read();
            if (nextChar != -1)
            {
                currentCh = (char)nextChar;
                if (currentCh != '\n')
                {
                    col += 1;
                    CurrentLineText += currentCh;
                }
                else
                {
                    row += 1;
                    col = 0;
                    CurrentLineText = "";
                }
            }
            else
            {
                currentCh = (char)0; // если достигнут конец файла, то возвращается #0
            }
        }

        public void NextLexem()
        {
            PassSpaces();
            // R К этому моменту первый символ лексемы считан в ch
            LexText = "";
            LexRow = row;
            LexCol = col;
            // Тип лексемы определяется по ее первому символу
            // Для каждой лексемы строится синтаксическая диаграмма

            if (currentCh == '+')
            {
                NextCh();
                LexKind = Tok.PLUS;
                if (currentCh == '=')
                {
                    NextCh();
                    LexKind = Tok.PLUSASSIGN;
                }
            }
            else if (currentCh == '-')
            {
                NextCh();
                LexKind = Tok.MINUS;
                if (currentCh == '=')
                {
                    NextCh();
                    LexKind = Tok.MINUSASSIGN;
                }
            }
            else if (currentCh == '*')
            {
                NextCh();
                LexKind = Tok.MULTIPLE;
                if (currentCh == '=')
                {
                    NextCh();
                    LexKind = Tok.MULTIPLEASSIGN;
                }
            }
            else if (currentCh == '/')
            {
                NextCh();
                LexKind = Tok.DIVISION;
                if (currentCh == '=')
                {
                    NextCh();
                    LexKind = Tok.DIVISIONASSIGN;
                }
                else if (currentCh == '/')
                {
                    NextCh();
                    while (currentCh != '\n')
                        NextCh();
                    LexKind = Tok.COMMENT;
                }
            }
            else if (currentCh == ',')
            {
                NextCh();
                LexKind = Tok.COMMA;
            }
            else if (currentCh == ';')
            {
                NextCh();
                LexKind = Tok.SEMICOLON;
            }
            else if (currentCh == '>')
            {
                NextCh();
                LexKind = Tok.MORE;
                if (currentCh == '=')
                {
                    NextCh();
                    LexKind = Tok.MOREEQUAL;
                }
            }
            else if (currentCh == '<')
            {
                NextCh();
                LexKind = Tok.LESS;
                if (currentCh == '=')
                {
                    NextCh();
                    LexKind = Tok.LESSEQUAL;
                }
                else if (currentCh == '>')
                {
                    NextCh();
                    LexKind = Tok.NOTEQUAL;
                }
            }
            else if (currentCh == '=')
            {
                NextCh();
                LexKind = Tok.EQUAL;
            }
            else if (currentCh == ';')
            {
                NextCh();
                LexKind = Tok.SEMICOLON;
            }
            else if (currentCh == '{')
            {
                NextCh();
                LexKind = Tok.LONGCOMMENT;
                while (true)
                {
                    if (currentCh == '}')
                    {
                        NextCh();
                        break;
                    }
                    if (currentCh == 0)
                        LexError("Comment unclosed ");
                    NextCh();
                }
            }
            else if (currentCh == ':')
            {
                LexKind = Tok.COLON;
                NextCh();
                if (currentCh == '=')
                {
                    LexKind = Tok.ASSIGN;
                    NextCh();
                }
            }
            else if (char.IsLetter(currentCh))
            {
                while (char.IsLetterOrDigit(currentCh))
                {
                    NextCh();
                }
                if (keywordsMap.ContainsKey(LexText))
                {
                    LexKind = keywordsMap[LexText];
                }
                else
                {
                    LexKind = Tok.ID;
                }
            }
            else if (char.IsDigit(currentCh))
            {
                while (char.IsDigit(currentCh))
                {
                    NextCh();
                }
                LexValue = Int32.Parse(LexText);
                LexKind = Tok.INUM;
            }
            else if ((int)currentCh == 0)
            {
                LexKind = Tok.EOF;
            }
            else
            {
                LexError("Incorrect symbol " + currentCh);
            }
        }

        public virtual void ParseToConsole()
        {
            do
            {
                Console.WriteLine(TokToString(LexKind));
                NextLexem();
            } while (LexKind != Tok.EOF);
        }

        public string TokToString(Tok t)
        {
            var result = t.ToString();
            switch (t)
            {
                case Tok.ID: result += ' ' + LexText;
                    break;
                case Tok.INUM: result += ' ' + LexValue.ToString();
                    break;
            }
            return result;
        }
    }
}
