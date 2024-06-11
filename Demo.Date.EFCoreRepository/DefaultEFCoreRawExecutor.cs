using Demo.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Npgsql;
using System.Data;
using System.Data.Common;

namespace Demo.Date.EFCoreRepository
{
    internal class DefaultEFCoreRawExecutor : IRawSqlExecutor
    {
        private readonly EFCoreDBcontext dbContext;

        public DefaultEFCoreRawExecutor(EFCoreDBcontext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void ExecuteNoQueryRawSql(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            dbContext.Database.ExecuteSqlRaw(sql, BuilderDynamicParameters(parameters));
        }

        public object ExecuteRawScalar(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            var result = ExecuteScalar(conn, sql, commandType, BuilderDynamicParameters(parameters));

            return result;
        }

        public DataTable ExecuteRawSql(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters)
        {
            var result = ExecuteToDataTable(conn, sql, commandType, BuilderDynamicParameters(parameters));

            return result;
        }

        public IEnumerable<T> ExecuteRawSql<T>(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters) where T : class, new()
        {
            if (!HasParameters(parameters))
            {
                var objParas = BuilderFormatParameterValues(ref sql, parameters);

                return dbContext.Set<T>().FromSqlRaw(sql, objParas).ToList();
            }

            var result = dbContext.Set<T>().FromSqlRaw(sql).ToList();

            return result;
        }

        private object[] BuilderFormatParameterValues(ref string sql, RawParameter[] parameters)
        {
            var sqlParaArray = new object[parameters.Length];
            int i = 0;
            foreach (var originPara in parameters)
            {
                sql = sql.Replace(originPara.Name, string.Format("{{0}}", i));

                sqlParaArray[i] = originPara.Value;
                i++;
            }

            return sqlParaArray;
        }

        private DbParameter[] BuilderDynamicParameters(RawParameter[] parameters)
        {
            if (!HasParameters(parameters))
            {
                return null;
            }

            if (dbContext.Database.IsSqlServer())
            {
                return BuildSqlParamters(parameters);
            }
            else if (dbContext.Database.IsMySql())
            {
                return BuildMySqlParamers(parameters);
            }
            else if (dbContext.Database.IsSqlite())
            {
                return BuildSqliteParamters(parameters);
            }
            else if (dbContext.Database.IsNpgsql())
            {
                return BuildNpgsqlParatmets(parameters);
            }

            else
            {
                throw new NotImplementedException("Not found relative DataProvider!");
            }
        }

        private static DbParameter[] BuildNpgsqlParatmets(RawParameter[] parameters)
        {
            var sqlParaArray = new NpgsqlParameter[parameters.Length];
            int i = 0;
            foreach (var originPara in parameters)
            {
                var para = new NpgsqlParameter()
                {
                    ParameterName = originPara.Name,
                    Value = originPara.Value
                };
                sqlParaArray[i] = para;
                i++;
            }

            return sqlParaArray;
        }

        private static DbParameter[] BuildSqliteParamters(RawParameter[] parameters)
        {
            var sqlParaArray = new SqliteParameter[parameters.Length];
            int i = 0;
            foreach (var originPara in parameters)
            {
                var para = new SqliteParameter()
                {
                    ParameterName = originPara.Name,
                    Value = originPara.Value
                };
                sqlParaArray[i] = para;
                i++;
            }

            return sqlParaArray;
        }

        private static DbParameter[] BuildMySqlParamers(RawParameter[] parameters)
        {
            var sqlParaArray = new MySqlParameter[parameters.Length];
            int i = 0;
            foreach (var originPara in parameters)
            {
                var para = new MySqlParameter()
                {
                    ParameterName = originPara.Name,
                    Value = originPara.Value
                };
                sqlParaArray[i] = para;
                i++;
            }

            return sqlParaArray;
        }

        private static DbParameter[] BuildSqlParamters(RawParameter[] parameters)
        {
            var sqlParaArray = new SqlParameter[parameters.Length];
            int i = 0;
            foreach (var originPara in parameters)
            {
                var para = new SqlParameter()
                {
                    ParameterName = originPara.Name,
                    Value = originPara.Value
                };
                sqlParaArray[i] = para;
                i++;
            }

            return sqlParaArray;
        }

        private static bool HasParameters(RawParameter[] parameters)
        {
            return parameters != null && parameters.Length > 0;
        }

        public static object ExecuteScalar(DbConnection conn, string sql, CommandType commandType = CommandType.Text, DbParameter[] parameters = null)
        {
            object value = null;

            using (var cmd = conn.CreateCommand())
            {
                if (conn.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.CommandText = sql;
                cmd.CommandType = commandType;

                //if (commandTimeOutInSeconds != null)
                //{
                //    cmd.CommandTimeout = (int)commandTimeOutInSeconds;
                //}
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                value = cmd.ExecuteScalar();
            }

            return value;
        }

        public static DataTable ExecuteToDataTable(DbConnection conn, string sql, CommandType commandType = CommandType.Text, params DbParameter[] parameters)
        {
            var dataTable = new DataTable();

            using (var cmd = conn.CreateCommand())
            {
                if (conn.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                cmd.CommandText = sql;

                cmd.CommandType = commandType;

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dataTable.Load(reader);
                    }
                }
            }

            return dataTable;
        }
    }
}