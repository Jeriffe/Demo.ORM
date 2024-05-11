using Demo.Infrastructure;
using RepoDb;
using System;
using System.Data;

namespace Demo.RepoDBConsole.Repositories
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
                trans = Context.CreateConnection().EnsureOpen().BeginTransaction();
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
            catch (Exception ex) { throw ex; }
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

        public object ExecuteScalar(string sql, object parameters)
        {
            //RepoDb: IDataReader ExecuteScalar(this IDbConnection connection
            return Context.Connection.ExecuteScalar(sql, parameters, transaction: trans);
        }

        public void ExecuteNoQueryRawSql(string sql, object parameters = null)
        {
            //RepoDb: IDataReader ExecuteNonQuery(this IDbConnection connection
            Context.Connection.ExecuteNonQuery(sql, parameters, transaction: trans);
        }

        DataTable IUnitOfWork.ExecuteRawSql(string sql, object parameter)
        {
            //RepoDb: IDataReader ExecuteReader(this IDbConnection connection
            using (var dataReader = Context.Connection.ExecuteReader(sql, parameter))
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

