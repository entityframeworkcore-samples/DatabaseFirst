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

## 9 Add new "Item" model and DbSet
Add the new Item class model 
```
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Expiration { get; set; }
        public bool IsEnable { get; set; }
    }
```
Update DB context to use DbSet Item
```
    public class EfDbContext : DbContext
    {
        DbSet<Item> Items { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO Extract connection string to a secret
            optionsBuilder.UseSqlServer(@"Server=.\;Database=EFDatabaseFirstDB;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
```
## 10 Use the data base context to add new "item"

```
            using (var context = new EfDbContext())
            {
                var Item = new Item()
                {
                    Name = "Ron Palido",
                    Description = "Drink",
                    Expiration = DateTime.Now.AddYears(1)

                };
                Console.WriteLine($"Item NOT saved -> Id {Item.Id} {Item.Name} {Item.Expiration}");

                context.Add(Item);
                context.SaveChanges();

                Console.WriteLine($"Item saved -> Id {Item.Id} {Item.Name} {Item.Expiration}");
                Console.ReadKey();
            }
```
## 11 Create new Testing project and add NuGet package InMemory
Create an empty mstest project with the name "IntegrationTest.JecaestevezApp"
 > dotnet new mstest -n IntegrationTest.JecaestevezApp -o IntegrationTest

 Add the created mstest project to the solution
  > dotnet sln EFDatabaseFirst.JecaestevezApp.sln add IntegrationTest/IntegrationTest.JecaestevezApp.csproj  

 Add a reference from IntegrationTest.JecaestevezApp to DAL.JecaestevezApp
  >dotnet add IntegrationTest/IntegrationTest.JecaestevezApp.csproj reference DAL/DAL.JecaestevezApp.csproj

Open Package Manager Console in Visual Studio, selecting the IntegrationTest.JecaestevezApp..csproj and execute 
> Install-Package Microsoft.EntityFrameworkCore.InMemory

You can also add manual the package opening  terminal and navigate to DatabaseFirst\IntegrationTest.JecaestevezApp.
Add to "IntegrationTest.JecaestevezApp.csproj"  "Microsoft.EntityFrameworkCore.InMemory" using the CLI 

> dotnet add .\IntegrationTest\IntegrationTest.JecaestevezApp.csproj package Microsoft.EntityFrameworkCore.InMemory

## 12 Update Database Context 
Add this two new constructor to the db context "EfDbContext" to enable testing against a different database is to modify your context to expose a constructor that accepts a DbContextOptions<TContext>
```
        public EfDbContext()
        {

        }
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
        }
```

Add a modification to check if option builder has been configured before configuring database context 

```
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //TODO Extract connection string to a secret
                optionsBuilder.UseSqlServer(@"Server=.\;Database=EFDatabaseFirstDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
```
## 13 Add integration test
To configure in the test the Database context options to use in memory:
```
            var options = new DbContextOptionsBuilder<EfDbContext>()
             .UseInMemoryDatabase(databaseName: "InMemory_EFDatabaseFirstDB")
             .Options;
```

The test will looks like this:
```
        public void Given_NoItems_Them_AddNewItem()
        {
             var options = new DbContextOptionsBuilder<EfDbContext>()
             .UseInMemoryDatabase(databaseName: "InMemory_EFDatabaseFirstDB")
             .Options;

            var itemSaved = new Item();

            //Arrange
            var expirationDay = DateTime.Now.AddYears(1);

            //Act
            using (var context = new EfDbContext(options))
            {
                var newItem = new Item()
                {
                    Name = "Ron Palido",
                    Description = "Drink",
                    Expiration = expirationDay

                };

                context.Add(newItem);
                context.SaveChanges();

                itemSaved = context.Items.Find(1);
            }
            
            //Assert            
            Assert.IsNotNull(itemSaved, "Failed -Item not saved");
            Assert.AreEqual(itemSaved.Name, "Ron Palido", "Failed - Errons in Field Name");
            Assert.AreEqual(itemSaved.Description, "Drink", "Failed - Errons in Field Description");
            Assert.AreEqual(itemSaved.Expiration, expirationDay, "Failed - Errons in Field expiration");

        }
```
