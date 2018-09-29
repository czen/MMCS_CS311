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
            Console.WriteLine("-----------  task 1 --------------");

            
            Scanner scanner = new Scanner(new FileStream(fname, FileMode.Open));

            //task 1
            int tok = 0;
            do
            {
                tok = scanner.yylex();
                if (tok == (int)Tok.EOF)
                    break;
                Console.WriteLine(scanner.TokToString((Tok)tok));
            } while (true);

            //task 2
            Console.WriteLine("------------ task 2 -------------");

            Console.WriteLine("Count = " + scanner.Count + " MaxLen = " + scanner.MaxLen +" MinLen = " + scanner.MinLen + " AvgLen = " + scanner.AvgLen);

            //task3
            Console.WriteLine("------------ task 3 -------------");

            Console.WriteLine("Int Sum = " + scanner.ISum + " Double Sum = " + scanner.DSum);

            Console.WriteLine("----------- extra task 1 (comment) extra task 2 (appostr) --------------");

            fname = @"..\..\b.txt";
            Console.WriteLine(File.ReadAllText(fname));


            scanner = new Scanner(new FileStream(fname, FileMode.Open));

            //extra task 1, 2
            tok = 0;
            do
            {
                tok = scanner.yylex();
                if (tok == (int)Tok.EOF)
                    break;
                Console.WriteLine(scanner.TokToString((Tok)tok));
            } while (true);


            Console.WriteLine("----------- extra task 3 --------------");

            fname = @"..\..\c.txt";
            Console.WriteLine(File.ReadAllText(fname));


            scanner = new Scanner(new FileStream(fname, FileMode.Open));

            //extra task 3
            tok = 0;
            do
            {
                tok = scanner.yylex();
                if (tok == (int)Tok.EOF)
                    break;
                Console.WriteLine(scanner.TokToString((Tok)tok));
            } while (true);

            Console.WriteLine("----------- extra task 4 --------------");

            foreach (var x in scanner.IDS)
                Console.WriteLine(x);

            Console.ReadKey();
        }
    }
}
