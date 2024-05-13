using System;
using System.Data.Common;

namespace Demo.Infrastructure
{
    public interface IDbContext : IDisposable
    {
        string ConnectionString { get; set; }
        DbConnection Connection { get; }

        DbConnection CreateConnection();

        void CloseConnection();
    }
}
