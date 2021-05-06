
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Include=\"[ReqHubDotNet]ReqHub.*\"

dotnet "%userprofile%\.nuget\packages\reportgenerator\4.0.4\tools\netcoreapp2.0\ReportGenerator.dll" "-reports:.\coverage.opencover.xml" "-targetdir:.\coverage"
