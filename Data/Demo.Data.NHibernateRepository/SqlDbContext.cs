using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.Configuration;
using System.Data;
using System.Data.Common;
using FluentNHibernate.Cfg;

namespace Demo.Data.NHibernateRepository
{
    public class SqlDbContext : Infrastructure.IDbContext
    {
        public string ConnectionString { get; set; }
        public NHibernate.ISession Session { get; private set; }
        public DbConnection Connection { get { return CreateConnection(); } set { conn = value; } }

        static SqlDbContext()
        {
            sessionFactory = CreateSessionFactory();
        }

        public SqlDbContext(string connectionString)
        {
            ConnectionString = connectionString;

            Session = sessionFactory.OpenSession();
        }

        private DbConnection conn;
        public DbConnection CreateConnection()
        {
            conn = Session.Connection;

            return conn;
        }

        public void CloseConnection()
        {
            if (conn != null)
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }

        public void Dispose()
        {
            CloseConnection();

            Connection = null;
        }
        static ISessionFactory sessionFactory;
        static ISessionFactory CreateSessionFactory()
        {
            var connStr = ConfigurationManager.ConnectionStrings["DB"].ConnectionString; ;
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(connStr))
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<TPatientMap>())
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        static void BuildSchema(NHibernate.Cfg.Configuration config)
        {
            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config).Create(false, false);
        }
    }
}
