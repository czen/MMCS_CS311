#!/bin/bash
mono packages/NUnit.ConsoleRunner.3.10.0/tools/nunit3-console.exe --labels=All TestLexer/bin/Debug/TestLexer.dll
mono packages/NUnit.ConsoleRunner.3.10.0/tools/nunit3-console.exe --labels=All TestSimpleLexer/bin/Debug/TestSimpleLexer.dll
mono packages/NUnit.ConsoleRunner.3.10.0/tools/nunit3-console.exe --labels=All TestGeneratedLexer/bin/Debug/TestGeneratedLexer.dll
mono packages/NUnit.ConsoleRunner.3.10.0/tools/nunit3-console.exe --labels=All TestDescentParser/bin/Debug/TestDescentParser.dll
mono packages/NUnit.ConsoleRunner.3.10.0/tools/nunit3-console.exe --labels=All TestGeneratedParser/bin/Debug/TestGeneratedParser.dll
mono packages/NUnit.ConsoleRunner.3.10.0/tools/nunit3-console.exe --labels=All TestASTParser/bin/Debug/TestASTParser.dll
mono packages/NUnit.ConsoleRunner.3.10.0/tools/nunit3-console.exe --labels=All TestVisitors/bin/Debug/TestVisitors.dll
mono packages/NUnit.ConsoleRunner.3.10.0/tools/nunit3-console.exe --labels=All TestCodeGenerator/bin/Debug/TestCodeGenerator.dll
