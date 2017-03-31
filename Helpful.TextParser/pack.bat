packages\NuGet.CommandLine.3.5.0\tools\nuget pack Helpful.TextParser\Helpful.TextParser.csproj -Properties "Configuration=Release;Platform=AnyCPU;OutputPath=bin\Release" -Build -IncludeReferencedProjects -OutputDirectory Helpful.TextParser.Nuget
packages\NuGet.CommandLine.3.5.0\tools\nuget pack Helpful.TextParser.Unity3.Installer\Helpful.TextParser.Unity3.Installer.csproj -Properties "Configuration=Release;Platform=AnyCPU;OutputPath=bin\Release" -Build -IncludeReferencedProjects -OutputDirectory Helpful.TextParser.Nuget

pause;