using Demo.Data.Models;
using Demo.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Data.Common;

namespace Demo.Date.EFCoreRepository
{
    public class SqlDBcontext : DbContext, IDbContext
    {
        static bool IsDataBaseTypeSettinged = false;

        public DbSet<TPatient> Patients { get; set; }

        public DataProviderType ProviderName { get; set; }

        public SqlDBcontext(string connectionString, DataProviderType providerName = DataProviderType.Sqlite)
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
                    optionsBuilder.UseSqlite(ConnectionString);

                    break;
                case DataProviderType.PostgreSQL:
                case DataProviderType.MySQL:

                    throw new NotImplementedException();

                case DataProviderType.SQLServer:

                default:

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


        public string ConnectionString { get; set; }

        public DbConnection Connection => throw new NotImplementedException();

        public void CloseConnection()
        {
            throw new NotImplementedException();
        }

        public DbConnection CreateConnection()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }
    }
}
