using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public abstract class Visitor
    {
        public virtual void VisitIdNode(IdNode id) { }
        public virtual void VisitIntNumNode(IntNumNode num) { }
        public virtual void VisitBinOpNode(BinOpNode binop) { }
        public virtual void VisitAssignNode(AssignNode a) { }
        public virtual void VisitCycleNode(CycleNode c) { }
        public virtual void VisitBlockNode(BlockNode bl) { }
        public virtual void VisitWriteNode(WriteNode w) { }
        public virtual void VisitVarDefNode(VarDefNode w) { }
        public virtual void VisitEmptyNode(EmptyNode w) { }
        public virtual void VisitIfNode(IfNode cond) { }
    }
}
