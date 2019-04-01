# Database First Entity Framework Dotnet Core 2x simple guide

## 1. Creating the solution projects for this guide
Open a new Terminal window and then type the bellow commands:

Create new empty solution "EFDatabaseFirst.JecaestevezApp"
 > dotnet new sln -n EFDatabaseFirst.JecaestevezApp

Create empty console application "ConsoleApp.Jecaestevez"
 > dotnet new console -n ConsoleApp.Jecaestevez -o ConsoleApp

Create empty library application "DAL.JecaestevezApp"
 > dotnet new classlib -n DAL.JecaestevezApp -o DAL

 Add the created console application to the solution
  > dotnet sln EFDatabaseFirst.JecaestevezApp.sln add ConsoleApp/ConsoleApp.Jecaestevez.csproj  

Add the console application to the solution
  > dotnet sln EFDatabaseFirst.JecaestevezApp.sln add DAL/DAL.JecaestevezApp.csproj  

Add a refrence from ConsoleApp to DAL.JecaestevezApp
  >dotnet add ConsoleApp/ConsoleApp.Jecaestevez.csproj reference DAL/DAL.JecaestevezApp.csproj

Build the solution
 > dotnet build
