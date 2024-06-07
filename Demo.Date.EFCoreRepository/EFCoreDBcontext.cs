using Demo.Data.Models;
using Demo.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;


namespace Demo.Date.EFCoreRepository
{
    public class EFCoreDBcontext : DbContext, IDbContext
    {
        static bool IsDataBaseTypeSettinged = false;

        public DbSet<TPatient> Patients { get; set; }

        public DataProviderType ProviderName { get; set; }

        public EFCoreDBcontext(string connectionString, DataProviderType providerName = DataProviderType.Sqlite)
        {
            this.ConnectionString = connectionString;
            this.ProviderName = providerName;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            SetDataBaseType(optionsBuilder);

        }

        private void SetDataBaseType(DbContextOptionsBuilder optionsBuilder)
        {
            if (IsDataBaseTypeSettinged)
            {
                return;
            }

            switch (ProviderName)
            {
                case DataProviderType.Sqlite:
                    //Install-Package Microsoft.EntityFrameworkCore.Sqlite
                    optionsBuilder.UseSqlite(ConnectionString);

                    break;
                case DataProviderType.PostgreSQL:
                    //NuGet\Install - Package Npgsql.EntityFrameworkCore.PostgreSQL - Version 8.0.4
                    optionsBuilder.UseNpgsql();
                    break;
                case DataProviderType.MySQL:
                    //NuGet\Install-Package Pomelo.EntityFrameworkCore.MySql -Version 8.0.2
                    optionsBuilder.UseMySql(ServerVersion.AutoDetect(ConnectionString));
                    break;
                case DataProviderType.SQLServer:
                default:
                    //NuGet\Install-Package Microsoft.EntityFrameworkCore.SqlServer - Version 8.0.6
                    optionsBuilder.UseSqlServer(ConnectionString);

                    break;
            }

            IsDataBaseTypeSettinged = true;

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }

        public string ConnectionString { get; private set; }

        public DbConnection Connection => Database.GetDbConnection();

        public void CloseConnection()
        {
            this.Database.GetDbConnection().Close();
        }

        public DbConnection CreateConnection()
        {
            return this.Database.GetDbConnection();
        }

        public void Dispose()
        {

        }
    }
}
