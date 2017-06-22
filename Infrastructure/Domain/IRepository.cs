using Infrastructure.DataObjects;
using Infrastructure.Dependency;
using Infrastructure.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public interface IRepository : ITransientDependency
    {

    }

    public interface IRepository<TEntity> : IRepository<TEntity,int> where TEntity:class,IEntity<int>
    { }
    public interface IRepository<TEntity,TPrimaryKey> : IRepository where TEntity:class,IEntity<TPrimaryKey>
    {
        #region 对原生sql的支持
        
        IEnumerable<TEntity> ExecuteQuery(string sqlQuery, params object[] parameters);
        int ExecuteCommand(string sqlCommand, params object[] parameters);
        IEnumerable<IDataParameter> ExecStoredProc(string storeProcName, params IDataParameter[] parameters);
        #endregion



        TEntity Get(TPrimaryKey id, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        Task<TEntity> GetAsync(TPrimaryKey id);
        IQueryable<TEntity> GetAll();
        List<TEntity> GetAllList(OrderAction<TEntity> orderAction = null,params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        Task<List<TEntity>> GetAllListAsync();
        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate, OrderAction<TEntity> orderAction = null,params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);
        
        T Query<T>(Func<IQueryable<TEntity>, T> queryMethod);
        TEntity Single(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault(TPrimaryKey id, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity Load(TPrimaryKey id, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        TEntity Insert(TEntity entity);
        int Insert(List<TEntity> entities);
        Task<TEntity> InsertAsync(TEntity entity);
        TPrimaryKey InsertAndGetId(TEntity entity);
        Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);
        TEntity InsertOrUpdate(TEntity entity);
        Task<TEntity> InsertOrUpdateAsync(TEntity entity);
        TPrimaryKey InsertOrUpdateAndGetId(TEntity entity);
        Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity);
        TEntity Update(TEntity entity);
        int Update(List<TEntity> entities);
        Task<TEntity> UpdateAsync(TEntity entity);
        TEntity Update(TPrimaryKey id, Action<TEntity> updateAction);
        Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction);
        void Delete(TEntity entity);
        int Delete(List<TEntity> entities);
        Task DeleteAsync(TEntity entity);
        void Delete(TPrimaryKey id);
        Task DeleteAsync(TPrimaryKey id);
        void Delete(Expression<Func<TEntity, bool>> predicate);
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        PagedResult<TEntity> GetPaged(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> predicate, OrderAction<TEntity> orderAction, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);
        PagedResult<TEntity> GetPaged(int pageNumber, int pageSize, OrderAction<TEntity> orderAction, params Expression<Func<TEntity, dynamic>>[] eagerLoadingProperties);

        int Count();
        Task<int> CountAsync();
        int Count(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        long LongCount();
        Task<long> LongCountAsync();
        long LongCount(Expression<Func<TEntity, bool>> predicate);
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

        DataSet ExecuteCommand(string sqlCommand);


    }
}
