using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using SvcCommon.Abstract;
using SvcCommon.Concrete;

namespace Api.Controllers
{
    [Produces("application/json")]
    public class UserClaimsController : GenericController<UserClaimViewModel>
    {
        public UserClaimsController(IRepository<UserClaimViewModel> repository) : base(repository)
        {
        }
    }
}
