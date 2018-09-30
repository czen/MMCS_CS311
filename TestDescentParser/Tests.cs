using System;
using NUnit.Framework;
using SimpleLexer;
using SimpleLangParser;
using System.IO;

namespace TestDescentParser
{
    [TestFixture]
    public class DescentParserTests
    {
        private bool Parse(string text)
        {
            TextReader inputReader = new StringReader(text);
            Lexer l = new Lexer(inputReader);
            Parser p = new Parser(l);
            p.Progr();
            if (l.LexKind == Tok.EOF)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        [Test]
        public void TestWhile()
        {
            Assert.IsTrue(Parse(@"begin while 5 do a:=2 end"));
            
            Assert.IsTrue(Parse(@"begin 
                                     while 5 do 
                                      begin 
                                       a:=2 
                                     end
                                  end"));
                                  
            Assert.IsTrue(Parse(@"begin 
                                     while 5 do 
                                      begin 
                                       while 6 do 
                                        a:=2; 
                                       while 7 do 
                                       begin
                                         a:=3;
                                         c:=4
                                       end
                                     end
                                  end"));
            
        }
        
        [Test]
        public void TestFor()
        {
            Assert.IsTrue(Parse(@"begin 
                                     for a:=1 to 5 do 
                                      begin 
                                       b:=b+1 
                                     end
                                  end"));
                                  
           Assert.IsTrue(Parse(@"begin 
                                     for a:=1 to 5 do 
                                      begin 
                                       for i:=1 to 6 do
                                          c:=c-1;
                                       b:=b+1 
                                     end
                                  end"));
            
        }
        
        [Test]
        public void TestIf()
        {
            Assert.IsTrue(Parse(@"begin 
                                     if 2 then  
                                        a:=2
                                     else 
                                        b:=2;

                                     if 3 then
                                        if c then
                                            c:=4
                                         else
                                            m:=1
                                     else
                                        v:=8;   
                                    
                                     if 4 then
                                       if 4 then
                                         if 6 then
                                            m:=0
                                  end"));
            
        }
        
        [Test]
        public void TestExpr()
        {
            Assert.IsTrue(Parse(@"begin 
                                     if 2+2*(c-d/3) then
                                        begin  
                                            a:=2;
                                            while 2-3+f do c:=c*2
                                        end
                                     else 
                                        b:=2-3*(c-d/f*3);
                                    
                                     for i:=2-3*(s-d) to (c-3) do
                                         a:=(a-(3-3));

                                     if 3 then
                                        if (c-3) then
                                            c:=4+2
                                         else
                                            m:=1
                                     else
                                        v:=(8+2)   
                                  end"));
            
        }
    }
}