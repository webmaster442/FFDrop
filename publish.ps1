dotnet restore
dotnet build --configuration Release --no-restore
dotnet publish .\src\FFDrop\FFDrop.csproj  -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true -o .\publish