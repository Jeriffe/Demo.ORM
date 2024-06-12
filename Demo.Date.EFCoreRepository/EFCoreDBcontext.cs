using Demo.Data.Models;
using Demo.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data;
using System.Data.Common;


namespace Demo.Date.EFCoreRepository
{
    public class EFCoreDBcontext : DbContext, IDbContext
    {
        public virtual DbSet<TCustomer> TCustomer { get; set; }

        public virtual DbSet<TOrder> TOrder { get; set; }

        public virtual DbSet<TOrderItem> TOrderItem { get; set; }

        public virtual DbSet<TPatient> TPatient { get; set; }

        public virtual DbSet<TProduct> TProduct { get; set; }

        public DataProviderType ProviderType { get; set; }

        public EFCoreDBcontext(string connectionString, DataProviderType providerType = DataProviderType.SQLServer)
        {
            this.ConnectionString = connectionString;
            this.ProviderType = providerType;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            SetDataBaseType(optionsBuilder);

        }

        private void SetDataBaseType(DbContextOptionsBuilder optionBuilder)
        {
            switch (ProviderType)
            {
                case DataProviderType.Sqlite:
                    //Install-Package Microsoft.EntityFrameworkCore.Sqlite
                    optionBuilder.UseSqlite(ConnectionString);

                    //Microsoft.EntityFrameworkCore.Database.Transaction.AmbientTransactionWarning: An ambient transaction has been detected
                    //https://stackoverflow.com/questions/60080620/sqlite-in-memory-transactionscope-ambient-transaction-has-been-detected
                    optionBuilder.ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning));

                    break;
                case DataProviderType.PostgreSQL:
                    //NuGet\Install - Package Npgsql.EntityFrameworkCore.PostgreSQL - Version 8.0.4
                    optionBuilder.UseNpgsql();
                    break;
                case DataProviderType.MySQL:
                    //NuGet\Install-Package Pomelo.EntityFrameworkCore.MySql -Version 8.0.2
                    optionBuilder.UseMySql(ServerVersion.AutoDetect(ConnectionString));
                    break;
                case DataProviderType.SQLServer:
                default:
                    //NuGet\Install-Package Microsoft.EntityFrameworkCore.SqlServer - Version 8.0.6
                    optionBuilder.UseSqlServer(ConnectionString);

                    break;
            }
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
