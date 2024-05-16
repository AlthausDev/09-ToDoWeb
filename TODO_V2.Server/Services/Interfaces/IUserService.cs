using Microsoft.AspNetCore.Mvc;
using TODO_V2.Shared;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Services.Interfaces
{
    public interface IUserService : IGenericService<User>
    {
        User GetByUserName(string Username);
        ActionResult<User?> Login(string username, string password);
    }
}
