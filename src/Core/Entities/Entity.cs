namespace Aviant.DDD.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class Entity<TKey> : IEntity<TKey>
    {
        #pragma warning disable 8618
        protected Entity()
        { }
        #pragma warning restore 8618

        protected Entity(TKey id) => Id = id;

        #region IEntity<TKey> Members

        public TKey Id { get; set; }

        public bool IsTransient()
        {
            if (EqualityComparer<TKey>.Default.Equals(Id, default))
                return true;

            // Workaround for EF Core since it sets int/long to min value when attaching to DbContext
            if (typeof(TKey) == typeof(int))
                return Convert.ToInt32(Id) <= 0;

            if (typeof(TKey) == typeof(long))
                return Convert.ToInt64(Id) <= 0;

            return false;
        }

        public virtual Task<bool> ValidateAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        #endregion

        #region Equality Check

        public override bool Equals(object? obj) => obj is Entity<TKey> entity
                                                 && GetType() == entity.GetType()
                                                 && EqualityComparer<TKey>.Default.Equals(Id, entity.Id);

        // ReSharper disable once NonReadonlyMemberInGetHashCode
        public override int GetHashCode() => HashCode.Combine(GetType(), Id);

        #endregion
    }
}