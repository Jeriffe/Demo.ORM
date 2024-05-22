using Demo.Infrastructure;
using RepoDb;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Demo.RawSql
{

    public class DefaultRepoDbRawExecutor : IRawSqlExecutor
    {
        public void ExecuteNoQueryRawSql(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            conn.ExecuteNonQuery(sql, BuilderDynamicParameters(parameters), transaction: trans, commandType: commandType);
        }
        public object ExecuteRawScalar(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            return conn.ExecuteScalar(sql, BuilderDynamicParameters(parameters), commandType: commandType, transaction: trans);
        }

        public DataTable ExecuteRawSql(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            using (var dataReader = conn.ExecuteReader(sql, BuilderDynamicParameters(parameters), transaction: trans,commandType:commandType))
            {
                var dataTable = new DataTable();
                dataTable.Load(dataReader);

                return dataTable;
            }
        }

        public IEnumerable<T> ExecuteRawSql<T>(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters) where T : class, new()
        {
            return conn.ExecuteQuery<T>(sql, BuilderDynamicParameters(parameters), transaction: trans, commandType: commandType);
        }

        private static Dictionary<string, object> BuilderDynamicParameters(RawParameter[] parameters)
        {
            if (!HasParameters(parameters))
            {
                return null;
            }

            var paramDic = new Dictionary<string, object>()
            {
            };
            foreach (var p in parameters)
            {
                if (p.Name.StartsWith("@"))
                {
                    p.Name = p.Name.Substring(1);
                }

                paramDic[p.Name] = p.Value;
            }

            return paramDic;
        }

        private static bool HasParameters(RawParameter[] parameters)
        {
            return parameters != null && parameters.Length > 0;
        }

       
    }
}
