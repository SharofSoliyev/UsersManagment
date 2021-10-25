using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UsersManagment.Core.Entities.Base;
using UsersManagment.Infostructure.Data;

namespace UsersManagment.Infostructure.Repository
{
    public interface IRepository<T> where T : Entity
    {
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(int id, bool disableTracking = true);
        Task<T> GetByIdIncAsync(int id, string[] includeTables, bool disableTracking = true);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool disableTracking = true);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, string[] includeTables, bool disableTracking = true);
        IQueryable<T> GetAllByExpIncOrder(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string[] includeTables, bool disableTracking = true);
        IQueryable<T> GetAllByInc(string[] includeTables, bool disableTracking = true);
        IQueryable<T> GetAllByExpInc(Expression<Func<T, bool>> predicate, string[] includeTables, bool disableTracking = true);
        IQueryable<T> GetAllByExp(Expression<Func<T, bool>> predicate, bool disableTracking = true);
        IQueryable<T> GetAllByExpIncOrder(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, bool disableTracking = true);
        IQueryable<T> GetAllByPage(int page, Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string[] includeTables, bool disableTracking = true);
        IQueryable<T> GetAllByPage(int page, Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, bool disableTracking = true);
        IQueryable<T> GetAll(bool disableTracking = true);
        IQueryable<T> GetAllByPage(int page, int limit, Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, bool disableTracking = true);

    }
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly AppDataContext _dbContext;

        private const int PAGE_COUNT = 20;

        public Repository(AppDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Entry(entity).Property("CreatedAt").CurrentValue = DateTime.UtcNow;
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        //// it only updates tracked changes (which have been changed after getting data in service/controller)
        //public async Task UpdateTrackedChangesAsync(T entity)
        //{
        //    _dbContext.Update(entity);
        //    await _dbContext.SaveChangesAsync();
        //}

        // It tells EF that the entity already exists in the database, and sets the state of the entity to Unchanged.
        //public async Task UpdateByPropertiesAsync(T entity, string [] properties)
        //{
        //    _dbContext.Attach(entity);
        //    foreach(var property in properties)
        //    {
        //        _dbContext.Entry(entity).Property(property).IsModified = true;
        //    }
        //    await _dbContext.SaveChangesAsync();
        //}

        //// The TrackGraph API provides access to individual entities within an object graph,
        //// and enables you to execute customised code against each one individually.
        //// This is useful in scenarios where you are dealing with complex object graphs that consist of various related entities in differing states.
        //public async Task UpdateUsingTrackGraphAsync(T entity)
        //{
        //    _dbContext.ChangeTracker.TrackGraph(entity, e => {
        //        if ((e.Entry.Entity as T) != null)
        //        {
        //            e.Entry.State = EntityState.Unchanged;
        //        }
        //        else
        //        {
        //            e.Entry.State = EntityState.Modified;
        //        }
        //    });
        //    await _dbContext.SaveChangesAsync();
        //}

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            query = query.Where(predicate);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, string[] includeTables, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            foreach (var includeTable in includeTables)
            {
                query = query.Include(includeTable);
            }

            query = query.Where(predicate);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetByIdAsync(int id, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<T> GetByIdIncAsync(int id, string[] includeTables = null, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            foreach (var includeTable in includeTables)
            {
                query = query.Include(includeTable);
            }

            return await query.Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public IQueryable<T> GetAllByExpIncOrder(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string[] includeTables, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            foreach (var includeTable in includeTables)
            {
                query = query.Include(includeTable);
            }

            query = query.Where(predicate);
            return orderBy(query);
        }

        public IQueryable<T> GetAllByInc(string[] includeTables, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            foreach (var includeTable in includeTables)
            {
                query = query.Include(includeTable);
            }

            return query;
        }

        public IQueryable<T> GetAllByExpInc(Expression<Func<T, bool>> predicate, string[] includeTables, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            foreach (var includeTable in includeTables)
            {
                query = query.Include(includeTable);
            }

            return query.Where(predicate);
        }

        public IQueryable<T> GetAllByExp(Expression<Func<T, bool>> predicate, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            return query.Where(predicate);
        }

        public IQueryable<T> GetAllByExpIncOrder(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            query = query.Where(predicate);
            return orderBy(query);
        }

        public IQueryable<T> GetAllByPage(int page, Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string[] includeTables, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            foreach (var includeTable in includeTables)
            {
                query = query.Include(includeTable);
            }

            query = query.Where(predicate);

            if (page > 1) query = query.Skip(page * PAGE_COUNT);

            query = query.Take(PAGE_COUNT);

            return orderBy(query);
        }

        public IQueryable<T> GetAllByPage(int page, Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            query = query.Where(predicate);

            if (page > 1) query = query.Skip(page * PAGE_COUNT);

            query = query.Take(PAGE_COUNT);

            return orderBy(query);
        }

        public IQueryable<T> GetAll(bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            return query;
        }

        public IQueryable<T> GetAllByPage(int page, int limit, Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            query = query.Where(predicate);

            if (page > 1) query = query.Skip((page - 1) * limit);

            query = query.Take(limit);

            return orderBy(query);
        }


    }
}
