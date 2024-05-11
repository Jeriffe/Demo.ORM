using System;
using System.Transactions;

namespace Demo.Infrastructure
{
    public static class ExtensionMethods
    {
        public static void ProcessWithTrans(this IUnitOfWork conn, Action action)
        {

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
                finally
                {
                    transaction.Dispose();
                }

            }
        }

        public static void ProcessWithoutTrans(this IUnitOfWork conn, Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
