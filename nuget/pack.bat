del *.nupkg
copy ..\CC.Common.Parser\bin\Release\CC.Common.Parser.dll lib\net40
REM nuget pack CC.Common.Parser.csproj -Prop Configuration=Release
nuget pack CC.Common.Parser.nuspec
nuget push *.nupkg