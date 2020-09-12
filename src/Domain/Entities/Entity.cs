namespace Aviant.DDD.Domain.Entities
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    #endregion

    public abstract class Entity<TKey> : IEntity<TKey>
    {
        protected Entity()
        { }

        protected Entity(TKey id) => Id = id;

        #region IEntity<TKey> Members

        public TKey Id { get; set; }

        public virtual Task<bool> Validate() => Task.FromResult(true);

        #endregion

        #region Equality Check

        public override bool Equals(object? obj) => obj is Entity<TKey> entity
                                                 && GetType() == entity.GetType()
                                                 && EqualityComparer<TKey>.Default.Equals(Id, entity.Id);

        public override int GetHashCode() => HashCode.Combine(GetType(), Id);

        public static bool operator ==(Entity<TKey> left, Entity<TKey> right) =>
            EqualityComparer<Entity<TKey>>.Default.Equals(left, right);

        public static bool operator !=(Entity<TKey> left, Entity<TKey> right) => !(left == right);

        #endregion
    }
}