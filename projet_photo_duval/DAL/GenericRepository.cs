using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using projet_photo_duval.Models;
using System.Linq.Expressions;

namespace projet_photo_duval.DAL
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal H18_Proj_Eq07Entities1 context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(H18_Proj_Eq07Entities1 context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void UpdateEntry(TEntity entityToEntry)
        {
            context.Entry(entityToEntry).State = EntityState.Modified;
        }

        public virtual List<TEntity> ToList()
        {
            return dbSet.ToList();
        }
    }
}