using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.JecaestevezApp.Migrations
{
    public partial class AlterItemsTable_AddColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Items')
                BEGIN
                    ALTER TABLE [dbo].[Items]  ADD IsEnable bit
                END
            ");                
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Items')
                BEGIN
                    ALTER TABLE [dbo].[Items]  DROP IsEnable
                END
            ");
        }
    }
}
