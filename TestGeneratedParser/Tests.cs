using System;
using System.IO;
using NUnit.Framework;
using SimpleScanner;
using SimpleParser;

namespace TestGeneratedParser
{
    [TestFixture]
    public class GeneratedParserTests
    {
        private bool Parse(string text)
        {
            Scanner scanner = new Scanner();
            scanner.SetSource(text, 0);
            
            Parser parser = new Parser(scanner);
                      
            return parser.Parse();
        }
        
        [Test]
        public void TestWhile()
        {
            Assert.True(Parse(@"begin while 2 do a:=2 end"));
        }
        
        [Test]
        public void TestRepeat()
        {
            Assert.True(Parse(@"begin repeat a:=2; b:=3 until 3 end"));
        }
        
        [Test]
        public void TestFor()
        {
            Assert.True(Parse(@"begin for i:= 1 to 5 do a:=2 end"));
            
            Assert.True(Parse(@"begin for i:= 1 to 5 do if 3 then a:=2 else a:=5 end"));
        }
        
        [Test]
        public void TestWrite()
        {
            Assert.True(Parse(@"begin for i:= 1 to 5 do begin a:=2; write(a) end end"));
            
            Assert.True(Parse(@"begin if 2 then write(3) else write(4) end"));
            
            Assert.True(Parse(@"begin repeat write(5) until 2 end"));
        }
        
        [Test]
        public void TestIf()
        {
            Assert.True(Parse(@"begin if 2 then a:=3 else c:=8; if 3 then if 5 then z:=0 else n:=12 else g:=7 end"));
            
            Assert.True(Parse(@"begin while 1 do begin if 2 then a:=1 else b:=2 end end"));
        }
        
        [Test]
        public void TestVar()
        {
            Assert.True(Parse(@"begin var a,b,d end"));
        }
        
        [Test]
        public void TestExr()
        {
            Assert.True(Parse(@"begin a:=x-z*3/(c+3-(ddz)+2) end"));
            
            Assert.True(Parse(@"begin for i:=2+2*(c-3) to 5+6*2 do begin a:=x-z*3/(c+3-(ddz)+2); if (2-2) then c:=a-2 else write(2+2) end end"));
        }
    }
}