using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public class MaxNestCyclesVisitor : AutoVisitor
    {
        public int MaxNest = 0;
    }
}
