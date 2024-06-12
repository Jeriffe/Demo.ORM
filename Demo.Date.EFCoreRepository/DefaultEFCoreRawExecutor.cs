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
        /// <summary>
        /// T must be registered in DBContext as DbSet<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteRawSql<T>(DbConnection conn, DbTransaction trans, string sql, CommandType commandType = CommandType.Text, params RawParameter[] parameters) where T : class, new()
        {
            if (!HasParameters(parameters))
            {
                var objParas = BuilderFormatParameterValues(ref sql, parameters);

                //DbSet.FromSqlRaw Parameterized Queries ---Format string
                return dbContext.Set<T>().FromSqlRaw(sql, objParas).ToList();
            }

            var result = dbContext.Set<T>().FromSqlRaw(sql).ToList();

            return result;

            /*https://www.learnentityframeworkcore.com/raw-sql
             
             * FromSql	     This method returns a DbSet of the specified type T, where T is the model representing the data returned from the query
             * SqlQuery	     This method returns an IEnumerable of the specified type T, where T is the model representing the data returned from the query.
             * ExecuteSql	 This method is used to execute a raw SQL command that does not return any data. It returns the number of rows affected by the command.
             
             *Limitations -- When returning entity types from SQL queries, you must be aware of all the limitations:
                1.Your SQL query must return all the properties of the entity type.
                2.The column names in the result set must match the column names that properties are mapped to.
                3.The SQL query can't contain related data. However, in many cases, you can compose on top of the query using the Include operator to return related data.

             * DbSet.FromSql
                 sql = $"SELECT * FROM Books WHERE Title = 'Hamlet'";
                 var book = context.Books.FromSql(sql).FirstOrDefault();

             * DbSet.FromSql Parameterized Queries 
                 var title = "Hamlet";
                 var book = context.Books.FromSql($"SELECT * FROM Books WHERE Title = {title}").FirstOrDefault();
            
             * DbSet.FromSql Stored Procedures 
                 var author = "Shakespeare";
                 var books = context.Books.FromSql($"EXECUTE dbo.GetMostPopularBooks {author}").ToList();

             * DbSet.FromSql Stored Procedures WITH DbParameter to set the precise database type of the parameter:
                 var author = new SqlParameter("author", "Shakespeare");
                 var books = context.Books.FromSql($"EXECUTE dbo.GetMostPopularBooks {author}").ToList();


             * DbSet.FromSqlRaw 
                DbSet.FromSqlRaw method enables you to pass in a SQL command to be executed against the database to return instances 
                of the type represented by the DbSet:

                The DbSet must be included in the model (i.e. it can not be configured as Ignored). 
                All columns in the target table that map to properties on the entity must be included in the SQL statement. 
                The column names must match those that the properties are mapped to. Property names are not taken into account 
                when the results are hydrated into instances of the entity.
                If any columns are missing, or are returned with names not mapped to properties, an InvalidOperationException will be raised with the message:
                    'The required column '[name of first missing column]' was not present in the results of a 'FromSqlRaw' operation.'

             * DbSet.FromSqlRaw
                var books = context.Books.FromSqlRaw("SELECT BookId, Title, AuthorId FROM Books").ToList();

             * DbSet.FromSqlRaw Parameterized Queries 
                 //Format string
                 var author = db.Authors.FromSqlRaw("SELECT * From Authors Where AuthorId = {0}", id).FirstOrDefault();
                 // String interpolation
                 var author = db.Authors.FromSqlInterpolated($"SELECT * From Authors Where AuthorId = {id}").FirstOrDefault();

             * DbSet.FromSqlRaw Stored Procedures 
                 var authorId = new SqlParameter("@AuthorId", 1);
                 var books = context.Books.FromSqlRaw("EXEC GetBooksByAuthor @AuthorId" , authorId).ToList();
             */
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