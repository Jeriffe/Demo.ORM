using System;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace Demo.Infrastructure
{
    public static class ExtensionMethods
    {
        public static void ProcessByTrans(this IUnitOfWork conn, Action action)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    action();

                    transaction.Complete();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static TEntity ProcessByTrans<TEntity>(this IUnitOfWork conn, Func<TEntity> action) where TEntity : class
        {
            using (var trans = new TransactionScope())
            {
                try
                {
                    var result = action();

                    trans.Complete();

                    return result;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static void ProcessWithTrans(this IUnitOfWork conn, Action action)
        {
            if (conn.GetType().FullName == "Demo.Data.NHibernateRepository.UnitOfWork")
            {
                conn.ProcessByTrans(action);

                return;
            }

            using (var transaction = new TransactionScope())
            {
                try
                {
                    conn.BeginTrans();

                    action();

                    conn.Commit();

                    transaction.Complete();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public static TEntity ProcessWithTrans<TEntity>(this IUnitOfWork conn, Func<TEntity> action) where TEntity : class
        {
            if (conn.GetType().FullName == "Demo.Data.NHibernateRepository.UnitOfWork")
            {
                return conn.ProcessByTrans(action);
            }

            using (var trans = new TransactionScope())
            {
                try
                {
                    conn.BeginTrans();
                    var result = action();
                    conn.Commit();

                    trans.Complete();

                    return result;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private static void Transby<TEntity>(Func<TEntity> action, TransactionScope trans) where TEntity : class
        {
            var result = action();

            trans.Complete();
        }

        public static void EnsureOpenConn(this DbConnection conn)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
        }

    }
}
