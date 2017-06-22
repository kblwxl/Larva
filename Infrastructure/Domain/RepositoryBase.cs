using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DataObjects;
using Infrastructure.UnitOfWork;

namespace Infrastructure.Domain
{
    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {

        public abstract IQueryable<TEntity> GetAll();

        
        public virtual List<TEntity> GetAllList(OrderAction<TEntity> orderAction = null, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return GetAllList(null, orderAction, eagerLoadingProperties);

        }

        public virtual Task<List<TEntity>> GetAllListAsync()
        {
            return Task.FromResult(GetAllList());
        }

        
        public abstract List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate, OrderAction<TEntity> orderAction = null, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        
        public virtual Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(GetAllList(predicate));
        }

        public virtual T Query<T>(Func<IQueryable<TEntity>, T> queryMethod)
        {
            return queryMethod(GetAll());
        }

        public virtual TEntity Get(TPrimaryKey id, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            var entity = FirstOrDefault(id,eagerLoadingProperties);
            if (entity == null)
            {
                throw new Exception("根据指定的主键未找到相应的记录. 实体类型: " + typeof(TEntity).FullName + ", 主键: " + id);
            }

            return entity;
        }

        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            var entity = await FirstOrDefaultAsync(id);
            if (entity == null)
            {
                throw new Exception("根据指定的主键未找到相应的记录. 实体类型: " + typeof(TEntity).FullName + ", 主键: " + id);
            }

            return entity;
        }

        public abstract TEntity Single(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);

        public virtual Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Single(predicate));
        }

        public virtual TEntity FirstOrDefault(TPrimaryKey id, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return FirstOrDefault(CreateEqualityExpressionForId(id), eagerLoadingProperties);
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return Task.FromResult(FirstOrDefault(id));
        }

        public abstract TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(FirstOrDefault(predicate));
        }

        public virtual TEntity Load(TPrimaryKey id, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return Get(id, eagerLoadingProperties);
        }

        public abstract TEntity Insert(TEntity entity);

        public virtual Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public virtual TPrimaryKey InsertAndGetId(TEntity entity)
        {
            return Insert(entity).Id;
        }

        public virtual Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            return Task.FromResult(InsertAndGetId(entity));
        }

        public virtual TEntity InsertOrUpdate(TEntity entity)
        {
            return entity.IsTransient()
                ? Insert(entity)
                : Update(entity);
        }

        public virtual async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            return entity.IsTransient()
                ? await InsertAsync(entity)
                : await UpdateAsync(entity);
        }

        public virtual TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
        {
            return InsertOrUpdate(entity).Id;
        }

        public virtual Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            return Task.FromResult(InsertOrUpdateAndGetId(entity));
        }

        public abstract TEntity Update(TEntity entity);

        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }

        public virtual TEntity Update(TPrimaryKey id, Action<TEntity> updateAction)
        {
            var entity = Get(id);
            updateAction(entity);
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction)
        {
            var entity = await GetAsync(id);
            await updateAction(entity);
            return entity;
        }

        public abstract void Delete(TEntity entity);

        public virtual Task DeleteAsync(TEntity entity)
        {
            Delete(entity);
            return Task.FromResult(0);
        }

        public abstract void Delete(TPrimaryKey id);

        public virtual Task DeleteAsync(TPrimaryKey id)
        {
            Delete(id);
            return Task.FromResult(0);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetAll().Where(predicate).ToList())
            {
                Delete(entity);
            }
        }


        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Delete(predicate);
        }

        public virtual int Count()
        {
            return GetAll().Count();
        }

        public virtual Task<int> CountAsync()
        {
            return Task.FromResult(Count());
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).Count();
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Count(predicate));
        }

        public virtual long LongCount()
        {
            return GetAll().LongCount();
        }

        public virtual Task<long> LongCountAsync()
        {
            return Task.FromResult(LongCount());
        }

        public virtual long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).LongCount();
        }

        public virtual Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(LongCount(predicate));
        }

        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
        protected MemberExpression GetMemberInfo(LambdaExpression lambda)
        {
            if (lambda == null)
                throw new ArgumentNullException(nameof(lambda));

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException(nameof(lambda));

            return memberExpr;
        }

        protected string GetEagerLoadingPath(Expression<Func<TEntity, dynamic>> eagerLoadingProperty)
        {
            MemberExpression memberExpression = this.GetMemberInfo(eagerLoadingProperty); 
             var parameterName = eagerLoadingProperty.Parameters.First().Name;
            var memberExpressionStr = memberExpression.ToString();
            var path = memberExpressionStr.Replace(parameterName + ".", "");
            return path;
        }


        public abstract IEnumerable<TEntity> ExecuteQuery(string sqlQuery, params object[] parameters);

        public abstract int ExecuteCommand(string sqlCommand, params object[] parameters);

        public abstract PagedResult<TEntity> GetPaged(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> predicate, OrderAction<TEntity> orderAction, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);

        public virtual PagedResult<TEntity> GetPaged(int pageNumber, int pageSize, OrderAction<TEntity> orderAction, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties)
        {
            return GetPaged(pageNumber, pageSize, null, orderAction, eagerLoadingProperties);
        }

        public abstract IEnumerable<IDataParameter> ExecStoredProc(string storeProcName, params IDataParameter[] parameters);

        public virtual int Insert(List<TEntity> entities)
        {
            int index = 0;

            for (; index < entities.Count; index++)
            {
                Insert(entities[index]);
            }
            return index;


        }

        public int Update(List<TEntity> entities)
        {
            int index = 0;
            for (; index < entities.Count; index++)
            {
                Update(entities[index]);
            }
            return index;
        }

        public int Delete(List<TEntity> entities)
        {
            int index = 0;
            for (; index < entities.Count; index++)
            {
                Delete(entities[index]);
            }
            return index;
        }

        public abstract DataSet ExecuteCommand(string sqlCommand);
    }
}
