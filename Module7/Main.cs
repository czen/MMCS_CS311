using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Visitors;

namespace SimpleCompiler
{
    public class SimpleCompilerMain
    {
        public static void Main()
        {
            string FileName = @"..\..\a.txt";
            try
            {
                string Text = File.ReadAllText(FileName);

                Scanner scanner = new Scanner();
                scanner.SetSource(Text, 0);

                Parser parser = new Parser(scanner);

                var b = parser.Parse();
                if (!b)
                    Console.WriteLine("Ошибка");
                else
                {
                    Console.WriteLine("Синтаксическое дерево построено");

                    var pp = new PrettyPrintVisitor();
                    parser.root.Visit(pp);
                    Console.WriteLine(pp.Text);
                    Console.WriteLine("-------------------------------");

                    var avis = new AssignCountVisitor();
                    parser.root.Visit(avis);
                    Console.WriteLine("Количество присваиваний = {0}", avis.Count);
                    Console.WriteLine("-------------------------------");

                    var midCount = new CountCyclesOpVisitor();
                    parser.root.Visit(midCount);
                    Console.WriteLine("Среднее количество операторов = {0}", midCount.MidCount());
                    Console.WriteLine("-------------------------------");

                    var cuv = new CommonlyUsedVarVisitor();
                    parser.root.Visit(cuv);
                    Console.WriteLine("Наиболее часто используемая переменная = {0}", cuv.mostCommonlyUsedVar());
                    Console.WriteLine("-------------------------------");

                    var cviv = new ChangeVarIdVisitor("a", "d");
                    parser.root.Visit(cviv);
                    Console.WriteLine("Переименование переменной a на d:");
                    pp = new PrettyPrintVisitor();
                    parser.root.Visit(pp);
                    Console.WriteLine(pp.Text);
                    Console.WriteLine("-------------------------------");

                    var mncv = new MaxNestCyclesVisitor();
                    parser.root.Visit(mncv);
                    Console.WriteLine("Максимальная вложенность циклов = {0}", mncv.MaxNest);
                    Console.WriteLine("-------------------------------");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл {0} не найден", FileName);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}", e);
            }

            Console.ReadLine();
        }

    }
}
