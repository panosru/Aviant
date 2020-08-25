namespace Aviant.DDD.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class Entity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; protected set; }// = default!;
        
        protected Entity()
        {}

        protected Entity(TKey id) => Id = id;

        #region Equality Check
        
        public override bool Equals(object? obj)
        {
            return obj is Entity<TKey> entity && GetType() == entity.GetType() &&
                   EqualityComparer<TKey>.Default.Equals(Id, entity.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GetType(), Id);
        }

        public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
        {
            return EqualityComparer<Entity<TKey>>.Default.Equals(left, right);
        }

        public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
        {
            return !(left == right);
        }
        
        #endregion

        public virtual Task<bool> Validate()
        {
            return Task.FromResult(true);
        }
    }
}