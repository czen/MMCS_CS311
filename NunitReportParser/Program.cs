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

        static void SendGrades(JObject settings, string nickname, int submoduleNumber, int semester, string service,
            string subject, double value)
        {
            //System.Console.Out.WriteLine("{0}-{1}-{2}-{3}", RecordBookID, SubmoduleID, DisciplineID, value);
            string gradeToken = (string) settings["GradeService"]["token"];
            string gradeUrl = (string) settings["GradeService"]["url"];
            string url = gradeUrl + "api/v0/sendGrades?token=" + gradeToken;
            //System.Console.Out.WriteLine(url);
            WebRequest request;
            request = WebRequest.Create(url);
            request.Method = "PUT";
            request.ContentType = "application/json charset=utf-8";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{\"subject\":\"" + subject + "\"," +
                              "\"nick\":\"" + nickname + "\"," +
                              "\"semester\":" + semester.ToString() + "," +
                              "\"service\":\"" + service + "\"," +
                              "\"submodules\": [" +
                              "{" +
                              "\"number\":" + submoduleNumber.ToString() + "," +
                              "\"value\":" + value.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                              "}" +
                              "]" +
                              "}";

                //System.Console.WriteLine(json);
                streamWriter.Write(json);
            }

            Stream objStream;
            var httpResponse = (HttpWebResponse) request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                System.Console.Out.WriteLine("grade response: ");
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

            JArray tasks = (JArray) grades["Projects"];

            var countedGrades = new Dictionary<int, double>();
            foreach (XmlNode testcase in cases)
            {
                string caseClass;
                string caseName;
                try
                {
                    caseClass = testcase.Attributes["classname"].Value.Split('.')[0];
                    caseName = testcase.Attributes["name"].Value;
                }
                catch (Exception e)
                {
                    string[] fullName = testcase.Attributes["name"].Value.Split('.');
                    caseClass = fullName[0];
                    caseName = fullName[fullName.Length - 1];
                }

                foreach (JObject task in tasks)
                {
                    string namespaceName = (string) task["namespace"];
                    if (namespaceName == caseClass)
                    {
                        double maxGrade = (double) task["tests"][caseName]["grade"];
                        int subModuleNumber = (int) task["tests"][caseName]["subModuleNumber"];
                        if (testcase.Attributes["result"].Value == "Passed" || testcase.Attributes["result"].Value == "Success")
                        {
                            if (!countedGrades.ContainsKey(subModuleNumber))
                            {
                                countedGrades[subModuleNumber] = 0;
                            }

                            countedGrades[subModuleNumber] += maxGrade;
                        }
                    }
                }

                //System.Console.Out.WriteLine(countedGrades[caseClass]);
            }

            string subject = (string) grades["Discipline"]["Subject"];
            int semester = (int) grades["Discipline"]["Semester"];
            string nick = userName;
            string service = (string) grades["Service"];

            foreach (KeyValuePair<int, double> g in countedGrades)
            {
                if (g.Value > 0)
                {
                    double grade = g.Value;
                    grade *= 0.6;

                    System.Console.Out.WriteLine("submodule #" + g.Key.ToString() + " = " + grade.ToString());

                    SendGrades(grades, nick, g.Key, semester, service, subject, grade);
                }
                else
                {
                    System.Console.Out.WriteLine("submodule #" + g.Key.ToString() + " = empty");
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
                //userName = args[1].Split('/')[0];
                string repoName = args[1].Split('/')[1];
                userName = repoName.Substring(23, repoName.Length - 23);
            }
            else
            {
                userName = "czen";
                filePath = @"./TestResult.xml";
            }
            System.Console.WriteLine("user name: " + userName);
            System.Console.WriteLine("base path: " + basePath);

            doc.Load(filePath);
            string[] XmlText = File.ReadAllLines(filePath);
//            System.Console.WriteLine("+++++++++++++++++++++++++");
//            System.Console.WriteLine("nunit report:");
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