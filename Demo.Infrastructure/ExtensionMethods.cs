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

        //public static void ProcessByTrans(this IUnitOfWork conn, Action action)
        //{
        //    using (var transaction = new TransactionScope())
        //    {
        //        try
        //        {
        //            conn.BeginTrans();

        //            action();

        //            conn.Commit();

        //            transaction.Complete();

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}

        public static void EnsureOpenConn(this DbConnection conn)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
        }

    }
}
