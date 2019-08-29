using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NunitReport
{
    class ReportParser
    {
        private static List<XmlNode> cases = new List<XmlNode>();
        
        static void ParseTestSuite(XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "test-suite")
                {
                    ParseTestSuite(child);
                }
                else if (child.Name == "test-case")
                {
                    cases.Add(child);
                }
            }
        }

        static void CountGrades()
        {
            
            string configPath = @"../../../grades/Grades.json";

            JObject grades = JObject.Parse(File.ReadAllText(configPath));

            JArray tasks = (JArray) grades["Tasks"];

            var countedGrades = new Dictionary<string, double>();
            foreach (XmlNode testcase in cases)
            {
                string caseClass = testcase.Attributes["classname"].Value.Split('.')[0];
                string caseName = testcase.Attributes["name"].Value;
                    
                foreach (JObject task in tasks)
                {
                    if ((string) task["name"] == caseClass)
                    {
                        double maxGrade = (double) task["grades"][caseName];
                        if (testcase.Attributes["result"].Value == "Passed")
                        {
                            if (!countedGrades.ContainsKey(caseClass))
                            {
                                countedGrades[caseClass] = 0;
                            }
                            countedGrades[caseClass] += maxGrade;
                        }
                    }
                } 
                //System.Console.Out.WriteLine(countedGrades[caseClass]);
            }

            foreach (KeyValuePair<string, double> g in countedGrades)
            {
                if (g.Value > 0)
                {
                    double grade = g.Value;
                    grade *= 0.6;
                    System.Console.Out.WriteLine(g.Key + " = " + grade.ToString());
                }
            }

        }
        
        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"../../../TestResult.xml");

            // XmlNodeList nodes = doc.DocumentElement.SelectNodes("test-run/test-suite"); 
            XmlNodeList nodes = doc.DocumentElement.ChildNodes;


            foreach (XmlNode node in nodes)
            {
                Console.Out.WriteLine(node.Name);
                if (node.Name == "test-suite")
                {
                    ParseTestSuite(node);
                }
            }

            CountGrades();

            System.Console.WriteLine("number of running tests: " + cases.Count);
        }
    }

    class ReportNode
    {
        public string id;
         public string title;
        public string author;
    }
}  