using Dapper;
using Demo.Infrastructure;
using Dommel;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Demo.DapperConsole
{
    public class GenericRepository<TEntity> : IRepository<TEntity, int> where TEntity : class, new()
    {
        private IUnitOfWork<IDbContext> unitOfWork;

        private PartsQryGenerator partsQryGenerator;
        public IDbContext Context { get { return unitOfWork.Context; } }

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            if (!(unitOfWork is IUnitOfWork<IDbContext>))
            {
                throw new ArgumentException("Expected IUnitOfWork<Microsoft.EntityFrameworkCore.DbContext>");
            }

            this.unitOfWork = unitOfWork as IUnitOfWork<IDbContext>;

            partsQryGenerator = new PartsQryGenerator();
        }

        #region IRepository<TEntity>
        public IUnitOfWork UnitOfWork
        {
            get { return unitOfWork; }
        }


        public bool Any(Expression<Func<TEntity, bool>> predicateExpr)
        {
            return GetQuery().Any(predicateExpr);
        }

        public int Count(Expression<Func<TEntity, bool>> predicateExpr)
        {
            return GetQuery().Count(predicateExpr);

        }
        public IEnumerable<TEntity> Get()
        {
            return GetQuery().ToList();
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicateExpr)
        {
            return GetQuery().Where(predicateExpr);
        }

        public TEntity GetByKey(int Id)
        {
            return Context.Connection.Get<TEntity>(Id);
        }


        public TEntity Get(Expression<Func<TEntity, bool>> predicateExpr)
        {
            return GetQuery().FirstOrDefault(predicateExpr);
        }


        private IQueryable<TEntity> GetQuery()
        {

            return Context.Connection.GetAll<TEntity>().AsQueryable();
        }

        public virtual TEntity Create(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var id = Context.Connection.Insert(entity);
            var INTID=Convert.ToInt32(id);
            return GetByKey(INTID);
        }

  
        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Context.Connection.Delete(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> criteria)
        {
            var records = GetList(criteria);

            foreach (var r in records)
            {
                Context.Connection.Delete(r);
            }
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            Context.Connection.Update(entity);
        }

        #endregion

        public TEntity GetSingle(string sql, CommandType commandType = CommandType.Text, object whereConditions = null)
        {
            return Context.Connection.QueryFirst<TEntity>(sql, commandType: commandType);

        }

        public IEnumerable<TEntity> GetList(string sql, CommandType commandType = CommandType.Text, object parameters = null)
        {
            return Context.Connection.Query<TEntity>(sql, commandType: commandType);
        }




        #region IRepositoryWithParameters<TEntity>
        public IEnumerable<TEntity> GetByParameters(object filter)
        {
            ParameterValidator.ValidateObject(filter, nameof(filter));

            var selectQry = partsQryGenerator.GenerateSelect(filter);

            var result = Context.Connection.Query<TEntity>(selectQry, filter);

            return result;
        }
       
        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (unitOfWork != null)
                    {
                        unitOfWork.Dispose();
                        unitOfWork = null;
                    }
                }

                disposed = true;
            }
        }



        #endregion

        public class PartsQryGenerator
        {
            private PropertyInfo[] properties;
            private string[] propertiesNames;
            private string typeName;

            private string characterParameter;

            public PartsQryGenerator(char characterParameter = '@')
            {
                var type = typeof(TEntity);

                this.characterParameter = characterParameter.ToString();

                properties = type.GetProperties();
                propertiesNames = properties.Where(a => !IsComplexType(a)).Select(a => a.Name).ToArray();
                typeName = type.Name;
            }
            public string GenerateSelect()
            {
                var result = string.Empty;

                var sb = new StringBuilder("SELECT ");

                string separator = $",{Environment.NewLine}";

                string selectPart = string.Join(separator, propertiesNames);

                sb.AppendLine(selectPart);

                string fromPart = $"FROM {typeName}";

                sb.Append(fromPart);

                result = sb.ToString();

                return result;
            }

            public string GenerateSelect(object fieldsFilter)
            {
                //   ParameterValidator.ValidateObject(fieldsFilter, nameof(fieldsFilter));

                var initialSelect = GenerateSelect();
                if (fieldsFilter == null)
                {
                    return initialSelect;
                }

                var where = GenerateWhere(fieldsFilter);

                var result = $" {initialSelect} {where}";

                return result;
            }

            private string GenerateWhere(object filtersPKs)
            {
                ParameterValidator.ValidateObject(filtersPKs, nameof(filtersPKs));

                var filtersPksFields = filtersPKs.GetType().GetProperties().Select(a => a.Name).ToArray();

                if (!filtersPksFields?.Any() ?? true) throw new ArgumentException($"El parameter filtersPks isn't valid. This parameter must be a class type", nameof(filtersPKs));

                var propertiesWhere = filtersPksFields.Select(a => $"{a} = {characterParameter}{a}").ToArray();

                var strWhere = string.Join(" AND ", propertiesWhere);

                var result = $" WHERE {strWhere} ";

                return result;
            }

            private bool IsComplexType(PropertyInfo propertyInfo)
            {
                bool result;

                result = (propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType.Name != "String") || propertyInfo.PropertyType.IsInterface;

                return result;
            }
        }

        public static class ParameterValidator
        {
            public static void ValidateObject(object obj, string nameParameter, string customMessage = null)
            {
                if (obj == null)
                    throw new ArgumentNullException(nameParameter, customMessage ?? $"The parameter {nameParameter} it is not null");
            }

            public static void ValidateString(string str, string nameParameter, string customMessage = null)
            {
                if (string.IsNullOrWhiteSpace(str))
                    throw new ArgumentNullException(nameParameter, customMessage ?? $"The parameter {nameParameter} it is not null/empty/white");
            }
        }
    }
}
