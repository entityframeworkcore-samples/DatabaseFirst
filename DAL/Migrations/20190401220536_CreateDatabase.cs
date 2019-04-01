using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.JecaestevezApp.Migrations
{
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
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Items')
                BEGIN
                    DROP TABLE [dbo].[Items]
                END
                ");
        }
    }
}
