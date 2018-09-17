using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SimpleLangLexer;

namespace SimpleLangLexerTest
{
    class Program
    {
        public static void Main()
        {
            string fileContents = @"begin 
id23 := 24;  
cycle ; 2 id258 id29 ; 
end";
            PrintTest(fileContents, 0);

            //task1: Добавить распознавание лексем , : + - * / div mod and or not
            string task1FileContents = @"begin 
id23 := 24;  
+ - , : * div / or and not mod ; 
end";
            PrintTest(task1FileContents, 1);
            //task2: Добавить распознавание лексем += -= *= /=. Тщательно продумать, как совместить распознавание лексем с одинаковым префиксом: например, + и +=
            string task2FileContents = @"begin 
id23 := 24;  
+ - -=+= /= *=**=*  
end";
            PrintTest(task2FileContents, 2);
            //task3: Добавить распознавание лексем > < >= <= = <>
            string task3FileContents = @"begin 
id23 := 24;  
< <> <=>>=<  
end";
            PrintTest(task3FileContents, 3);
            //task4: Добавить пропуск комментариев // - до конца строки
            string task4FileContents = @"begin 
id23 := 24;
//<
+
//-
//hello!";
            PrintTest(task4FileContents, 4);
            //task5: Добавить пропуск комментариев { комментарий до закрывающей фигурной скобки }. Обратить внимание, что незакрытый до конца файла комментарий - это синтаксическая ошибка
            string task5FileContents = @"begin 
id23 := 24;
{<
+
//-
//hello!}"; //для получения теста на ошибку можно убрать закрывающую фигурную скобку ('}')
            PrintTest(task5FileContents, 5);
        }

        private static void PrintTest(string testStr, int taskNumber)
        {
            Console.WriteLine("\\=====================================\\");
            Console.WriteLine("Task {0}:", taskNumber);
            TextReader taskReader = new StringReader(testStr);
            Lexer taskLexer = new Lexer(taskReader);
            try
            {
                do
                {
                    Console.WriteLine(taskLexer.TokToString(taskLexer.LexKind));
                    taskLexer.NextLexem();
                } while (taskLexer.LexKind != Tok.EOF);
            }
            catch (LexerException e)
            {
                Console.WriteLine("task1Lexer error: " + e.Message);
            }
            Console.WriteLine("\\=====================================\\");
        }
    }
}