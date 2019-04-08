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

Add a reference from ConsoleApp to DAL.JecaestevezApp
  >dotnet add ConsoleApp/ConsoleApp.Jecaestevez.csproj reference DAL/DAL.JecaestevezApp.csproj

Build the solution
 > dotnet build

 It's possible to execute the previous command executing in powershell the script "1.SetupGuide.ps1"

## 2 Adding Entity Framework Core packages

You can also add manual the package opening  terminal and navigate to DatabaseFirst\DAL and  add to "DAL.JecaestevezApp.csproj"  EntityFrameworkCore.SqlServer , EntityFrameworkCore.Tools and Microsoft.EntityFrameworkCore.Design  using the CLI 

> dotnet add .\DAL\DAL.JecaestevezApp.csproj package Microsoft.EntityFrameworkCore.SqlServer

> dotnet add .\DAL\DAL.JecaestevezApp.csproj package Microsoft.EntityFrameworkCore.Tools 

> dotnet add .\DAL\DAL.JecaestevezApp.csproj package Microsoft.EntityFrameworkCore.Design 


## 3 Add a simple class to be used in a new  DBContext
Add DBContext
```
    public class EfDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO Extract connection string to a secret
            optionsBuilder.UseSqlServer(@"Server=.\;Database=EFDatabaseFirstDB;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
        public DbSet<Item> Items { get; set; }
    }
```

## 4 Create the first migration
Add a new migration "CreateDatabase"
> dotnet ef  migrations add CreateDatabase --startup-project ..\ConsoleApp

It's possible do the same step using the Package Manager Console in Visual Studio, selecting the DAL.JecaestevezApp.csproj and execute 
> PM > add-migration CreateDatabase

It will be create a folder "Migrations" and the following files:
* CreateDatabase.cs
* CreateDatabase.Designer.cs
* EfDbContextModelSnapshot.cs

## 5 Modify migration to add sql script create "items" table
Modify the file "CreateDatabase.cs" to execute a sql and create the new table "Items" :
```
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Items')
                BEGIN
                    DROP TABLE [dbo].[Items]
                END
                CREATE TABLE [dbo].[Items](
	                [id] [int] IDENTITY(1,1) NOT NULL,
	                [Name] [nvarchar](max) NULL,
	                [Description] [nvarchar](max) NULL,
	                [Expiration] [datetime2](7) NOT NULL,
                 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
                (
	                [id] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TABLE [dbo].[Items]");
        }
    }
```
## 6 Update Database
Using dotnet EF CLI open powershell console , navigate to "\DAL" and execute the update database
> dotnet ef database update --startup-project ..\ConsoleApp

Using Package Manager Console select the DAL.JecaestevezApp.csproj and execute 
> PM> update-database –verbose


## 7 Add new migration to modified "Items" table

It's possible do the same step using the Package Manager Console in Visual Studio, selecting the DAL.JecaestevezApp.csproj and execute 
> PM > add-migration AlterItemsTable_AddColumn

Modify new migration to add a sql code to alter the existent Items table to add a new column "IsEnable" as we can see here bellow 
```
    public partial class AlterItemsTable_AddColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE [dbo].[Items]  ADD IsEnable bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE [dbo].[Items]  DROP IsEnable");
        }
    }
```

## 8 Update Database
Using Package Manager Console select the DAL.JecaestevezApp.csproj and execute 
> PM> update-database 


## 9 Adding Nuget packages for Configuration

You can also add manual the package opening  terminal and navigate to DatabaseFirst\DAL and  add to "DAL.JecaestevezApp.csproj"  Microsoft.Extensions.Configuration.Json  using the CLI 

> dotnet add .\DAL\DAL.JecaestevezApp.csproj package Microsoft.Extensions.Configuration.Json

> dotnet add .\DAL\DAL.JecaestevezApp.csproj package MicrosoftMicrosoft.Extensions.Configuration

## 10 Adding settings file  AppSettings 
Add a new settings file "appsettins.json" to DAL.JecaestevezApp.csproj
```
{
  "ConnectionStrings": {
    "Database": "Server=.\\;Database=EFDatabaseFirstDB;Trusted_Connection=True;MultipleActiveResultSets=true;"
  }
}
```
## 11 Read connection string from appsettings.json 

Use Configuration Builder to create a helper " IConfigurationRoot _configuration" to read the the settings file 
```
public class EfDbContext : DbContext
{
    private static IConfigurationRoot _configuration;

    public EfDbContext()
    {
        var dbo = new DbContextOptionsBuilder<EfDbContext>();
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true);

        _configuration = builder.Build();
    }
}
```
Modify "OnConfiguring" to read the connection string from the appsetting.json using the _configuration helper like a dictionary "_configuration["ConnectionStrings:Database"]"
```
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optBuilder.UseSqlServer(_configuration["ConnectionStrings:Database"]);
    }
```
## 12 Simple refactoring
```
public class EfDbContext : DbContext
{
    private static IConfigurationRoot _configuration;

    public EfDbContext()
    {
        var dbo = new DbContextOptionsBuilder<EfDbContext>();
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true);

        _configuration = builder.Build();

        UseSqlServer(dbo);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        UseSqlServer(optionsBuilder);
    }

    void UseSqlServer(DbContextOptionsBuilder optBuilder)
    {
        if (!optBuilder.IsConfigured)
        {
            var dbOptionsBuilder = optBuilder.UseSqlServer(_configuration["ConnectionStrings:Database"]);
        }
    }
}
```