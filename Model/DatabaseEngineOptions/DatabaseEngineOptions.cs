using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PersonDBGenerator.Model.DatabaseEngineOptions
{
    public abstract class DatabaseEngineOptions
    {
        private protected readonly IConfiguration _config = new ConfigurationBuilder().AddJsonFile("config.json").Build();
        public abstract string ConnectionStringConfigFileKey { get; set; }
        public abstract DbContextOptionsBuilder SetDatabaseEngine(DbContextOptionsBuilder optionsBuilder);
    }
}
