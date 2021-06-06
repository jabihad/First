using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FileUploadApi.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);
        #region LINQ ASYNC
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> DeleteListAsync(ICollection<TEntity> items);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetById(int id);

        Task<IList<TResult>> GetAllIncludeAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                            Expression<Func<TEntity, bool>> predicate = null,
                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                            bool disableTracking = true);


        Task<TResult> GetFirstOrDefaultIncludeAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                            Expression<Func<TEntity, bool>> predicate = null,
                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                            bool disableTracking = true);

        Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> SaveChangesAsync();

        #endregion
    }
}
