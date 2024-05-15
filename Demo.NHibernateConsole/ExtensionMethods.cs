using Demo.Infrastructure;
using System;
using System.Transactions;

namespace Demo.Data.NHibernateRepository
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



        
    }

}
