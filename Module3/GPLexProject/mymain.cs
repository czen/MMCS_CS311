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

            int tok = 0;
            int cnt_id = 0;
            double avg_len = 0d;
            int max_len = Int32.MinValue;
            int min_len = Int32.MaxValue;
            scanner.Ids = new System.Collections.Generic.List<String>();

            int int_sum = 0;
            double d_sum = 0d;

            do
            {
                tok = scanner.yylex();
                if (tok == (int)Tok.ID)
                {
                    ++cnt_id;
                    int len = scanner.yytext.Length;
                    avg_len += len;
                    if (len > max_len)
                        max_len = len;
                    if (len < min_len)
                        min_len = len;
                }
                if (tok == (int)Tok.INUM)
                {
                    int_sum += Convert.ToInt32(scanner.yytext);
                }
                if (tok == (int)Tok.RNUM)
                {
                    d_sum += Convert.ToDouble(scanner.yytext);
                }
                if (tok == (int)Tok.EOF)
                    break;
                Console.WriteLine(scanner.TokToString((Tok)tok));
            } while (true);

            avg_len /= cnt_id;
            Console.WriteLine(@"count of ids = {0}, average lenght = {1},
max lenght = {2}, min lenght = {3}", cnt_id, avg_len, max_len, min_len);
            Console.WriteLine("sum of inum = {0}, sum of rnum = {1}", int_sum, d_sum);

            Console.WriteLine("IDs in comments: ");
            foreach (String s in scanner.Ids)
                Console.WriteLine(s);

            Console.ReadKey();
        }
    }
}
