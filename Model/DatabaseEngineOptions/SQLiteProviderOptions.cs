using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PersonDBGenerator.Model.DatabaseEngineOptions
{
    public class SQLiteProviderOptions : DatabaseEngineOptions
    {
        public override string ConnectionStringConfigFileKey { get; set; } = "SqliteConnection";

        public override DbContextOptionsBuilder SetDatabaseEngine(DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.UseSqlite(connectionString: _config.GetConnectionString(ConnectionStringConfigFileKey));
        }
    }
}
