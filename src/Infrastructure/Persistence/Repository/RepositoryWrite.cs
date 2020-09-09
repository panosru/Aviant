namespace Aviant.DDD.Infrastructure.Persistence.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Application.Identity;
    using Contexts;
    using Domain.Entities;
    using Domain.Persistence;
    using Microsoft.EntityFrameworkCore;

    public abstract class RepositoryWrite<TDbContext, TEntity, TPrimaryKey>
        : IRepositoryWrite<TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
        where TDbContext : DbContextWrite<TDbContext>
    {
        private readonly TDbContext _dbContext;

        private readonly DbSet<TEntity> _dbSet;

        protected RepositoryWrite(TDbContext context)
        {
            _dbContext = context;
            _dbSet     = _dbContext.Set<TEntity>();
        }

        #region IRepositoryWrite<TEntity,TPrimaryKey> Members

        public async Task Add(TEntity entity)
        {
            // First validate entity's rules
            await entity.Validate();

            await _dbSet.AddAsync(entity);
        }

        public Task Update(TEntity entity)
        {
            // First validate entity's rules
            entity.Validate();

            _dbContext.Entry(entity).State = EntityState.Modified;

            return Task.CompletedTask;
        }

        public Task Delete(TEntity entity)
        {
            // First validate entity's rules
            entity.Validate();

            _dbContext.Entry(entity).State = EntityState.Deleted;

            return Task.CompletedTask;
        }

        public Task DeleteWhere(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> entities = _dbSet.Where(predicate);

            foreach (var entity in entities) _dbContext.Entry(entity).State = EntityState.Deleted;

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        #endregion
    }
    
    /**
     * No one likes code duplication, but I can’t think of an elegant solution to use
     * C# 8.0 default interface implementation for a trait-like pattern to avoid code
     * duplication in this case (as I did for DbContext objects).
     * TODO: Find a way to avoid code duplication here
     */
    
    public abstract class RepositoryWrite<TDbContext, TApplicationUser, TApplicationRole, TEntity, TPrimaryKey>
        : IRepositoryWrite<TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
        where TApplicationUser : ApplicationUser
        where TApplicationRole : ApplicationRole
        where TDbContext : AuthorizationDbContextWrite<TDbContext, TApplicationUser, TApplicationRole>
    {
        private readonly TDbContext _dbContext;
    
        private readonly DbSet<TEntity> _dbSet;
    
        protected RepositoryWrite(TDbContext context)
        {
            _dbContext = context;
            _dbSet     = _dbContext.Set<TEntity>();
        }
    
        #region IRepositoryWrite<TEntity,TPrimaryKey> Members
    
        public async Task Add(TEntity entity)
        {
            // First validate entity's rules
            await entity.Validate();
    
            await _dbSet.AddAsync(entity);
        }
    
        public Task Update(TEntity entity)
        {
            // First validate entity's rules
            entity.Validate();
    
            _dbContext.Entry(entity).State = EntityState.Modified;
    
            return Task.CompletedTask;
        }
    
        public Task Delete(TEntity entity)
        {
            // First validate entity's rules
            entity.Validate();
    
            _dbContext.Entry(entity).State = EntityState.Deleted;
    
            return Task.CompletedTask;
        }
    
        public Task DeleteWhere(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> entities = _dbSet.Where(predicate);
    
            foreach (var entity in entities) _dbContext.Entry(entity).State = EntityState.Deleted;
    
            return Task.CompletedTask;
        }
    
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    
        #endregion
    }
}