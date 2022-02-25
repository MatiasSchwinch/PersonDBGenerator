using GeneradorBaseDatos.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace GeneradorBaseDatos.Service
{
    public class DBConnector : DbContext
    {
        public DbSet<Person> Persons {get; set;}

        private readonly DatabaseEngine _dbEngine;
        private readonly Dictionary<int, DatabaseEngineOptions> _dbEngineOptions = new()
        {
            { 1, new SqlServerProviderOptions() },
            { 2, new PostgreSQLProviderOptions() },
            { 3, new SQLiteProviderOptions() }
        };

        public DBConnector(DatabaseEngine dbEngine)
        {
            _dbEngine = dbEngine;

            // Este enfoque fue el seleccionado ya que al ser una Db enfocada íntegramente para realizar pruebas, no veo que valga la pena implementar un soporte para migraciones.
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _dbEngineOptions[(int)_dbEngine].SetDatabaseEngine(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Persona
            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("BasicData");

                entity.HasKey(k => k.PersonID);

                entity.Property(p => p.Title).HasMaxLength(20);

                entity.Property(p => p.FirstName).HasMaxLength(30).IsRequired();

                entity.Property(p => p.LastName).HasMaxLength(30).IsRequired();

                entity.Property(p => p.Gender).HasColumnType("smallint");

                entity.Property(p => p.Date).HasColumnType("date").IsRequired();

                entity.Property(p => p.Age);

                entity.Property(p => p.Email).HasMaxLength(320).IsRequired();

                entity.Property(p => p.Phone).HasMaxLength(30);

                entity.Property(p => p.Cell).HasMaxLength(30);

                entity.Property(p => p.Nationality).HasMaxLength(4);

                // 1:1 Locación.
                entity.HasOne(r => r.Location)
                      .WithOne(r => r.Person)
                      .HasForeignKey<Person>(fk => fk.LocationID)
                      .OnDelete(DeleteBehavior.SetNull);

                // 1:1 Datos de Login.
                entity.HasOne(r => r.Login)
                      .WithOne(r => r.Person)
                      .HasForeignKey<Person>(fk => fk.LoginID)
                      .OnDelete(DeleteBehavior.SetNull);

                // 1:1 Data de Registro.
                entity.HasOne(r => r.Registered)
                      .WithOne(r => r.Person)
                      .HasForeignKey<Person>(fk => fk.RegisteredID)
                      .OnDelete(DeleteBehavior.SetNull);

                // 1:1 Picture
                entity.HasOne(r => r.Picture)
                      .WithOne(r => r.Person)
                      .HasForeignKey<Person>(fk => fk.PictureID)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            #region Locación
            // Locación
            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.HasKey(k => k.LocationID);

                entity.Property(p => p.StreetNumber);

                entity.Property(p => p.StreetName).HasMaxLength(90);

                entity.Property(p => p.City).HasMaxLength(60);

                entity.Property(p =>p.State).HasMaxLength(60);

                entity.Property(p => p.Country).HasMaxLength(60);

                entity.Property(p => p.Postcode).HasMaxLength(30);

                // 1:1 Datos de coordenadas.
                entity.HasOne(r => r.Coordinates)
                      .WithOne(r => r.Location)
                      .HasForeignKey<Location>(fk => fk.CoordinatesID)
                      .OnDelete(DeleteBehavior.SetNull);

                // 1:1 Zona horaria.
                entity.HasOne(r => r.Timezone)
                      .WithOne(r => r.Location)
                      .HasForeignKey<Location>(fk => fk.TimezoneID)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Locación - Coordenadas
            modelBuilder.Entity<Coordinates>(entity =>
            {
                entity.ToTable("Coordinates");

                entity.HasKey(k => k.CoordinatesID);

                entity.Property(p => p.Latitude);

                entity.Property(p => p.Longitude);
            });

            // Locación - Zona horaria
            modelBuilder.Entity<Timezone>(entity =>
            {
                entity.ToTable("Timezone");

                entity.HasKey(k => k.TimezoneID);

                entity.Property(p => p.Offset).HasMaxLength(6);

                entity.Property(p => p.Description).HasMaxLength(120);
            });
            #endregion

            modelBuilder.Entity<Login>(entity =>
            {
                entity.ToTable("Login");

                entity.HasKey(k => k.LoginID);

                entity.Property(p => p.Uuid).HasMaxLength(36);

                entity.Property(p => p.Username).HasMaxLength(25);

                entity.Property(p => p.Password).HasMaxLength(25);

                entity.Property(p => p.Salt).HasMaxLength(10);

                entity.Property(p => p.Md5).HasMaxLength(32);

                entity.Property(p => p.Sha1).HasMaxLength(40);

                entity.Property(p => p.Sha256).HasMaxLength(64);
            });

            modelBuilder.Entity<Registered>(entity =>
            {
                entity.ToTable("Registered");

                entity.HasKey(k => k.RegisteredID);

                entity.Property(p => p.Date).HasColumnType("date").IsRequired();

                entity.Property(p => p.Age);
            });

            modelBuilder.Entity<Picture>(entity =>
            {
                entity.ToTable("Picture");

                entity.HasKey(k => k.PictureID);

                entity.Property(p => p.Large).HasMaxLength(60);

                entity.Property(p => p.Medium).HasMaxLength(60);

                entity.Property(p => p.Thumbnail).HasMaxLength(60);
            });
        }
    }

    public abstract class DatabaseEngineOptions
    {
        private protected readonly IConfiguration _config = new ConfigurationBuilder().AddJsonFile("config.json").Build();
        public abstract string ConnectionStringConfigFileKey { get; set; }
        public abstract DbContextOptionsBuilder SetDatabaseEngine(DbContextOptionsBuilder optionsBuilder);
    }

    public class SqlServerProviderOptions : DatabaseEngineOptions
    {
        public override string ConnectionStringConfigFileKey { get; set; } = "SqlServerConnection";

        public override DbContextOptionsBuilder SetDatabaseEngine(DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.UseSqlServer(connectionString: _config.GetConnectionString(ConnectionStringConfigFileKey));
        }
    }

    public class SQLiteProviderOptions : DatabaseEngineOptions
    {
        public override string ConnectionStringConfigFileKey { get; set; } = "SqliteConnection";

        public override DbContextOptionsBuilder SetDatabaseEngine(DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.UseSqlite(connectionString: _config.GetConnectionString(ConnectionStringConfigFileKey));
        }
    }

    public class PostgreSQLProviderOptions : DatabaseEngineOptions
    {
        public override string ConnectionStringConfigFileKey { get; set; } = "PostgreSqlConnection";

        public override DbContextOptionsBuilder SetDatabaseEngine(DbContextOptionsBuilder optionsBuilder)
        {
            return optionsBuilder.UseNpgsql(connectionString: _config.GetConnectionString(ConnectionStringConfigFileKey));
        }
    }

    public enum DatabaseEngine
    {
        SQLServer = 1,
        PostgreSQL = 2,
        SQLite = 3
    }
}
