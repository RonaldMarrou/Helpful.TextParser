.\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -target:.\packages\NUnit.ConsoleRunner.3.6.1\tools\nunit3-console.exe -targetargs:".\Helpful.TextParser.Test\bin\Debug\Helpful.TextParser.Test.dll" -filter:"+[Helpful.*]* -[*.Tests]*" -register:user

.\packages\coveralls.io.1.3.4\tools\coveralls.net.exe --opencover .\results.xml