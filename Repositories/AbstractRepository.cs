using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace aspdotnet_managesys.Repositories
{
    public abstract class AbstractRepository : IdentityDbContext
    {
        public AbstractRepository(DbContextOptions options) : base(options) {
        }

        public DbSet<TEntity> EntitySet<TEntity>() where TEntity : class
        {
            return this.Set<TEntity>();
        }

        public TEntity FindOne<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return EntitySet<TEntity>().FirstOrDefault(predicate);
        }

        public List<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return EntitySet<TEntity>().Where(predicate).ToList();
        }

        public PagedList<TEntity> Find<TEntity>(IQueryable<TEntity> query, int page, int pageSize) where TEntity : class
        {
            var result = new PagedList<TEntity>();
            result.Page = page;
            result.Size = pageSize;
            result.NumberOfElements = pageSize;

            //IQueryable<TEntity> query = EntitySet<TEntity>().Where(predicate);
            result.TotalElements = query.Count();

            var pageCount = (double)result.TotalElements / pageSize;
            result.TotalPages = (int)Math.Ceiling(pageCount);

            var skip = page * pageSize;     
            result.Content = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }

        public List<TEntity> FindAll<TEntity>() where TEntity : class
        {
            return EntitySet<TEntity>().ToList();
        }

        public TEntity Save<TEntity>(TEntity entity) where TEntity : class
        {
            EntitySet<TEntity>().Add(entity);
            SaveChanges();
            return entity;
        }

        public TEntity Change<TEntity>(TEntity entity) where TEntity : class
        {
            EntitySet<TEntity>().Update(entity);
            SaveChanges();
            return entity;
        }

        public TEntity Delete<TEntity>(TEntity entity) where TEntity : class
        {
            EntitySet<TEntity>().Remove(entity);
            SaveChanges();
            return entity;
        }

        public TResult Transaction<TResult>(Func<TResult> func)
        {
            using (var transaction = this.Database.BeginTransaction())
            {
                try
                {
                    var ret = func();
                    SaveChanges();
                    transaction.Commit();
                    return ret;
                }
                catch
                {
                    transaction.Rollback();
                    return default(TResult);
                }
            }
        }

        public void Transaction(Action func)
        {
            Transaction(() => { func(); return true;});
        }
    }
}