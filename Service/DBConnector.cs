using GeneradorBaseDatos.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GeneradorBaseDatos.Service
{
    public class DBConnector : DbContext
    {
        public DbSet<Person> Person {get; set;}
        
        private const string DBFileName = "Person.db";

        public DBConnector()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString: $"Filename={DBFileName}", sqliteOptionsAction: options =>
            {
                _ = options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            
            base.OnConfiguring(optionsBuilder);
        }
    }
}
