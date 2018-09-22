using System;
using System.IO;
using SimpleScanner;
using ScannerHelper;
using System.Collections.Generic;

namespace Main
{
    class mymain
    {

        static void parser(string fname)
        {
            Console.WriteLine(File.ReadAllText(fname));
            Console.WriteLine("-------------------------");

            Scanner scanner = new Scanner(new FileStream(fname, FileMode.Open));

            int tok = 0;
            do
            {
                tok = scanner.yylex();
                if (tok == (int)Tok.EOF)
                    break;
                Console.WriteLine(scanner.TokToString((Tok)tok));
            } while (true);

            if (!scanner.flag)
                scanner.SynError();
            else
            {

                Console.WriteLine("Count ID: {0}", scanner.cntID);
                Console.WriteLine("Average length of ID: {0}", scanner.AverageID());
                Console.WriteLine("Min length of ID: {0}", scanner.minID);
                Console.WriteLine("Max length of ID: {0}", scanner.maxID);
                Console.WriteLine("Sum of ints: {0}", scanner.sumInt);
                Console.WriteLine("Sum of doubles: {0}", scanner.sumDouble);
                if (scanner.lst.Count != 0)
                {
                    Console.Write("All ID in multiline comment: ");
                    foreach (string str in scanner.lst)
                        Console.Write(str + ' ');
                    Console.WriteLine();
                }
            }
            Console.ReadKey();
        }
        static void Main(string[] args)
        {
            // Чтобы вещественные числа распознавались и отображались в формате 3.14 (а не 3,14 как в русской Culture)
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            Console.WriteLine("------------------- a.txt -------------------");
            parser(@"..\..\a.txt");

            Console.WriteLine("------------------- b.txt -------------------");
            parser(@"..\..\b.txt");

            Console.WriteLine("------------------- c.txt -------------------");
            parser(@"..\..\c.txt");

            Console.WriteLine("------------------- d.txt -------------------");
            parser(@"..\..\d.txt");
        }
    }
}
