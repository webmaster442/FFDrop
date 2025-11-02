dotnet restore
dotnet build --configuration Release --no-restore
dotnet publish -c Release -r win-x64 --self-contained true -o .\publish