using System;
using System.IO;
using System.Net;
using NUnit.Framework;
using SimpleScanner;
using SimpleParser;
using SimpleLang.Visitors;

namespace TestCodeGenerator
{
    public class TestHelper
    {
        public static Parser Parse(string text)
        {
            Scanner scanner = new Scanner();
            scanner.SetSource(text, 0);
            Parser parser = new Parser(scanner);
            Assert.IsTrue(parser.Parse());
            return parser;
        }

        public static string GenerateNRun(string text)
        {
            var parser = TestHelper.Parse(text);
            var code = new GenCodeVisitor();
            parser.root.Visit(code);
            code.EndProgram();
            //code.PrintCommands();
            string output = "";
            using(MemoryStream ms = new MemoryStream())
            {
                var sw = new StreamWriter(ms);
                try
                {
                    Console.SetOut(sw);
                    code.RunProgram();
                    sw.Flush();

                    ms.Seek(0, SeekOrigin.Begin);
                    var sr = new StreamReader(ms);        
                    output = sr.ReadToEnd().Trim();
                }
                finally
                {
                    sw.Dispose();
                }
            }
            return output;
        }
    }
    
    [TestFixture]
    public class TestCodeGenerator
    {
        [Test]
        public void SmokeTest()
        {
            var parser = TestHelper.Parse(@"begin end");
            var code = new GenCodeVisitor();
            parser.root.Visit(code);
            code.EndProgram();
            //code.PrintCommands();
            code.RunProgram();
        }
        
        [Test]
        public void TestOutput()
        {
            Assert.AreEqual("2", TestHelper.GenerateNRun(@"begin write(2) end"));
        }
        
        [Test]
        public void TestIntDivMod()
        {
            Assert.AreEqual("48", TestHelper.GenerateNRun(@"begin var a; a := (232 / 5) + (232 % 5); write(a) end"));
        }
        
        [Test]
        public void TestIf()
        {
            Assert.AreEqual("3", TestHelper.GenerateNRun(@"begin var a1; a1 := 0; if a1 then write(2) else write(3) end"));
            
            Assert.AreEqual("3", TestHelper.GenerateNRun(@"begin var x,y; x := 1; y := x-1; if x then if y then write(2) else write(3) end"));
        }
        
        [Test]
        public void TestWhile()
        {
            Assert.AreEqual("1024", TestHelper.GenerateNRun(@"begin var a2,b2; b2:=1; a2:=10; while a2 do begin a2:=a2-1; b2:=b2*2; end; write(b2) end"));
        }
        
        [Test]
        public void TestUntil()
        {
            Assert.AreEqual("1024", TestHelper.GenerateNRun(@"begin var a3,b3; b3:=1; a3:=10; repeat a3:=a3-1; b3:=b3*2 until a3; write(b3) end"));
        }
    }
}