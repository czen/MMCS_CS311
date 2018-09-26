using System;
using System.IO;
using System.Collections.Generic;
using SimpleScanner;
using SimpleParser;

namespace SimpleCompiler
{
    public class SimpleCompilerMain
    {
        public static void Test(string FileName)
        {
            try
            {
                string Text = File.ReadAllText(FileName);

                Scanner scanner = new Scanner();
                scanner.SetSource(Text, 0);

                Parser parser = new Parser(scanner);

                var b = parser.Parse();
                if (!b)
                    Console.WriteLine("Ошибка");
                else Console.WriteLine("Программа распознана");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл {0} не найден", FileName);
            }
            catch (LexException e)
            {
                Console.WriteLine("Лексическая ошибка. " + e.Message);
            }
            catch (SyntaxException e)
            {
                Console.WriteLine("Синтаксическая ошибка. " + e.Message);
            }
        }

        public static void Main()
        {
            int tests_cnt = 4;
            for (int i = 0; i < tests_cnt; ++i)
            {
                Test(@"..\..\" + i.ToString() + ".txt");
            }
              

         //   Test(@"..\..\2.txt");

            Console.ReadLine();
        }

    }
}
