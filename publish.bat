dotnet pack
dotnet nuget push bin\Debug\iobloc.3.0.0.nupkg -k nuget_key_from_email -s https://api.nuget.org/v3/index.json
rem remember to also change version in csproj