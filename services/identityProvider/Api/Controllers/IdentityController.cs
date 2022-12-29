using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using SvcCommon.Abstract;
using SvcCommon.Concrete;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class IdentityController : GenericController<User>
{
    public IdentityController(IRepository<User> repository) : base(repository)
    {
    }
}