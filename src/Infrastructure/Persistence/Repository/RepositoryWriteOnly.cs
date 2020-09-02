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

    public abstract class RepositoryWriteOnly<TDbContext, TApplicationUser, TApplicationRole, TEntity, TPrimaryKey>
        : IRepositoryWrite<TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
        where TApplicationUser : ApplicationUser
        where TApplicationRole : ApplicationRole
        where TDbContext : ApplicationDbContext<TDbContext, TApplicationUser, TApplicationRole>
    {
        private readonly TDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        protected RepositoryWriteOnly(TDbContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<TEntity>();
        }

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
    }
}