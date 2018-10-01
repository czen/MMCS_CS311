using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class ChangeVarIdVisitor : AutoVisitor
    {
        private string from, to;

        public ChangeVarIdVisitor(string _from, string _to)
        {
            from = _from;
            to = _to;
        }
       
    }
}
