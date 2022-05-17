using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PersonDBGenerator.Model.DatabaseEngineOptions
{
    public class PostgreSQLProviderOptions : DatabaseEngineOptions
    {
        public override string ConnectionStringConfigFileKey { get; set; } = "PostgreSqlConnection";

        public override DbContextOptionsBuilder SetDatabaseEngine(DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.UseNpgsql(connectionString: _config.GetConnectionString(ConnectionStringConfigFileKey));
        }
    }
}
