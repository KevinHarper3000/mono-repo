using Microsoft.AspNetCore.Mvc;
using SvcCommon.Abstract;

namespace SvcCommon.Concrete;

[ApiController]
[Route("api/[controller]")]
public class GenericController<TModel> : Controller
{
    private readonly IRepository<TModel> repository;

    public GenericController(IRepository<TModel> repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public virtual async Task<ActionResult<TModel>> Get()
    {
        IEnumerable<TModel> response = await repository.GetAllAsync();
        return Ok(response);
    }

    [HttpGet("{id:Guid}")]
    public virtual async Task<ActionResult<TModel>> Get(Guid id)
    {
        TModel response = await repository.GetByIdAsync(id);
        return Ok(response);
    }

    [HttpPost]
    public virtual async Task<ActionResult<TModel>> Post([FromBody] TModel model)
    {
        TModel response = await repository.AddAsync(model);
        return Ok(response);
    }

    [HttpPut]
    public virtual async Task<ActionResult<TModel>> Put([FromBody] TModel model)
    {
        TModel response = await repository.UpdateAsync(model);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public virtual async Task<ActionResult> Delete(Guid id)
    {
        await repository.DeleteAsync(id);
        return NoContent();
    }

    [HttpDelete]
    public virtual async Task<ActionResult> Delete([FromBody] TModel model)
    {
        await repository.DeleteAsync(model);
        return NoContent();
    }
}