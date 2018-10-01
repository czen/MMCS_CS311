using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class ExprComplexityVisitor : AutoVisitor
    {
        // список должен содержать сложность каждого выражения, встреченного при обычном порядке обхода AST
        public List<int> getComplexityList()
        {
            throw new NotImplementedException();
        }

     }
}
