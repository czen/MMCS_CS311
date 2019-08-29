using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NunitReport
{
    class ReportParser
    {
        private static List<XmlNode> cases = new List<XmlNode>();
        private static string basePath = "";

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
                else
                {
                    ParseTestSuite(child);   
                }
            }
        }

        static void CountGrades()
        {
            string configPath = @"../../../grades/Grades.json"; 
            if (basePath != "")
            {
                configPath = basePath + @"/grades/Grades.json";
            }
            
            JObject grades = JObject.Parse(File.ReadAllText(configPath));

            JArray tasks = (JArray) grades["Tasks"];

            var countedGrades = new Dictionary<string, double>();
            foreach (XmlNode testcase in cases)
            {
                string caseClass;
                string caseName;
                try
                {
                    caseClass = testcase.Attributes["classname"].Value.Split('.')[0];
                    caseName = testcase.Attributes["name"].Value;
                } catch(Exception e)
                {
                    string[] fullName = testcase.Attributes["name"].Value.Split('.');
                    caseClass = fullName[0];
                    caseName = fullName[fullName.Length - 1];
                }

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
            string filePath;
            if (args.Length > 0)
            {
                basePath = args[0];
                filePath = basePath + @"/TestResult.xml";
                
            }
            else
            {
                filePath = @"./TestResult.xml";
            }
            doc.Load(filePath);
            string[] XmlText = File.ReadAllLines(filePath);
//            System.Console.WriteLine("+++++++++++++++++++++++++");
//            foreach (string line in XmlText)
//            {
//                System.Console.WriteLine(line);
//            }
//            System.Console.WriteLine("+++++++++++++++++++++++++");

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