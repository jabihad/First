using Entities;
using FileUploadApi.Services.AppUser.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FileUploadApi.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly RepositoryContext _context;
        private readonly IAppUserService _appUserService;


        public Repository(RepositoryContext context, IAppUserService appUserService)
        {
            _context = context;
            _appUserService = appUserService;

        }
        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().AsNoTracking().FirstOrDefault(predicate);
        }
        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).AsNoTracking().AsQueryable();
        }


        #region LINQ ASYNC
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync<TEntity>();
        }
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await SaveChangesAsync();
            return entity;
        }
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Set<TEntity>().Attach(entity);
            var entry = _context.Entry(entity);
            entry.State = EntityState.Modified;
            await SaveChangesAsync();
            return entity;
        }
        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var records = await _context.Set<TEntity>().Where(predicate).ToListAsync();
            if (!records.Any())
            {
                throw new Exception(".NET ObjectNotFoundException"); //new ObjectNotFoundException();
            }
            foreach (var record in records)
            {
                _context.Set<TEntity>().Remove(record);
            }
            return await SaveChangesAsync();
        }
        public async Task<int> DeleteListAsync(ICollection<TEntity> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            try
            {
                foreach (var record in items)
                {
                    _context.Set<TEntity>().Attach(record);
                }
                _context.Set<TEntity>().RemoveRange(items);
                var result = await SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "");
                throw;
            }
        }
        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).AsNoTracking().ToListAsync();
        }
        public async Task<TEntity> GetById(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<IList<TResult>> GetAllIncludeAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                            Expression<Func<TEntity, bool>> predicate = null,
                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                            bool disableTracking = true)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();

            if (include != null)
                query = include(query);

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            if (disableTracking)
                query = query.AsNoTracking();

            var result = await query.Select(selector).ToListAsync();

            return result;
        }


        public virtual async Task<TResult> GetFirstOrDefaultIncludeAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                            Expression<Func<TEntity, bool>> predicate = null,
                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                            bool disableTracking = true)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();

            if (include != null)
                query = include(query);

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            if (disableTracking)
                query = query.AsNoTracking();

            var result = await query.Select(selector).FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var count = await _context.Set<TEntity>().CountAsync(predicate);
            return count > 0;
        }



        #endregion
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                 //Log.Information("saving data");
                //CreateLog();
                var result = await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception e)
            {

                //Log.Error(e, "failed to save");
                throw;
            }

        }

        //private void CreateLog()
        //{
        //    var userId = _appUserService.GetuserId();
        //    _context.ChangeTracker.DetectChanges();
        //    var auditEntries = new List<AuditEntry>();
        //    foreach (var entry in _context.ChangeTracker.Entries())
        //    {
        //        if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged) continue;

        //        if (entry.State == EntityState.Added)
        //        {
        //            if (entry.Entity is IAuditableEntity addAudit)
        //            {
        //                addAudit.CreatedBy = userId;
        //                addAudit.CreatedTime = DateTime.Now;
        //            }


        //        }
        //        if (entry.State == EntityState.Modified)
        //        {
        //            if (entry.Entity is IAuditableEntity addAudit)
        //            {
        //                addAudit.ModifiedBy = userId;
        //                addAudit.ModifiedTime = DateTime.Now;
        //            }

        //        }


        //        AuditEntry auditEntry = new(entry);
        //        auditEntry.TableName = entry.Entity.GetType().Name;
        //        auditEntry.UserId = userId;
        //        auditEntries.Add(auditEntry);
        //        foreach (var property in entry.Properties)
        //        {
        //            string propertyName = property.Metadata.Name;
        //            if (property.Metadata.IsPrimaryKey())
        //            {
        //                auditEntry.KeyValues[propertyName] = property.CurrentValue;
        //                continue;
        //            }
        //            switch (entry.State)
        //            {
        //                case EntityState.Added:
        //                    auditEntry.AuditType = AuditType.Create;
        //                    auditEntry.NewValues[propertyName] = property.CurrentValue;
        //                    break;
        //                case EntityState.Deleted:
        //                    auditEntry.AuditType = AuditType.Delete;
        //                    auditEntry.OldValues[propertyName] = property.OriginalValue;
        //                    break;
        //                case EntityState.Modified:
        //                    if (property.IsModified)
        //                    {
        //                        auditEntry.ChangedColumns.Add(propertyName);
        //                        auditEntry.AuditType = AuditType.Update;
        //                        auditEntry.OldValues[propertyName] = property.OriginalValue;
        //                        auditEntry.NewValues[propertyName] = property.CurrentValue;

        //                    }
        //                    break;
        //            }
        //        }
        //    }
        //    foreach (var auditEntry in auditEntries)
        //    {
        //        _context.AuditLogs.Add(auditEntry.ToAudit());
        //    }
        //}


    }
}
