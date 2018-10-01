using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    class PrettyPrintVisitor: Visitor
    {
        public string Text = "";
        private int Indent = 0;

        private string IndentStr()
        {
            return new string(' ', Indent);
        }
        private void IndentPlus()
        {
            Indent += 2;
        }
        private void IndentMinus()
        {
            Indent -= 2;
        }
        public override void VisitIdNode(IdNode id) 
        {
            Text += id.Name;
        }
        public override void VisitIntNumNode(IntNumNode num) 
        {
            Text += num.Num.ToString();
        }
        public override void VisitBinOpNode(BinOpNode binop) 
        {
            Text += "(";
            binop.Left.Visit(this);
            Text += " " + binop.Op + " ";
            binop.Right.Visit(this);
            Text += ")";
        }
        public override void VisitAssignNode(AssignNode a) 
        {
            Text += IndentStr();
            a.Id.Visit(this);
            Text += " := ";
            a.Expr.Visit(this);
        }
        public override void VisitCycleNode(CycleNode c) 
        {
            Text += IndentStr() + "cycle ";
            c.Expr.Visit(this);
            Text += Environment.NewLine;
            c.Stat.Visit(this);
        }
        public override void VisitBlockNode(BlockNode bl) 
        {
            Text += IndentStr() + "begin" + Environment.NewLine;
            IndentPlus();

            var Count = bl.StList.Count;

            if (Count>0)
                bl.StList[0].Visit(this);
            for (var i = 1; i < Count; i++)
            {
                Text += ';';
                if (!(bl.StList[i] is EmptyNode))
                    Text += Environment.NewLine;
                bl.StList[i].Visit(this);
            }
            IndentMinus();
            Text += Environment.NewLine + IndentStr() + "end";
        }
        public override void VisitWriteNode(WriteNode w) 
        {
            Text += IndentStr() + "write(";
            w.Expr.Visit(this);
            Text += ")";
        }
        public override void VisitVarDefNode(VarDefNode w) 
        {
            Text += IndentStr() + "var " + w.vars[0].Name;
            for (int i = 1; i < w.vars.Count; i++)
                Text += ',' + w.vars[i].Name;
        }
    }
}
