using System;
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
        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,
        ASSIGN,
        BEGIN,
        END,
        CYCLE,
        COMMA,
        DIV,
        MOD,
        AND,
        OR,
        NOT,
        PLUSEQUAL,
        MINUSEQUAL,
        MULTIPLYEQUAL,
        DIVIDEEQUAL,
        GREATER,
        LESS,
        GREATEREQUAL,
        LESSEQUAL,
        EQUAL,
        NOTEQUAL
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

        private void TokOrTokEq(Tok tok, Tok tokEq)
        {
            NextCh();
            LexKind = tok;
            if (currentCh == '=')
            {
                NextCh();
                LexKind = tokEq;
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
            if (currentCh == '{') //skipping multi-line comment
            {
                NextCh();
                while (currentCh != '}' && (int)currentCh != 0)
                    NextCh();
                if ((int)currentCh == 0)
                    LexError("Error at the end of file: expected closing '}'");
                NextCh();
                NextLexem();
            }
            if (currentCh == '/') //skipping one-line comment
            {
                NextCh();
                if (currentCh == '=')
                {
                    NextCh();
                    LexKind = Tok.DIVIDEEQUAL;
                    return;
                }

                if (currentCh != '/')
                {
                    LexKind = Tok.DIVIDE;
                    return;
                }

                NextCh();
                while (LexRow == row && (int)currentCh != 0)
                    NextCh();
                NextLexem();
                return;
            }
            if (currentCh == '<')
            {
                NextCh();
                LexKind = Tok.LESS;
                if (currentCh == '>')
                {
                    NextCh();
                    LexKind = Tok.NOTEQUAL;
                }
                else if (currentCh == '=')
                {
                    NextCh();
                    LexKind = Tok.LESSEQUAL;
                }
            }
            else if (currentCh == '>')
                TokOrTokEq(Tok.GREATER, Tok.GREATEREQUAL);
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
            else if (currentCh == ',')
            {
                NextCh();
                LexKind = Tok.COMMA;
            }
            else if (currentCh == '+')
                TokOrTokEq(Tok.PLUS, Tok.PLUSEQUAL);
            else if (currentCh == '-')
                TokOrTokEq(Tok.MINUS, Tok.MINUSEQUAL);
            else if (currentCh == '*')
                TokOrTokEq(Tok.MULTIPLY, Tok.MULTIPLYEQUAL);
            else if (currentCh == ':')
            {
                NextCh();
                if (currentCh != '=')
                    LexKind = Tok.COLON;
                else
                {
                    NextCh();
                    LexKind = Tok.ASSIGN;
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
