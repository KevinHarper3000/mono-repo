using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using SvcCommon.Abstract;
using SvcCommon.Concrete;

namespace Api.Controllers;

[Produces("application/json")]
public class UsersController : GenericController<UserViewModel>
{
    public UsersController(IRepository<UserViewModel> repository) : base(repository)
    {
    }
}