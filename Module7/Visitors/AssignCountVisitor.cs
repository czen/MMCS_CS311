using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class AssignCountVisitor : AutoVisitor
    {
        public int Count = 0;
        
    }
}
