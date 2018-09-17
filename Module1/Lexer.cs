using System.Threading;

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


//TASK1
public class IntLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public IntLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        int number = 0;
        bool isNegative = false;
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            isNegative = currentCh == '-';
            NextCh();
        }

        if (char.IsDigit(currentCh))
        {
            number = int.Parse(currentCh.ToString());
            System.Console.WriteLine("Number is: {0}", number);
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {

            number = number*10 + int.Parse(currentCh.ToString());
            System.Console.WriteLine("Number is: {0}", number);
            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }
        if (isNegative)
            number *= -1;

        System.Console.WriteLine("Integer is recognized");
        System.Console.WriteLine("Your integer is:{0}", number);
    }
}


//TASK2
public class IdentLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public IdentLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (char.IsLetter(currentCh))
        {
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh) || char.IsLetter(currentCh) || currentCh == '_')
            NextCh();

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Identificator is recognized");
    }
}


//TASK3
public class IntNotZeroLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public IntNotZeroLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '+' || currentCh == '-')
        {
            NextCh();
        }

        if (currentCh != '0' && char.IsDigit(currentCh))
        {
            NextCh();
        }
        else
        {
            Error();
        }

        while (char.IsDigit(currentCh))
        {

            NextCh();
        }


        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Integer is recognized");
    }
}

//Task 4
public class LetterDigitInterleave: Lexer
{

    protected System.Text.StringBuilder intString;

    public LetterDigitInterleave(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        bool nextDigit = false;
        NextCh();
        if (char.IsLetter(currentCh))
        {
            NextCh();
            nextDigit = true;
        }
        else
        {
            Error();
        }

        while (nextDigit && char.IsDigit(currentCh) || !nextDigit && char.IsLetter(currentCh))
        {
            nextDigit = !nextDigit;
            NextCh();
        }

        if (currentCharValue != -1) // StringReader вернет -1 в конце строки
        {
            Error();
        }

        System.Console.WriteLine("Identificator is recognized");
    }
}

//Task 5
public class LettersSeparated : Lexer
{

    protected System.Text.StringBuilder intString;

    public LettersSeparated(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string chars = "";
        bool lastReadSeparator = false;
        NextCh();
        if (char.IsLetter(currentCh))
        {
            chars += currentCh;
            NextCh();
        }
        else
        {
            Error();
        }

        while (currentCharValue != -1)
        {
                if ((currentCh == ';' || currentCh == ',') && !lastReadSeparator)
                {
                    lastReadSeparator = true;
                    NextCh();
                }
                else if (char.IsLetter(currentCh))
                {
                    lastReadSeparator = false;
                    while (char.IsLetter(currentCh))
                    {
                        chars += currentCh;
                        NextCh();
                    }
                }
                else
                    Error();
        }

        if (lastReadSeparator)
        {
            Error();
        }

        System.Console.WriteLine("Identificator is recognized");
        System.Console.WriteLine("Chars: {0}", chars);
    }
}

//Extra task 1
/*
 * Список цифр, разделенных одним или несколькими пробелами.
 * В качестве семантического действия должно быть накопление списка цифр в списке и вывод этого списка в конце программы 
 */
public class DigitsSepBySpaces : Lexer
{

    protected System.Text.StringBuilder intString;

    public DigitsSepBySpaces(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        string digits = "";
        bool lastReadSeparator = false;
        bool wasDigit = false;
        NextCh();
        if (char.IsDigit(currentCh))
        {
            digits+= currentCh;
            wasDigit = true;
            NextCh();
        }
        else
        {
            Error();
        }

        while (currentCharValue != -1)
        {
            if (wasDigit)
            {
                if (currentCh != ' ')
                    Error();
                while (currentCh == ' ')
                    NextCh();
                wasDigit = false;
            }
            else
            {
                if (!char.IsDigit(currentCh))
                    Error();
                wasDigit = true;
                digits += currentCh;
                NextCh();
            }
        }

        if (!wasDigit)
        {
            Error();
        }

        System.Console.WriteLine("Identificator is recognized");
        System.Console.WriteLine("Digits: {0}", digits);
    }
}

