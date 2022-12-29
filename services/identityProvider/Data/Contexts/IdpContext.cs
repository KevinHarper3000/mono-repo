using System.Diagnostics.CodeAnalysis;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SvcCommon.Abstract;

namespace Data.Contexts;

public class IdpContext : DbContext, IBaseContext
{
    public DbSet<UserClaim> UserClaims => Set<UserClaim>();

    public DbSet<User> Users => Set<User>();


    public Task<int> SaveChangesAsync(CancellationToken? cancellationToken)
    {
        return base.SaveChangesAsync(CancellationToken.None);
    }

    public override EntityEntry<TEntity> Entry<TEntity>([NotNull] TEntity entity)
        where TEntity : class
    {
        return base.Entry(entity);
    }
}