using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PersonDBGenerator.Model.DatabaseEngineOptions
{
    public class SqlServerProviderOptions : DatabaseEngineOptions
    {
        public override string ConnectionStringConfigFileKey { get; set; } = "SqlServerConnection";

        public override DbContextOptionsBuilder SetDatabaseEngine(DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.UseSqlServer(connectionString: _config.GetConnectionString(ConnectionStringConfigFileKey));
        }
    }
}
