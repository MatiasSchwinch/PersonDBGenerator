using GeneradorBaseDatos.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GeneradorBaseDatos.Service
{
    public class DBConnector : DbContext
    {
        public DbSet<Person> Persons {get; set;}

        private readonly DatabaseEngine _dbEngine;
        private readonly IConfiguration _config = new ConfigurationBuilder().AddJsonFile("config.json").Build();

        public DBConnector(DatabaseEngine dbEngine)
        {
            _dbEngine = dbEngine;
            
            // Este enfoque fue el seleccionado ya que al ser una Db enfocada íntegramente para realizar pruebas, no veo que valga la pena implementar un soporte para migraciones.
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (_dbEngine)
            {
                case DatabaseEngine.SQLServer:
                    optionsBuilder.UseSqlServer(connectionString: _config.GetConnectionString("SqlServerConnection"));
                    break;

                case DatabaseEngine.SQLite:
                    optionsBuilder.UseSqlite(connectionString: _config.GetConnectionString("SqliteConnection"));
                    break;
            }
        }

        public enum DatabaseEngine
        {
            SQLServer = 1,
            SQLite = 2
        }
    }
}
