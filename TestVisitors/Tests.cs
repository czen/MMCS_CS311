using System;
using System.Security.Cryptography;
using NUnit.Framework;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Visitors;

namespace TestVisitors
{
    public class ParserTest
    {
        public static Parser Parse(string text)
        {
            Scanner scanner = new Scanner();
            scanner.SetSource(text, 0);

            Parser parser = new Parser(scanner);
            return parser;
        }
    }
    
    [TestFixture]
    public class TestAvgOpCount: ParserTest
    {
        [Test]
        public void NoLoopTest()
        {
            Parser p = Parse(@"begin end ");
            Assert.IsTrue(p.Parse());
            var avgCounter = new CountCyclesOpVisitor();
            p.root.Visit(avgCounter);
            Assert.AreEqual(0, avgCounter.MidCount());            
        }
             
        [Test]
        public void ThreeLoopsTest()
        {
            Parser p = Parse(@"begin
       var a,b,d;
       b := 2;
       a := 3;
       a := a * 4 + b;;;
       cycle 3
       begin
         a := a + 1;
         cycle 3
         begin
           a := 1;
           a := 1;
           a := 1;
           a := 1;
           a := 1;
           a := 1;
           a := 1;
           a := 1;
           a := 1;
           a := 1;
         end;
         write(a)
       end;
       cycle 3
       begin
         d := 2
       end
     end");
            Assert.IsTrue(p.Parse());
            var avgCounter = new CountCyclesOpVisitor();
            p.root.Visit(avgCounter);
            Assert.AreEqual(4, avgCounter.MidCount());            
        }
    }
    
    [TestFixture]
    public class TestCommonVariable: ParserTest
    {
        [Test]
        public void OneVarTest()
        {
            Parser p = Parse(@"begin var a0; a0:=2; a0:=a0+2*a0-3; a0:=3; end ");
            Assert.IsTrue(p.Parse());
            var varCounter = new CommonlyUsedVarVisitor();
            p.root.Visit(varCounter);
            Assert.AreEqual("a0", varCounter.mostCommonlyUsedVar());            
        }
        
        [Test]
        public void ManyVarTest()
        {
            Parser p = Parse(@"begin var a1,b1,c1; a1:=2+c1-b1; b1:=a1+2*a1-3-b1+b1-b1+b1+b1; b1:=c1-3+b1-3; end ");
            Assert.IsTrue(p.Parse());
            var varCounter = new CommonlyUsedVarVisitor();
            p.root.Visit(varCounter);
            Assert.AreEqual("b1", varCounter.mostCommonlyUsedVar());            
        }
    }
    
    [TestFixture]
    public class TestExprComplexity: ParserTest
    {
        [Test]
        public void AssignTest()
        {
            Parser p = Parse(@"begin var a2; a2:=2+2; a2:=a2+2*a2-3; a2:=3; end ");
            Assert.IsTrue(p.Parse());
            var exprMeter = new ExprComplexityVisitor();
            p.root.Visit(exprMeter);
            var resultList = exprMeter.getComplexityList();
            CollectionAssert.AreEqual(new int[] {1, 5, 0}, resultList);            
        }
        
        [Test]
        public void CycleTest()
        {
            Parser p = Parse(@"begin var a3; cycle 2+2/3 a3:=2-2 end ");
            Assert.IsTrue(p.Parse());
            var exprMeter = new ExprComplexityVisitor();
            p.root.Visit(exprMeter);
            var resultList = exprMeter.getComplexityList();
            CollectionAssert.AreEqual(new int[] {4, 1}, resultList);            
        }
        
        [Test]
        public void WriteTest()
        {
            Parser p = Parse(@"begin write(2+2-3) end ");
            Assert.IsTrue(p.Parse());
            var exprMeter = new ExprComplexityVisitor();
            p.root.Visit(exprMeter);
            var resultList = exprMeter.getComplexityList();
            CollectionAssert.AreEqual(new int[] {2}, resultList);            
        }
        
        [TestFixture]
        public class TestLoopNestVisitor
        {
            [Test]
            public void OneLoopTest()
            {
                Parser p = Parse(@"begin cycle 2 write(2) end");
                Assert.IsTrue(p.Parse());
                var loopCounter = new MaxNestCyclesVisitor();
                p.root.Visit(loopCounter);
                Assert.AreEqual(1, loopCounter.MaxNest);            
            }
            
            [Test]
            public void ThreeLoopsTest1()
            {
                Parser p = Parse(@"begin cycle 2 cycle 3 cycle 4 write(5) end");
                Assert.IsTrue(p.Parse());
                var loopCounter = new MaxNestCyclesVisitor();
                p.root.Visit(loopCounter);
                Assert.AreEqual(3, loopCounter.MaxNest);            
            }
            
            [Test]
            public void LoopTreeTest()
            {
                Parser p = Parse(@"begin var a6; 
                                                    cycle 2 
                                                    begin
                                                        cycle 1 
                                                            a6:=2; 
                                                        cycle 3 
                                                            cycle 5 
                                                            begin
                                                                cycle 6 
                                                                    a6:=5; 
                                                                cycle 4 
                                                                    write(5) 
                                                            end
                                                    end
                                              end");
                Assert.IsTrue(p.Parse());
                var loopCounter = new MaxNestCyclesVisitor();
                p.root.Visit(loopCounter);
                Assert.AreEqual(4, loopCounter.MaxNest);            
            }
           
        }
       
    }

    [TestFixture]
    public class TestVariableRenamer: ParserTest 
    {
        [Test]
        public void SimpleTest()
        {
            Parser p = Parse(@"begin var a4; a4:=2; a4:=a4+2*a4-3; a4:=3; end ");
            Assert.IsTrue(p.Parse());
            var varRenamer = new ChangeVarIdVisitor("a4", "z");
            p.root.Visit(varRenamer);
            
            var varCounter = new CommonlyUsedVarVisitor();
            p.root.Visit(varCounter);
            Assert.AreEqual("z", varCounter.mostCommonlyUsedVar());
        }
    }
    
    [TestFixture]
    public class TestIfCycleNest: ParserTest
    {
        [Test]
        public void FirstTest()
        {
            Parser p = Parse(@"begin var a5; cycle 3 if 1 then cycle 4 a5 := 1 end ");
            Assert.IsTrue(p.Parse());
            var nestWalker = new MaxIfCycleNestVisitor();
            p.root.Visit(nestWalker);
            Assert.AreEqual(3, nestWalker.MaxNest);
        }
    }

}
