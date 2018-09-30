using System;
using NUnit.Framework;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Visitors;

namespace TestVisitors
{
    public class TestHelpers
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
    public class TestAvgOpCount
    {
        [Test]
        public void NoLoopTest()
        {
            Parser p = TestHelpers.Parse(@"begin end ");
            Assert.IsTrue(p.Parse());
            var avgCounter = new CountCyclesOpVisitor();
            p.root.Visit(avgCounter);
            Assert.AreEqual(0, avgCounter.MidCount());            
        }
             
        [Test]
        public void ThreeLoopsTest()
        {
            Parser p = TestHelpers.Parse(@"begin
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
    public class TestCommonVariable
    {
        [Test]
        public void OneVarTest()
        {
            Parser p = TestHelpers.Parse(@"begin var a; a:=2; a:=a+2*a-3; a:=3; end ");
            Assert.IsTrue(p.Parse());
            var varCounter = new CommonlyUsedVarVisitor();
            p.root.Visit(varCounter);
            Assert.AreEqual("a", varCounter.mostCommonlyUsedVar());            
        }
        
        [Test]
        public void ManyVarTest()
        {
            Parser p = TestHelpers.Parse(@"begin var a,b,c; a:=2+c-b; b:=a+2*a-3-b+b-b+b+b; b:=c-3+b-3; end ");
            Assert.IsTrue(p.Parse());
            var varCounter = new CommonlyUsedVarVisitor();
            p.root.Visit(varCounter);
            Assert.AreEqual("b", varCounter.mostCommonlyUsedVar());            
        }
    }
    
    [TestFixture]
    public class TestExprComplexity
    {
        [Test]
        public void AssignTest()
        {
            Parser p = TestHelpers.Parse(@"begin var a; a:=2+2; a:=a+2*a-3; a:=3; end ");
            Assert.IsTrue(p.Parse());
            var exprMeter = new ExprComplexityVisitor();
            p.root.Visit(exprMeter);
            var resultList = exprMeter.getComplexityList();
            CollectionAssert.AreEqual(new int[] {1, 5, 0}, resultList);            
        }
        
        [Test]
        public void CycleTest()
        {
            Parser p = TestHelpers.Parse(@"begin var a; cycle 2+2/3 a:=2-2 end ");
            Assert.IsTrue(p.Parse());
            var exprMeter = new ExprComplexityVisitor();
            p.root.Visit(exprMeter);
            var resultList = exprMeter.getComplexityList();
            CollectionAssert.AreEqual(new int[] {4, 1}, resultList);            
        }
        
        [Test]
        public void WriteTest()
        {
            Parser p = TestHelpers.Parse(@"begin write(2+2-3) end ");
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
                Parser p = TestHelpers.Parse(@"begin cycle 2 write(2) end");
                Assert.IsTrue(p.Parse());
                var loopCounter = new MaxNestCyclesVisitor();
                p.root.Visit(loopCounter);
                Assert.AreEqual(1, loopCounter.MaxNest);            
            }
            
            [Test]
            public void ThreeLoopsTest()
            {
                Parser p = TestHelpers.Parse(@"begin cycle 2 cycle 3 cycle 4 write(5) end");
                Assert.IsTrue(p.Parse());
                var loopCounter = new MaxNestCyclesVisitor();
                p.root.Visit(loopCounter);
                Assert.AreEqual(3, loopCounter.MaxNest);            
            }
            
            [Test]
            public void LoopTreeTest()
            {
                Parser p = TestHelpers.Parse(@"begin var a; 
                                                    cycle 2 
                                                        cycle 1 
                                                            a:=2; 
                                                        cycle 3 
                                                            cycle 5 
                                                                cycle 6 
                                                                    a:=5; 
                                                                cycle 4 
                                                                    write(5) 
                                              end");
                Assert.IsTrue(p.Parse());
                var loopCounter = new MaxNestCyclesVisitor();
                p.root.Visit(loopCounter);
                Assert.AreEqual(4, loopCounter.MaxNest);            
            }
           
        }
       
    }

    [TestFixture]
    public class TestVariableRenamer 
    {
        [Test]
        public void SimpleTest()
        {
            Parser p = TestHelpers.Parse(@"begin var a; a:=2; a:=a+2*a-3; a:=3; end ");
            Assert.IsTrue(p.Parse());
            var varRenamer = new ChangeVarIdVisitor("a", "z");
            p.root.Visit(varRenamer);
            
            var varCounter = new CommonlyUsedVarVisitor();
            p.root.Visit(varCounter);
            Assert.AreEqual("z", varCounter.mostCommonlyUsedVar());
        }
    }
    
    [TestFixture]
    public class TestIfCycleNest 
    {
        [Test]
        public void FirstTest()
        {
            Parser p = TestHelpers.Parse(@"begin var a; cycle 3 if 1 then cycle 4 a := 1 end ");
            Assert.IsTrue(p.Parse());
            var nestWalker = new MaxIfCycleNestVisitor();
            p.root.Visit(nestWalker);
            Assert.AreEqual(3, nestWalker.MaxNest);
        }
    }

}