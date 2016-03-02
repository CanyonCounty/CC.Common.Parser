del *.nupkg
nuget pack CC.Common.Parser.csproj -Prop Configuration=Release
nuget push *.nupkg