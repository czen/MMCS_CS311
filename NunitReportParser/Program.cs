using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Net.Mime;
using System.Runtime.InteropServices;

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

        static void SendGrades(JObject settings, int recordBookID, int submoduleID, int disciplineID, double value)
        {
            //System.Console.Out.WriteLine("{0}-{1}-{2}-{3}", RecordBookID, SubmoduleID, DisciplineID, value);
            string gradeToken = (string) settings["GradeService"]["token"];
            string gradeUrl = (string) settings["GradeService"]["url"];
            string url = gradeUrl + "api/v0/sendGrades?token=" + gradeToken;
            System.Console.Out.WriteLine(url);
            WebRequest request;
            request = WebRequest.Create(url);
            request.Method = "PUT";
            request.ContentType = "application/json charset=utf-8";
            
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{\"discipline\":"+ disciplineID.ToString() +"," +
                               "\"recordbook\":"+ recordBookID.ToString() +"," +
                               "\"submodules\": [" +
                               "{" +
                               "\"id\":" + submoduleID.ToString() + "," + 
                               "\"value\":" + value.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                               "}" +
                               "]" +
                              "}";

                System.Console.WriteLine(json);
                streamWriter.Write(json);
            }
            
            Stream objStream;
            var httpResponse = (HttpWebResponse) request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                System.Console.Out.WriteLine(result);
            }
        }

        static void CountGrades(string userName)
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

            int discipline = (int) grades["Discipline"]["ID"];
            
            foreach (KeyValuePair<string, double> g in countedGrades)
            {
                if (g.Value > 0)
                {
                    double grade = g.Value;
                    grade *= 0.6;
                    
                    System.Console.Out.WriteLine(g.Key + " = " + grade.ToString());

                    int recordBook = (int) grades["RecordBooks"][userName];
                    JArray allTasks = (JArray) grades["Tasks"];
                    foreach (JObject task in allTasks)
                    {
                        if ((string) task["name"] == g.Key)
                        {
                            JArray modules = (JArray) task["modules"];
                            foreach (JObject module in modules)
                            {
                                int moduleid = (int) module["id"];
                                double w = (double) module["weight"];
                                double v = w * grade;
                                SendGrades(grades, recordBook, moduleid, discipline, v);            
                            }
                        }   
                    }
                    
                }
            }
            
            
        }

        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            string filePath;
            string userName;
            if (args.Length > 0)
            {
                basePath = args[0];
                filePath = basePath + @"/TestResult.xml";
                userName = args[1].Split('/')[0];
            }
            else
            {
                userName = "czen";
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

            CountGrades(userName);

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