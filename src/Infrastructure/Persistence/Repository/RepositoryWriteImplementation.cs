namespace Aviant.DDD.Infrastructure.Persistence.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Contexts;
    using Domain.Entities;
    using Domain.Persistence;
    using Microsoft.EntityFrameworkCore;

    public abstract class RepositoryWriteImplementation<TDbContext, TEntity, TPrimaryKey>
        : IRepositoryWrite<TEntity, TPrimaryKey>
        where TDbContext : DbContext
        where TEntity : Entity<TPrimaryKey>
    {
        private TDbContext DbContext { get; }
        
        private DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        protected RepositoryWriteImplementation(TDbContext dbContext) => DbContext = dbContext;

        #region IRepositoryWrite<TEntity,TPrimaryKey> Members
    
        public virtual async Task Add(TEntity entity)
        {
            // First validate entity's rules
            await entity.Validate();
    
            await DbSet.AddAsync(entity);
        }
    
        public virtual Task Update(TEntity entity)
        {
            // First validate entity's rules
            entity.Validate();

            if (DbContext.GetType() == typeof(DbContextWrite<>))
            
                DbContext.Entry(entity).State = EntityState.Modified;
    
            return Task.CompletedTask;
        }
    
        public virtual Task Delete(TEntity entity)
        {
            // First validate entity's rules
            entity.Validate();
    
            DbContext.Entry(entity).State = EntityState.Deleted;
    
            return Task.CompletedTask;
        }
    
        public virtual Task DeleteWhere(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> entities = DbSet.Where(predicate);
    
            foreach (var entity in entities) DbContext.Entry(entity).State = EntityState.Deleted;
    
            return Task.CompletedTask;
        }
    
        public void Dispose()
        {
            DbContext.Dispose();
        }
    
        #endregion
    }
}