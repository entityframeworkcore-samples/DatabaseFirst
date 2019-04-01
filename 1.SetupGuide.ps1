dotnet new sln -n EFDatabaseFirst.JecaestevezApp
dotnet new console -n ConsoleApp.Jecaestevez -o ConsoleApp
dotnet new classlib -n DAL.JecaestevezApp -o DAL
dotnet sln EFDatabaseFirst.JecaestevezApp.sln add ConsoleApp/ConsoleApp.Jecaestevez.csproj
dotnet sln EFDatabaseFirst.JecaestevezApp.sln add DAL/DAL.JecaestevezApp.csproj
dotnet add ConsoleApp/ConsoleApp.Jecaestevez.csproj reference DAL/DAL.JecaestevezApp.csproj
dotnet build