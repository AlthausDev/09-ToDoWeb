using Microsoft.AspNetCore.Mvc;
using TODO_V2.Client.ClientModels;
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
