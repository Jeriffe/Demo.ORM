using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

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
