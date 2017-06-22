using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Infrastructure.DataObjects;
using Infrastructure.UnitOfWork;

namespace Data.Seedwork.EF.Repositories
{
    public class EfRepositoryBase<TDbContext, TEntity> : EfRepositoryBase<TDbContext, TEntity, int>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
        where TDbContext : DbContext
    {
        public EfRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
    public class EfRepositoryBase<TDbContext, TEntity, TPrimaryKey> : RepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Gets EF DbContext object.
        /// </summary>
        public virtual TDbContext Context { get { return _dbContextProvider.GetDbContext(); } }

        /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>
        public virtual DbSet<TEntity> Table { get { return Context.Set<TEntity>(); } }

        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContextProvider"></param>
        public EfRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public override IQueryable<TEntity> GetAll()
        {
            return Table;
        }

        public override async Task<List<TEntity>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        public override async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        public override async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().SingleAsync(predicate);
        }

        public override async Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return await GetAll().FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        public override async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate);
        }

        public override TEntity Insert(TEntity entity)
        {
            return Table.Add(entity);
        }
        public override int Insert(List<TEntity> entities)
        {
            return Table.AddRange(entities).Count();
        }

        public override Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Table.Add(entity));
        }

        public override TPrimaryKey InsertAndGetId(TEntity entity)
        {
            entity = Insert(entity);

            if (entity.IsTransient())
            {
                Context.SaveChanges();
            }

            return entity.Id;
        }

        public override async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            entity = await InsertAsync(entity);

            if (entity.IsTransient())
            {
                await Context.SaveChangesAsync();
            }

            return entity.Id;
        }

        public override TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
        {
            entity = InsertOrUpdate(entity);

            if (entity.IsTransient())
            {
                Context.SaveChanges();
            }

            return entity.Id;
        }

        public override async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            entity = await InsertOrUpdateAsync(entity);

            if (entity.IsTransient())
            {
                await Context.SaveChangesAsync();
            }

            return entity.Id;
        }

        public override TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public override Task<TEntity> UpdateAsync(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return Task.FromResult(entity);
        }

        public override void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        public override void Delete(TPrimaryKey id)
        {
            var entity = Table.Local.FirstOrDefault(ent => EqualityComparer<TPrimaryKey>.Default.Equals(ent.Id, id));
            if (entity == null)
            {
                entity = FirstOrDefault(id);
                if (entity == null)
                {
                    return;
                }
            }

            Delete(entity);
        }

        public override async Task<int> CountAsync()
        {
            return await GetAll().CountAsync();
        }

        public override async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).CountAsync();
        }

        public override async Task<long> LongCountAsync()
        {
            return await GetAll().LongCountAsync();
        }

        public override async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).LongCountAsync();
        }

        protected virtual TEntity AttachIfNot(TEntity entity)
        {
            if (!Table.Local.Contains(entity))
            {
                entity=Table.Attach(entity);
                
            }
            return entity;


        }

        

        public override IEnumerable<TEntity> ExecuteQuery(string sqlQuery, params object[] parameters)
        {
            return Context.Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }

        public override int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return Context.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        public override IEnumerable<IDataParameter> ExecStoredProc(string storeProcName, params IDataParameter[] parameters)
        {
            var cmd = Context.Database.Connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = storeProcName;
            cmd.Parameters.AddRange(parameters);
            try
            {
                Context.Database.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                Context.Database.Connection.Close();
            }
            return cmd.Parameters.Cast<IDataParameter>();
        }

        public override DataSet ExecuteCommand(string sqlCommand)
        {
            DataSet retValue = new DataSet();
            DataTable table = new DataTable("Table1");
            var cmd = Context.Database.Connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = sqlCommand;
            try
            {
                Context.Database.Connection.Open();
                var reader=cmd.ExecuteReader(CommandBehavior.CloseConnection);
                var schemaTable = reader.GetSchemaTable();
                foreach(DataRow st in schemaTable.Rows)
                {
                    DataColumn column = new DataColumn(st["ColumnName"].ToString(), (Type)st["DataType"]);
                    table.Columns.Add(column);
                }
                table.Load(reader);
                reader.Close();

                retValue.Tables.Add(table);
                return retValue;

            }
            finally
            {
                if(Context.Database.Connection.State==ConnectionState.Open)
                    Context.Database.Connection.Close();
            }
        }
        
        public override List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate, OrderAction<TEntity> orderAction = null, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            Context.Configuration.AutoDetectChangesEnabled = false;
            var query = this.Table as IQueryable<TEntity>;
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                query = query.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    query = query.Include(eagerLoadingPath);
                }
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderAction != null)
            {
                OrderCriteria<TEntity> ocs = new OrderCriteria<TEntity>();
                orderAction(ocs);
                var parameter = Expression.Parameter(typeof(TEntity), "o");

                for (int i = 0; i < ocs.OrderByFields.Count; i++)
                {
                    var oc = ocs.OrderByFields[i];
                    var property = typeof(TEntity).GetProperty(GetEagerLoadingPath(oc.Field));
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);
                    string OrderName = "";
                    if (i > 0)
                    {
                        OrderName = oc.OrderBy == OrderBy.Desc ? "ThenByDescending" : "ThenBy";
                    }
                    else
                        OrderName = oc.OrderBy == OrderBy.Desc ? "OrderByDescending" : "OrderBy";
                    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), OrderName, new Type[] { typeof(TEntity), property.PropertyType }, query.Expression, Expression.Quote(orderByExp));

                    query = query.Provider.CreateQuery<TEntity>(resultExp);
                }
                    
                var result= query.ToList();
                for(int i=0;i<result.Count;i++)
                {
                    result[i] = Table.Attach(result[i]);
                }
                Context.Configuration.AutoDetectChangesEnabled = true;
                return result;
            }
            var retValue = query.ToList();
            for(int i=0;i<retValue.Count;i++)
            {
                retValue[i] = AttachIfNot(retValue[i]);
            }
            Context.Configuration.AutoDetectChangesEnabled = true;
            return retValue;
        }

        public override TEntity Single(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            Context.Configuration.AutoDetectChangesEnabled = false;
            var query = this.Table as IQueryable<TEntity>;
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                query = query.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    query = query.Include(eagerLoadingPath);
                }
            }
            var retValue= query.Single(predicate);
            retValue = AttachIfNot(retValue);
            Context.Configuration.AutoDetectChangesEnabled = true;
            return retValue;

        }

        public override TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            Context.Configuration.AutoDetectChangesEnabled = false;
            var query = this.Table as IQueryable<TEntity>;
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                query = query.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    query = query.Include(eagerLoadingPath);
                }
            }
            
            var retValue = query.FirstOrDefault(predicate);
            if(retValue!=null)
            {
                retValue = AttachIfNot(retValue);
            }
            
            
            Context.Configuration.AutoDetectChangesEnabled = true;
            return retValue;
        }

        public override PagedResult<TEntity> GetPaged(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> predicate, OrderAction<TEntity> orderAction, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            Context.Configuration.AutoDetectChangesEnabled = false;
            var query = this.Table as IQueryable<TEntity>;
            if (eagerLoadingProperties != null &&
                eagerLoadingProperties.Length > 0)
            {
                var eagerLoadingProperty = eagerLoadingProperties[0];
                var eagerLoadingPath = GetEagerLoadingPath(eagerLoadingProperty);
                query = query.Include(eagerLoadingPath);
                for (int i = 1; i < eagerLoadingProperties.Length; i++)
                {
                    eagerLoadingProperty = eagerLoadingProperties[i];
                    eagerLoadingPath = this.GetEagerLoadingPath(eagerLoadingProperty);
                    query = query.Include(eagerLoadingPath);
                }
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            OrderCriteria<TEntity> ocs = new OrderCriteria<TEntity>();
            orderAction(ocs);
            var parameter = Expression.Parameter(typeof(TEntity), "o");
            
            for(int i=0;i<ocs.OrderByFields.Count;i++)
            {
                var oc = ocs.OrderByFields[i];
                var property = typeof(TEntity).GetProperty(GetEagerLoadingPath(oc.Field));
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                string OrderName = "";
                if (i > 0)
                {
                    OrderName = oc.OrderBy==OrderBy.Desc ? "ThenByDescending" : "ThenBy";
                }
                else
                    OrderName = oc.OrderBy == OrderBy.Desc ? "OrderByDescending" : "OrderBy";
                MethodCallExpression resultExp = Expression.Call(typeof(Queryable), OrderName, new Type[] { typeof(TEntity), property.PropertyType }, query.Expression, Expression.Quote(orderByExp));

                query = query.Provider.CreateQuery<TEntity>(resultExp);
                //if (oc.OrderBy == OrderBy.Asc)
                //{
                //    if (ordered != null)
                //    {
                //        ordered = ordered.OrderBy(oc.Field);
                //    }
                //    else
                //    {
                //        IOrderedQueryable<TEntity> or = query.OrderBy(oc.Field);
                //    }
                //}
                //else
                //{
                //    if (ordered != null)
                //    {
                //        ordered = ordered.OrderByDescending(oc.Field);
                //    }
                //    else
                //    {
                //        ordered = query.OrderByDescending(oc.Field);
                //    }
                //}
            }
            var total = query.Count();
            var grouped = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            if (grouped != null && grouped.Count>0)
            {
                var retValue =new PagedResult<TEntity>(total, (total + pageSize - 1) / pageSize, pageSize, pageNumber, grouped);
                for(int i=0;i<retValue.Data.Count;i++)
                {
                    retValue.Data[i] = this.AttachIfNot(retValue.Data[i]);
                }
                Context.Configuration.AutoDetectChangesEnabled = true;
                return retValue;
            }
            Context.Configuration.AutoDetectChangesEnabled = true;
            return PagedResult<TEntity>.Empty;
        }
    }
}
