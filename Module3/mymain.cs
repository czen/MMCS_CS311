using System;
using System.IO;
using SimpleScanner;
using ScannerHelper;

namespace GeneratedLexer
{

    class mymain
    {
        static void Main(string[] args)
        {
            int cnt_id = 0;//кол-во идентификаторов
            int min_id_len = Int32.MaxValue, max_id_len = 0; //минимальная, максимальная длины идентификаторов
            double avg_id_len = 0; //средняя длина идентификатора

            int sum_int = 0; //сумма всех целых
            double sum_d = 0; //сумма всех вещественных

            // Чтобы вещественные числа распознавались и отображались в формате 3.14 (а не 3,14 как в русской Culture)
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            var fname = @"..\..\a.txt";
            Console.WriteLine(File.ReadAllText(fname));
            Console.WriteLine("-------------------------");

            Scanner scanner = new Scanner(new FileStream(fname, FileMode.Open));

            int tok = 0;
            do {
                tok = scanner.yylex();

                if (tok == (int)Tok.EOF)
                {
                    Console.WriteLine();
                    Console.WriteLine("number of id: {0:D}", cnt_id);
                    Console.WriteLine("average length of the id: {0:N}", avg_id_len / cnt_id);
                    Console.WriteLine("min length of the id: {0:D}", min_id_len);
                    Console.WriteLine("min length of the id: {0:D}", max_id_len);

                    Console.WriteLine();
                    Console.WriteLine("sum of int: {0:D}", sum_int);
                    Console.WriteLine("sum of double: {0:N}", sum_d);

                    Console.WriteLine();

                    break;
                }

                Console.WriteLine(scanner.TokToString((Tok)tok));
            } while (true);

            Console.ReadKey();
        }
    }
}