//extra-task 2
/*
 * Лексема вида aa12c23dd1, в которой чередуются группы букв и цифр, в каждой группе не более 2 элементов. В качестве семантического действия необходимо накопить данную лексему в виде строки
 */
public class Lexem2 : Lexer
{

    protected System.Text.StringBuilder intString;

    public Lexem2(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        int digitsCount = 0;
        int lettersCount = 0;
        string lexem = "";
        if (char.IsDigit(currentCh))
            digitsCount += 1;
        else if (char.IsLetter(currentCh))
            lettersCount += 1;
        lexem += currentCh;
        NextCh();
        
        while (currentCharValue != -1)
        {
            if (char.IsDigit(currentCh))
            {
                if (digitsCount == 2)
                    Error();
                ++digitsCount;
                lettersCount = 0;
            }
            else if (char.IsLetter(currentCh))
            {
                if (lettersCount == 2)
                    Error();
                ++lettersCount;
                digitsCount = 0;
            }

            lexem += currentCh;
            NextCh();
        }

        System.Console.WriteLine("Identificator is recognized");
        System.Console.WriteLine("Parsed lexem: {0}", lexem);
    }
}

//extra task3: Вещественное с десятичной точкой
public class RealLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public RealLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        bool afterDot = false;
        bool endedWithDot = false;
        double number = 0;
        int sign = 1;
        double divideBy = 10;

        NextCh();

        if (currentCh == '-')
        {
            sign = -1;
            NextCh();
        }
        else if (currentCh == '+')
            NextCh();
            
        while (currentCharValue != -1)
        {
            if (currentCh == '.')
            {
                if (afterDot)
                    Error();
                afterDot = true;
                endedWithDot = true;
                NextCh();
            }
            else if (char.IsDigit(currentCh))
            {
                if (!afterDot)
                    number = 10 * number + int.Parse(currentCh.ToString());
                else
                {
                    number += double.Parse(currentCh.ToString()) / divideBy;
                    divideBy *= 10;
                    endedWithDot = false;
                }
                NextCh();
            }
            else
                Error();
        }

        if (endedWithDot == false)
        {
            System.Console.WriteLine("Expected numbers after dot!");
            Error();
        }

        number *= sign;

        System.Console.WriteLine("Identificator is recognized");
        System.Console.WriteLine("Parsed number: {0}", number);
    }
}

//extra task 4 : Лексема вида 'строка', внутри апострофов отсутствует символ ' (1 балл)
public class ApostrophiesLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public ApostrophiesLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '\'')
            NextCh();
        else
            Error();

        while (currentCharValue != -1)
        {
            if (currentCh == '\'')
            {
                NextCh();
                break;
            }
            /*по условию не совсем понятно, есть ли какие-либо ограничения на "строку" внутри апострофов
           если предполагалось, что они есть, нужно раскомментировать следующие строки кода:
           if (!char.isLetter(currentCh))
               Error();
           */
            NextCh();
        }

        if (currentCharValue != -1)
            Error();

        System.Console.WriteLine("Identificator is recognized");
    }
}

//extra task 5 : Лексема вида /*комментарий*/, внутри комментария не может встретиться последовательность символов */
public class CommentLexer : Lexer
{

    protected System.Text.StringBuilder intString;

    public CommentLexer(string input)
        : base(input)
    {
        intString = new System.Text.StringBuilder();
    }

    public override void Parse()
    {
        NextCh();
        if (currentCh == '/')
        {
            NextCh();
            if (currentCh == '*')
                NextCh();
            else
                Error();
        }
        else
            Error();

        bool wasClosed = false;
        bool wasAsterisk = false;
        while (currentCharValue != -1)
        {
            if (currentCh == '/' && wasAsterisk)
            {
                NextCh();
                wasClosed = true;
                break;
            }
            if (currentCh == '*')
                wasAsterisk = true;
            else
                wasAsterisk = false;
            NextCh();
        }

        if (currentCharValue != -1 || !wasClosed)
            Error();

        System.Console.WriteLine("Identificator is recognized");
    }
}

