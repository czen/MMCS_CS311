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
            // ����� ������������ ����� �������������� � ������������ � ������� 3.14 (� �� 3,14 ��� � ������� Culture)
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            var fname = @"..\..\a.txt";
            Console.WriteLine(File.ReadAllText(fname));
            Console.WriteLine("-------------------------");

            Scanner scanner = new Scanner(new FileStream(fname, FileMode.Open));

            int tok = 0;
            do {
                tok = scanner.yylex();
                if (tok == (int)Tok.EOF)
                    break;
                Console.WriteLine(scanner.TokToString((Tok)tok));
            } while (true);
            Console.WriteLine("\nInfo about Id: \n" + scanner.IdInfo());
            Console.WriteLine("\nInfo about numbers: \n" + scanner.NumsInfo());
            Console.WriteLine("\nInfo about Id in multiline comments: \n" + scanner.IdInCommentsInfo());

            Console.ReadKey();
        }
    }
}
