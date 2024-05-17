using Demo.Infrastructure;
using System;
using System.Data;

namespace Demo.Data.NHibernateRepository
{
    public class UnitOfWork : IUnitOfWork<IDbContext>
    {
        private IDbTransaction trans = null;


        public IDbTransaction Transaction => trans;

        public IDbContext Context { get; private set; }

        public UnitOfWork(IDbContext context)
        {
            Context = context;
        }

        public void BeginTrans()
        {
            if (trans == null)
            {
                var conn = Context.CreateConnection();

                if (conn.State != ConnectionState.Open)
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
                //Context.Dispose();
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
            //  return Context.Connection.ExecuteScalar(sql, null, transaction: trans);
            return null;
        }

        public void ExecuteNoQueryRawSql(string sql, params object[] parameters)
        {
            //RepoDb: IDataReader ExecuteNonQuery(this IDbConnection connection
            //Context.Connection.ExecuteNonQuery(sql, null, transaction: trans);
        }

        public DataTable ExecuteRawSql(string sql, params object[] parameter)
        {
            //RepoDb: IDataReader ExecuteReader(this IDbConnection connection
            //using (var dataReader = Context.Connection.ExecuteReader(sql, null))
            //{
            //    if (dataReader.Read())
            //    {
            //        var dataTable = new DataTable();
            //        dataTable.Load(dataReader);
            //        return dataTable;
            //    }

            //    return null;
            //}
            return null;
        }
    }

}

