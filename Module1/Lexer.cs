using System;
using System.Collections.Generic;

using LexerTasks;

public class LexerException : System.Exception
{
    public LexerException(string msg)
        : base(msg)
    {
    }

}

public class Lexer
{

    protected int position;
    protected char currentCh;       // очередной считанный символ
    protected int currentCharValue; // целое значение очередного считанного символа
    protected System.IO.StringReader inputReader;
    protected string inputString;

    public Lexer(string input)
    {
        inputReader = new System.IO.StringReader(input);
        inputString = input;
    }

    public void Error()
    {
        System.Text.StringBuilder o = new System.Text.StringBuilder();
        o.Append(inputString + '\n');
        o.Append(new System.String(' ', position - 1) + "^\n");
        o.AppendFormat("Error in symbol {0}", currentCh);
        throw new LexerException(o.ToString());
    }

    protected void NextCh()
    {
        this.currentCharValue = this.inputReader.Read();
        this.currentCh = (char)currentCharValue;
        this.position += 1;
    }

    public virtual void Parse()
    {

    }
}

public class Program
{
    public static void Main()
    {
        IntLexer.Testing();
        IdLexer.Testing();
        IntZeroLexer.Testing();
        AlternateCharDigitLexer.Testing();
        SeparatedCharsLexer.Testing();

        DigitsSpacesLexer.Testing();
        AlternateDigitLetters2Lexer.Testing();
        RealDotLexer.Testing();
        StringLexer.Testing();

        //Console.ReadLine();
    }
}