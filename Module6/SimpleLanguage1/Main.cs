using System;
using System.IO;
using System.Collections.Generic;
using SimpleScanner;
using SimpleParser;
using System.Text;
using Newtonsoft.Json;

namespace SimpleCompiler
{
    public class SimpleCompilerMain
    {
        public static void Main()
        {
            string FileName = @"..\..\a.txt";
            string OutputFileName = @"..\..\a.json";
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
                    JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
                    jsonSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                    jsonSettings.TypeNameHandling = TypeNameHandling.All;
                    string output = JsonConvert.SerializeObject(parser.root, jsonSettings);
                    File.WriteAllText(OutputFileName, output);
                }
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

            Console.ReadLine();
        }

    }
}
