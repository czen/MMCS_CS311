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

            //task2    количество, среднюю, минимальную и максимальную длину всех идентификаторов
            int cnt = 0;
            double middle = 0;
            int min = int.MaxValue, max = 0;
            //task3 сумму всех целых и сумму всех вещественных в файле
            int intsum = 0;
            double doublesum = 0;
            int tok = 0;
            do {
                tok = scanner.yylex();
                if (tok == (int)Tok.EOF)
                    break;
                Console.WriteLine(scanner.TokToString((Tok)tok));
                if (tok == (int)Tok.ID)
                {
                    ++cnt;
                    int length = scanner.yytext.Length;
                    middle += length;
                    if (length < min)
                        min = length;
                    if (length > max)
                        max = length;
                }
                else if (tok == (int)Tok.INUM)
                    intsum += scanner.LexValueInt;
                else if (tok == (int)Tok.RNUM)
                    doublesum += scanner.LexValueDouble;
            } while (true);

            middle = middle / (double)cnt;

            //task2 
            Console.WriteLine("number of all identifiers: {0}", cnt);
            Console.WriteLine("average length: {0}, min length: {1}, max length: {2}", middle, min, max);
            //task3
            Console.WriteLine("sum of all integers: {0}", intsum);
            Console.WriteLine("sum of all doubles: {0}", doublesum);

            //extra4
            Console.WriteLine("list of all identifiers:\n{0}", scanner.All_IDs);
            Console.ReadKey();
        }
    }
}
