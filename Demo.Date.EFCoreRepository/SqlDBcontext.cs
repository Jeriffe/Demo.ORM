using Demo.Data.Models;
using Demo.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;

namespace Demo.Date.EFCoreRepository
{
    public class SqlDBcontext : DbContext, IDbContext
    {
        public DbSet<TPatient> Patients { get; set; }
        public SqlDBcontext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(ConnectionString);
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
