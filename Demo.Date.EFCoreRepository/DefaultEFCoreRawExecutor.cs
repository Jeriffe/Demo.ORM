using Demo.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MySqlConnector;
using Npgsql;
using System.Collections.Generic;
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
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> ExecuteRawSql<T>(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters) where T : class, new()
        {
            return dbContext.Set<T>().FromSqlRaw(string.Format(sql, BuilderDynamicParameters(parameters)));
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
    }
}