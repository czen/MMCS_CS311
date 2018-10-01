using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace SimpleLang.Visitors
{
    class GenCodeCreator
    {
        private DynamicMethod dyn;
        private ILGenerator gen;
        private bool write_commands = true;
        private static MethodInfo writeLineInt = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) });

        public List<string> commands = new List<string>();

        public GenCodeCreator()
        {
            dyn = new DynamicMethod("My", null, null, typeof(void));
            gen = dyn.GetILGenerator();
        }

        public void Emit(OpCode op)
        {
            gen.Emit(op);
            if (write_commands)
                commands.Add(op.ToString());
        }

        public void Emit(OpCode op, int num)
        {
            gen.Emit(op,num);
            if (write_commands)
                commands.Add(op.ToString() + " " + num);
        }

        public void Emit(OpCode op, LocalBuilder lb)
        {
            gen.Emit(op, lb);
            if (write_commands)
                commands.Add(op.ToString() + " var" + lb.LocalIndex);
        }

        public void Emit(OpCode op, Label l)
        {
            gen.Emit(op, l);
            if (write_commands)
                commands.Add(op.ToString() + " Label" + l.GetHashCode());
        }

        public LocalBuilder DeclareLocal(Type t)
        {
            var lb = gen.DeclareLocal(t);
            if (write_commands)
                commands.Add("DeclareLocal " + "var" + lb.LocalIndex + ": " + t);
            return lb;
        }

        public Label DefineLabel()
        {
            var l = gen.DefineLabel();
            if (write_commands)
                commands.Add("DefineLabel" + " Label" + l.GetHashCode());

            return l;
        }

        public void MarkLabel(Label l)
        {
            gen.MarkLabel(l);
            if (write_commands)
                commands.Add("MarkLabel" + " Label" + l.GetHashCode());
        }

        public void EmitWriteLine()
        {
            gen.Emit(OpCodes.Call, writeLineInt);
            if (write_commands)
                commands.Add("WriteLine");
        }

        public void EndProgram()
        {
            gen.Emit(OpCodes.Ret);
        }

        public void RunProgram()
        {
            dyn.Invoke(null, null);
        }

        public void WriteCommandsOn()
        {
            write_commands = true;
        }

        public void WriteCommandsOff()
        {
            write_commands = false;
        }
    }
}
