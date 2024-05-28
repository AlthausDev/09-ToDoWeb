using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TODO_V2.Client.DTO;
using TODO_V2.Server.Models;
using TODO_V2.Shared;
using TODO_V2.Shared.Models;
using TODO_V2.Shared.Utils;

namespace TODO_V2.Server.Services.Interfaces
{
    public interface IUserService : IGenericService<User, LoginCredentials>
    {
        Task<User?> GetByUserName(string username);
        Task<ActionResult<LoginResponse>> Login(LoginCredentials credentials);
        Task<bool> Logout();
        Task<LoginCredentials> GetCredentialsByUserId(int userId);

    }
}
