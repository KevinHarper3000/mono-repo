namespace SvcCommon.Abstract;

public interface IRepository<TModel>
{
    Task<TModel> AddAsync(TModel model);
    Task DeleteAsync(TModel model);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<TModel>> GetAllAsync();
    Task<TModel> GetByIdAsync(Guid id);
    Task<TModel> GetByIndexAsync(int id);
    Task<TModel> UpdateAsync(TModel model);
}