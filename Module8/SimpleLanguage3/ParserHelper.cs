using System.Collections.Generic;
using System;

namespace SimpleParser
{

    public enum type { tint, tdouble };

    public static class SymbolTable // Таблица символов
    {
        public static Dictionary<string, type> vars = new Dictionary<string, type>(); // таблица символов
        public static void NewVarDef(string name, type t)
        {
            if (vars.ContainsKey(name))
                throw new Exception("Переменная " + name + " уже определена");
            else vars.Add(name, t);
        }
    }

    public class LexException : Exception
    {
        public LexException(string msg) : base(msg) { }
    }
    
    public class SyntaxException : Exception
    {
        public SyntaxException(string msg) : base(msg) { }
    }

    public static class ParserHelper
    {
    }
}