using GeneradorBaseDatos.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeneradorBaseDatos.Service
{
    public class DBConnector : DbContext
    {
        public DbSet<Person> Person {get; set;}
        private const string _dbTableName = "PersonDBGenerator";

        private readonly DatabaseEngine _dbEngine;
        private readonly string _dbSQLConnectionString;

        public DBConnector(DatabaseEngine dbEngine)
        {
            _dbEngine = dbEngine;

            if (dbEngine is DatabaseEngine.SQLServer)
            {
                string obtainConnectionString = JsonSerializer.Deserialize<ConnectionString>(File.Exists("config.json")
                    ? File.ReadAllText("config.json")
                    : throw new Exception("El archivo \"config.json\" no se encuentra en el directorio.")).DefaultConnection;

                _dbSQLConnectionString = string.IsNullOrWhiteSpace(obtainConnectionString)
                    ? throw new Exception("Debe ingresar la cadena de conexión de SQL Server en el archivo \"config.json\" dentro del apartado: DefaultConnection.")
                    : obtainConnectionString;
            }

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (_dbEngine)
            {
                case DatabaseEngine.SQLServer:
                    optionsBuilder.UseSqlServer(connectionString: _dbSQLConnectionString, sqlServerOptionsAction: options =>
                    {
                        _ = options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                    });
                    break;

                case DatabaseEngine.SQLite:
                    optionsBuilder.UseSqlite(connectionString: $"Filename={_dbTableName}.db", sqliteOptionsAction: options =>
                    {
                        _ = options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                    });
                    break;
            }
            
            base.OnConfiguring(optionsBuilder);
        }

        internal class ConnectionString
        {
            [JsonPropertyName("DefaultConnection")]
            public string DefaultConnection { get; set; }
        }

        public enum DatabaseEngine
        {
            SQLServer = 1,
            SQLite = 2
        }
    }
}