public class Program
{
    public static void Main()
    {
        //ints
        string intPos = "154216";
        string intNeg = "-154216";
        Lexer L1 = new IntLexer(intPos);
        Lexer L3 = new IntLexer("12x3");
        Lexer L2 = new IntLexer(intNeg);
        System.Console.WriteLine("task 1:");
        try
        {
            L1.Parse();
            L2.Parse();
            L3.Parse();
            
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        //idents
        string goodIdent = "abc_23d";
        string badIdent = "123a";
        Lexer LId1 = new IdentLexer(goodIdent);
        Lexer LId2 = new IdentLexer(badIdent);
        System.Console.WriteLine("task 2:");
        try
        {
            LId1.Parse();
            LId2.Parse();

        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        //not zeros
        string goodInt = "154216";
        string badInt = "054216";
        Lexer NotZeroL1 = new IntNotZeroLexer(goodInt);
        Lexer NotZeroL2 = new IntNotZeroLexer(badInt);
        System.Console.WriteLine("task 3:");
        try
        {
            NotZeroL1.Parse();
            NotZeroL2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }


        //interleaved
        Lexer Interleave1 = new LetterDigitInterleave("a1b2c3");
        Lexer Interleave2 = new LetterDigitInterleave("a1b2c33");
        Lexer Interleave3 = new LetterDigitInterleave("a1b2c");
        System.Console.WriteLine("task 4:");
        try
        {
            Interleave1.Parse();
            Interleave3.Parse();
            Interleave2.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        //letters separated
        Lexer Separated1 = new LettersSeparated("a,b;c");
        Lexer Separated2 = new LettersSeparated("abc");
        Lexer Separated3 = new LettersSeparated("a;bc,");
        System.Console.WriteLine("task 4:");
        try
        {
            Separated1.Parse();
            Separated2.Parse();
            Separated3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        //extra 1
        Lexer SpaceDigits1 = new DigitsSepBySpaces("1  2 3");
        Lexer SpaceDigits2 = new DigitsSepBySpaces("1 2 3");
        Lexer SpaceDigits3 = new DigitsSepBySpaces("1  ");
        System.Console.WriteLine("extra task 1:");
        try
        {
            SpaceDigits1.Parse();
            SpaceDigits2.Parse();
            SpaceDigits3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        //extra 2
        Lexer LettersDigitsInterleaved1 = new Lexem2("aa11bb34cc");
        Lexer LettersDigitsInterleaved2 = new Lexem2("aa11bb3cc");
        Lexer LettersDigitsInterleaved3 = new Lexem2("aa11bb345");
        System.Console.WriteLine("extra task 2:");
        try
        {
            LettersDigitsInterleaved1.Parse();
            LettersDigitsInterleaved2.Parse();
            LettersDigitsInterleaved3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        //extra 3
        Lexer Real1 = new RealLexer("123.54");
        Lexer Real2 = new RealLexer("-15.2");
        Lexer Real3 = new RealLexer("11..");
        System.Console.WriteLine("extra task 3:");
        try
        {
            Real1.Parse();
            Real2.Parse();
            Real3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        //extra 4
        Lexer Apostrophies1 = new ApostrophiesLexer("'Hello, world!'");
        Lexer Apostrophies2 = new ApostrophiesLexer("'Howdy");
        Lexer Apostrophies3 = new ApostrophiesLexer("'Hey, Hey!'blablabla");
        System.Console.WriteLine("extra task 4:");
        try
        {
            Apostrophies1.Parse();
            Apostrophies2.Parse();
            Apostrophies3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }

        //extra 5
        Lexer Comment1 = new CommentLexer("/*hello there*/");
        Lexer Comment2 = new CommentLexer("/*hello * there */");
        Lexer Comment3 = new CommentLexer("/*hello * there */, General Kenobi");
        System.Console.WriteLine("extra task 5:");
        try
        {
            Comment1.Parse();
            Comment2.Parse();
            Comment3.Parse();
        }
        catch (LexerException e)
        {
            System.Console.WriteLine(e.Message);
        }
    }
}