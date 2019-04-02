using DAL.JecaestevezApp.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DAL.JecaestevezApp
{
    public class EfDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        public EfDbContext()
        {

        }
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //TODO Extract connection string to a secret
                optionsBuilder.UseSqlServer(@"Server=.\;Database=EFDatabaseFirstDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }
    }
}
