using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.JecaestevezApp
{
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
}
