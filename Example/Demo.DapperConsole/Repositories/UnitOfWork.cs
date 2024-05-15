using Dapper;
using Demo.Infrastructure;

using System.Data;
using System.Data.Common;

namespace Demo.DapperConsole
{
    public class UnitOfWork : IUnitOfWork<IDbContext>
    {
        private IDbTransaction trans = null;

        public IDbContext Context { get; private set; }

        public IDbTransaction Transaction => trans;

        public UnitOfWork(IDbContext context)
        {
            Context = context;
        }

        public void BeginTrans()
        {
            if (trans == null)
            {
                var conn = Context.CreateConnection();
                if (conn.State!= ConnectionState.Open)
                {
                    conn.Open();
                }
                
                trans = conn.BeginTransaction();
            }
        }

        public void SaveChanges()
        {

        }

        public void Commit()
        {
            if (trans == null)
            {
                throw new Exception("CANNOT_COMMIT_TRANSACTION_WHILE_NO_TRANSACTION");
            }

            try
            {
                trans.Commit();
            }
            catch (Exception) { throw; }
            finally
            {
                DisposeTransaction();
            }
        }

        public void Rollback()
        {
            if (trans != null)
            {
                trans.Rollback();
            }
        }

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }
        }

        private void DisposeTransaction()
        {
            if (trans != null)
            {
                trans.Dispose();

                trans = null;
            }

            CloseConnection();
        }

        private void CloseConnection()
        {
            Context.CloseConnection();
        }

        public object ExecuteScalar(string sql, params object[] parameters)
        {
            //RepoDb: IDataReader ExecuteScalar(this IDbConnection connection
            var result = Context.Connection.ExecuteScalar(sql, null, transaction: trans);

            return result;
        }

        public void ExecuteNoQueryRawSql(string sql, params object[] parameters)
        {
            //RepoDb: IDataReader ExecuteNonQuery(this IDbConnection connection
            Context.Connection.Execute(sql, null, transaction: trans);
        }

        public DataTable ExecuteRawSql(string sql, params object[] parameter)
        {
            //RepoDb: IDataReader ExecuteReader(this IDbConnection connection
            using (var dataReader = Context.Connection.ExecuteReader(sql, null))
            {
                if (dataReader.Read())
                {
                    var dataTable = new DataTable();
                    dataTable.Load(dataReader);
                    return dataTable;
                }

                return null;
            }
        }
    }

}

