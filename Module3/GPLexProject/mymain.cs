using System;
using System.IO;
using SimpleScanner;
using ScannerHelper;
using System.Collections.Generic;

namespace Main
{
    class mymain
    {


        static void test(string fname) {
            Console.WriteLine("Testing file: " +fname);
            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%");
            Console.WriteLine(File.ReadAllText(fname));
            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%");
            Scanner scanner = new Scanner(new FileStream(fname, FileMode.Open));
            scanner.IdList = new List<string>();
            int tok = 0;
            do
            {
                tok = scanner.yylex();

                if (tok == (int)Tok.EOF)
                    break;
                Console.WriteLine(scanner.TokToString((Tok)tok));
            } while (true);
            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%");
            Console.WriteLine(String.Format("ID count:{0} ; maxlen:{1}; minlen:{2}; avglen:{3};", scanner.LexCount, scanner.LexMaxLength, scanner.LexMinLength, scanner.LexAvgLength()));
            Console.WriteLine(String.Format("Sum int: {0}; double: {1}",scanner.LexIntSum,scanner.LexDoubleSum));
            Console.WriteLine(String.Format("IDs in MLcomment: {0}",String.Join(", ",scanner.IdList.ToArray())));
            Console.WriteLine("--------------------------------------------------\n");
        }

        static void Main(string[] args)
        {
            // ����� ������������ ����� �������������� � ������������ � ������� 3.14 (� �� 3,14 ��� � ������� Culture)
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            List<string> fnames = new List<string>{ @"..\..\a.txt", @"..\..\b.txt", @"..\..\c.txt" };
            foreach (var name in fnames) {
                test(name);
            }
            Console.ReadKey();
        }
    }
}
