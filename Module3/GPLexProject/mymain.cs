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

            // Задание 2
            int ID_count = 0;
            int ID_sum_len = 0;
            int ID_max_len = 0;
            int ID_min_len = int.MaxValue;

            // Задание 3
            int INUM_sum = 0;
            double RNUM_sum = 0;

            int tok = 0;
            do {
                tok = scanner.yylex();
                if (tok == (int)Tok.EOF)
                    break;

                // Задание 2
                if (tok == (int)Tok.ID)
                {
                    ++ID_count;
                    int l = scanner.yytext.Length;
                    ID_sum_len += l;
                    if (l > ID_max_len)
                        ID_max_len = l;
                    if (l < ID_min_len)
                        ID_min_len = l;
                }

                // Задание 3
                else if (tok == (int)Tok.INUM)
                    INUM_sum += scanner.LexValueInt;
                else if (tok == (int)Tok.RNUM)
                    RNUM_sum += scanner.LexValueDouble;

                Console.WriteLine(scanner.TokToString((Tok)tok));
            } while (true);

            Console.WriteLine("\nID_count = " + ID_count.ToString());
            Console.WriteLine("ID_max_len = " + ID_max_len.ToString());
            Console.WriteLine("ID_min_len = " + ID_min_len.ToString());
            Console.WriteLine("ID_avg_len = " + 
                (ID_sum_len / (double)ID_count).ToString());
            Console.WriteLine("Integer sum = " + INUM_sum.ToString());
            Console.WriteLine("Real sum = " + RNUM_sum.ToString());

            Console.ReadKey();
        }
    }
}
