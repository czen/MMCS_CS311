using System;
using System.Collections.Generic;
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

            //task 1 (just correct file)
            Console.WriteLine(File.ReadAllText(@"..\..\a.txt"));
            Scanner scanner1 = new Scanner(new FileStream(@"..\..\a.txt", FileMode.Open));
            TestFile(scanner1);

            //task2 (min, max, average length and count of IDs)
            Console.WriteLine(File.ReadAllText(@"..\..\b.txt"));
            Scanner scanner2 = new Scanner(new FileStream(@"..\..\b.txt", FileMode.Open));
            TestFile(scanner2);
            Console.WriteLine("IDs count : {0}, max ID length: {1}, min ID length : {2}, average ID length : {3}",
                scanner2.IdsCount, scanner2.MaxIdLength, scanner2.MinIdLength, scanner2.AverageIdsLength);

            //task3 (sum of ints, sum of doubles)
            Console.WriteLine(File.ReadAllText(@"..\..\c.txt"));
            Scanner scanner3 = new Scanner(new FileStream(@"..\..\c.txt", FileMode.Open));
            TestFile(scanner3);
            Console.WriteLine("sum of ints : {0}, sum of reals : {1}", scanner3.IntSum, scanner3.RealSum);

            //extra task 1 (oneline comment)
            Console.WriteLine(File.ReadAllText(@"..\..\extra1.txt"));
            Scanner scanner4 = new Scanner(new FileStream(@"..\..\extra1.txt", FileMode.Open));
            TestFile(scanner4);

            //extra task 2 (string in apostrophies)
            Console.WriteLine(File.ReadAllText(@"..\..\extra2.txt"));
            Scanner scanner5 = new Scanner(new FileStream(@"..\..\extra2.txt", FileMode.Open));
            TestFile(scanner5);

            //extra task 3 (multiline comment)
            Console.WriteLine(File.ReadAllText(@"..\..\extra3.txt"));
            Scanner scanner6 = new Scanner(new FileStream(@"..\..\extra3.txt", FileMode.Open));
            TestFile(scanner6);

            //extra task 4 (ids in multiline comment)
            Console.WriteLine(File.ReadAllText(@"..\..\extra4.txt"));
            Scanner scanner7 = new Scanner(new FileStream(@"..\..\extra4.txt", FileMode.Open));
            TestFile(scanner7);

            Console.ReadKey();
        }

        static void TestFile(Scanner scanner)
        {
            Console.WriteLine("-------------------------");

            int tok = 0;
            do
            {
                tok = scanner.yylex();
                if (tok == (int)Tok.EOF)
                    break;
                Console.WriteLine(scanner.TokToString((Tok)tok));
            } while (true);
            Console.WriteLine("-------------------------");
        }
    }
}
