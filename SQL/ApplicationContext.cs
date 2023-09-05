using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using PoemaBotTelegram.DataProvider;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Extensions.Configuration;

namespace PoemaBotTelegram.SQL
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Url> Urls => Set<Url>();
        public DbSet<Poema> Poemas => Set<Poema>();
        public DbSet<Category> Categorys => Set<Category>();
        public DbSet<PoemaCategory> PoemasCategorys => Set<PoemaCategory>();
        
        public ApplicationContext() => Database.EnsureCreated(); // Если база уже создана - ничего не делает. При Миграциях нужно комментить

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //получаем строку подключения из конфига
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            var connectionString = config.GetConnectionString("DefaultConnection");

            var serverVersion = new MySqlServerVersion(new Version(8, 0, 33));
            optionsBuilder.UseMySql(connectionString, serverVersion);
           
            //NOTE: как привязать к Nlog? 
            //optionsBuilder.LogTo()
        }
    }
}
