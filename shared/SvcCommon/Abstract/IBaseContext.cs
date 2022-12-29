using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SvcCommon.Abstract;

public interface IBaseContext
{
    EntityEntry<TEntity> Entry<TEntity>([NotNull] TEntity entity) where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken? cancellationToken);

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}