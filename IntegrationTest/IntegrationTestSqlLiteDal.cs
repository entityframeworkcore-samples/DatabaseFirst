using DAL.JecaestevezApp;
using DAL.JecaestevezApp.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IntegrationTest.JecaestevezApp
{
    [TestClass]
    public class IntegrationTestSqlLiteDal
    {
        [TestMethod]
        public void Given_NoItems_Them_AddNewItem()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<EfDbContext>()
              .UseSqlite(connection)
              .Options;

            var itemSaved = new Item();

            //Arrange
            var expirationDay = DateTime.Now.AddYears(1);
            // Create the schema in the database
            using (var context = new EfDbContext(options))
            {
                context.Database.EnsureCreated();
            }
            //Act
            using (var context = new EfDbContext(options))
            {
                context.Database.EnsureCreated();
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
    }
}
