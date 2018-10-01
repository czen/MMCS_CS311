#!/bin/bash
mono NUnit.3.10.1/NUnit.ConsoleRunner.3.9.0/tools/nunit3-console.exe --labels=All TestLexer/bin/Debug/TestLexer.dll
mono NUnit.3.10.1/NUnit.ConsoleRunner.3.9.0/tools/nunit3-console.exe --labels=All TestSimpleLexer/bin/Debug/TestSimpleLexer.dll
mono NUnit.3.10.1/NUnit.ConsoleRunner.3.9.0/tools/nunit3-console.exe --labels=All TestGeneratedLexer/bin/Debug/TestGeneratedLexer.dll
mono NUnit.3.10.1/NUnit.ConsoleRunner.3.9.0/tools/nunit3-console.exe --labels=All TestDescentParser/bin/Debug/TestDescentParser.dll
mono NUnit.3.10.1/NUnit.ConsoleRunner.3.9.0/tools/nunit3-console.exe --labels=All TestGeneratedParser/bin/Debug/TestGeneratedParser.dll
mono NUnit.3.10.1/NUnit.ConsoleRunner.3.9.0/tools/nunit3-console.exe --labels=All TestASTParser/bin/Debug/TestASTParser.dll
mono NUnit.3.10.1/NUnit.ConsoleRunner.3.9.0/tools/nunit3-console.exe --labels=All TestVisitors/bin/Debug/TestVisitors.dll
mono NUnit.3.10.1/NUnit.ConsoleRunner.3.9.0/tools/nunit3-console.exe --labels=All TestCodeGenerator/bin/Debug/TestCodeGenerator.dll
