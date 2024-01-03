using Aviant.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aviant.Infrastructure.Persistence.Configurations;

public class EntityConfiguration<TEntity, T> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IEntity<T>
{
    #region IEntityTypeConfiguration<TEntity> Members

    public virtual void Configure(EntityTypeBuilder<TEntity> builder) =>
        builder.HasKey(e => e.Id);

    #endregion
}
