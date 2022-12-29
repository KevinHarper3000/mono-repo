using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SvcCommon.Abstract;

namespace SvcCommon.Concrete;

public class BaseRepository<TModel, TEntity> : IRepository<TModel> where TModel : class where TEntity : class
{
    private const string InvalidOperationExceptionMessage = "The item could not be found.";
    private readonly IBaseContext context;
    private readonly IMapper mapper;

    public BaseRepository(IMapper mapper, IBaseContext context)
    {
        this.mapper = mapper;
        this.context = context;
    }

    public virtual async Task<TModel> AddAsync(TModel model)
    {
        TEntity? entity = mapper.Map<TModel, TEntity>(model);

        EntityEntry<TEntity> created = await context.Set<TEntity>().AddAsync(entity);
        await context.SaveChangesAsync(null);

        return MapToModel(created.Entity);
    }

    public virtual async Task DeleteAsync(TModel model)
    {
        TEntity? entity = mapper.Map<TModel, TEntity>(model);
        context.Set<TEntity>().Remove(entity);
        await context.SaveChangesAsync(null);
    }

    public virtual async Task DeleteAsync(int id)
    {
        TEntity? toDelete = await context.Set<TEntity>().FindAsync(id);

        TEntity deleted = context.Set<TEntity>()
            .Remove(toDelete ??
                    throw new InvalidOperationException(InvalidOperationExceptionMessage)).Entity;

        await context.SaveChangesAsync(null);
    }

    public virtual async Task<IEnumerable<TModel>> GetAllAsync()
    {
        List<TEntity> entities = await context.Set<TEntity>().ToListAsync();

        return entities.Select(entity => mapper.Map<TEntity, TModel>(entity));
    }

    public virtual async Task<TModel> GetByIdAsync(Guid id)
    {
        TEntity entity = await context.Set<TEntity>().FindAsync(id) ??
                         throw new InvalidOperationException(InvalidOperationExceptionMessage);
        return MapToModel(entity);
    }

    public virtual async Task<TModel> GetByIdAsync(int id)
    {
        TEntity entity = await context.Set<TEntity>().FindAsync(id) ??
                         throw new InvalidOperationException(InvalidOperationExceptionMessage);
        return MapToModel(entity);
    }

    public virtual async Task<TModel> UpdateAsync(TModel model)
    {
        TEntity? entity = mapper.Map<TModel, TEntity>(model);
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync(null);

        return MapToModel(entity);
    }

    private TModel MapToModel(TEntity entity)
    {
        TModel? updatedModel = mapper.Map<TEntity, TModel>(entity);

        // No-op return statement for debugging.
        return updatedModel;
    }
}