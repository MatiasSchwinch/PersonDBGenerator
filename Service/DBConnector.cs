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
            Database.EnsureDeleted();
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Persona
            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("PersonalBasicData");

                entity.HasKey(k => k.PersonID);

                entity.Property(p => p.Title).HasColumnType("nvarchar(20)");

                entity.Property(p => p.FirstName).HasColumnType("nvarchar(30)").IsRequired();

                entity.Property(p => p.LastName).HasColumnType("nvarchar(30)").IsRequired();

                entity.Property(p => p.Gender).HasColumnType("tinyint");

                entity.Property(p => p.Date).HasColumnType("date").IsRequired();

                entity.Property(p => p.Age);

                entity.Property(p => p.Email).HasColumnType("nvarchar(320)").IsRequired();

                entity.Property(p => p.Phone).HasColumnType("nvarchar(30)");

                entity.Property(p => p.Cell).HasColumnType("nvarchar(30)");

                entity.Property(p => p.Nationality).HasColumnType("nvarchar(4)");

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
                entity.ToTable("PersonalLocation");

                entity.HasKey(k => k.LocationID);

                entity.Property(p => p.StreetNumber).HasColumnType("int");

                entity.Property(p => p.StreetName).HasColumnType("nvarchar(90)");

                entity.Property(p => p.City).HasColumnType("nvarchar(60)");

                entity.Property(p =>p.State).HasColumnType("nvarchar(60)");

                entity.Property(p => p.Country).HasColumnType("nvarchar(60)");

                entity.Property(p => p.Postcode).HasColumnType("nvarchar(30)");

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
                entity.ToTable("PersonalCoordinates");

                entity.HasKey(k => k.CoordinatesID);

                entity.Property(p => p.Latitude);

                entity.Property(p => p.Longitude);
            });

            // Locación - Zona horaria
            modelBuilder.Entity<Timezone>(entity =>
            {
                entity.ToTable("PersonalTimezone");

                entity.HasKey(k => k.TimezoneID);

                entity.Property(p => p.Offset).HasColumnType("nvarchar(6)");

                entity.Property(p => p.Description).HasColumnType("nvarchar(120)");
            });
            #endregion

            modelBuilder.Entity<Login>(entity =>
            {
                entity.ToTable("PersonalLogin");

                entity.HasKey(k => k.LoginID);

                entity.Property(p => p.Uuid).HasColumnType("nvarchar(36)");
                
                entity.Property(p => p.Username).HasColumnType("nvarchar(25)");

                entity.Property(p => p.Password).HasColumnType("nvarchar(25)");

                entity.Property(p => p.Salt).HasColumnType("nvarchar(10)");

                entity.Property(p => p.Md5).HasColumnType("nvarchar(32)");

                entity.Property(p => p.Sha1).HasColumnType("nvarchar(40)");

                entity.Property(p => p.Sha256).HasColumnType("nvarchar(64)");
            });

            modelBuilder.Entity<Registered>(entity =>
            {
                entity.ToTable("PersonalRegistered");

                entity.HasKey(k => k.RegisteredID);

                entity.Property(p => p.Date).HasColumnType("date").IsRequired();

                entity.Property(p => p.Age);
            });

            modelBuilder.Entity<Picture>(entity =>
            {
                entity.ToTable("PersonalPicture");

                entity.HasKey(k => k.PictureID);

                entity.Property(p => p.Large).HasColumnType("nvarchar(60)");

                entity.Property(p => p.Medium).HasColumnType("nvarchar(60)");

                entity.Property(p => p.Thumbnail).HasColumnType("nvarchar(60)");
            });
        }

        public enum DatabaseEngine
        {
            SQLServer = 1,
            SQLite = 2
        }
    }
}
