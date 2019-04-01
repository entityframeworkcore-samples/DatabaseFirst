using DAL.JecaestevezApp.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DAL.JecaestevezApp
{
    public class EfDbContext : DbContext
    {
        DbSet<Item> Items { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO Extract connection string to a secret
            optionsBuilder.UseSqlServer(@"Server=.\;Database=EFDatabaseFirstDB;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
