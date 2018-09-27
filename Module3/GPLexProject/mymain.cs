using System;
using System.IO;
using SimpleScanner;
using ScannerHelper;
using System.Collections.Generic;


namespace Main
{
    class mymain
    {
        static void Main(string[] args)
        {
            // ����� ������������ ����� �������������� � ������������ � ������� 3.14 (� �� 3,14 ��� � ������� Culture)
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            //var fname = @"..\..\a.txt";
            var names = new List<string>();
            names.Add(@"..\..\bbb.txt");//���������� �������
            names.Add(@"..\..\a.txt"); // ������������ �������

            foreach (var name in names)
            {
                Console.WriteLine("\n \n-------------------------");
                Console.WriteLine("����: {0}", name);
                Console.WriteLine("-------------------------");
                
                Console.WriteLine(File.ReadAllText(name));
                Console.WriteLine("-------------------------");

                Scanner scanner = new Scanner(new FileStream(name, FileMode.Open));
                var idLength = new List<int>();
                var id2All = new List<string>();
                int intSum = 0;
                double doubleSum = 0.0;

                int tok = 0;
                do
                {
                    tok = scanner.yylex();

                    if (tok == (int)Tok.EOF)
                        break;

                    if (tok == (int)Tok.ID || tok == (int)Tok.ID2)
                        idLength.Add(scanner.yytext.Length);

                    if (tok == (int)Tok.ID2)
                        id2All.Add(scanner.yytext);

                    if (tok == (int)Tok.INUM)
                        intSum += scanner.LexValueInt;

                    if (tok == (int)Tok.RNUM)
                        doubleSum += scanner.LexValueDouble;

                    Console.WriteLine(scanner.TokToString((Tok)tok));

                } while (true);

                double totalLength = 0;
                int maxLength = int.MinValue;
                int minLength = int.MaxValue;

                foreach (var item in idLength)
                {
                    totalLength += item;
                    if (item > maxLength)
                        maxLength = item;
                    if (item < minLength)
                        minLength = item;
                }

                Console.WriteLine();
                Console.WriteLine("���������� ���������������: {0}", idLength.Count);
                Console.WriteLine("������� ������ ���������������: {0}", totalLength / idLength.Count);
                Console.WriteLine("����������� ������ ���������������: {0}", minLength);
                Console.WriteLine("������������ ������ ���������������: {0}", maxLength);
                Console.WriteLine("����� ����� �����: {0}", intSum);
                Console.WriteLine("����� ������������ �����: {0}", doubleSum);
                Console.WriteLine("����� ����� �����: {0}", intSum + doubleSum);

                //extra task 4
                if (id2All.Count !=0)
                    Console.WriteLine("\n��������������, ������������� ������ �������������� �����������");
                foreach (var item in id2All)
                {
                    Console.WriteLine("{0} \n", item);
                }

            }
            Console.ReadKey();
        }
    }
}
