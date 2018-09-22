using System;
using System.IO;
using SimpleScanner;
using ScannerHelper;

namespace Main
{
    class mymain
    {
        static void Main(string[] args)
        {
            // Чтобы вещественные числа распознавались и отображались в формате 3.14 (а не 3,14 как в русской Culture)
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            var fname = @"..\..\a.txt";
            Console.WriteLine(File.ReadAllText(fname));
            Console.WriteLine("-------------------------");

            Scanner scanner = new Scanner(new FileStream(fname, FileMode.Open));

            // task 2
            int numberOfIds = 0;
            int minLength = Int32.MaxValue;
            int maxLength = 0;
            int sumOfLengths = 0;
            // task 3
            int sumInt = 0;
            double sumReal = 0.0;

            int tok = 0;
            do
            {
                tok = scanner.yylex();
                if (tok == (int)Tok.EOF)
                    break;
                if (tok == (int)Tok.ID)
                {
                    ++numberOfIds;
                    minLength = Math.Min(minLength, scanner.yyleng);
                    maxLength = Math.Max(maxLength, scanner.yyleng);
                    sumOfLengths += scanner.yyleng;
                }
                else if (tok == (int)Tok.INUM)
                {
                    sumInt += Int32.Parse(scanner.yytext);
                }
                else if (tok == (int)Tok.RNUM)
                {
                    sumReal += Double.Parse(scanner.yytext);
                }
                Console.WriteLine(scanner.TokToString((Tok)tok));
            } while (true);

            // task 2
            Console.WriteLine("Number of ids: " + numberOfIds);
            Console.WriteLine("Average length is " + (double)sumOfLengths / numberOfIds);
            Console.WriteLine("Min length is " + minLength);
            Console.WriteLine("Max length is " + maxLength);
            // task 3
            Console.WriteLine("Sum of int numbers: " + sumInt);
            Console.WriteLine("Sum of real numbers: " + sumReal);

            Console.ReadKey();
        }
    }
}
