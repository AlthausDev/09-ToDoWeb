using Microsoft.AspNetCore.Mvc;
using TODO_V2.Server.Models;
using TODO_V2.Shared;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Repository.Interfaces
{
    public interface IUserRepository : IGenericRepository<User, UserCredentials>
    {
        Task<User?> GetByUserName(string username);
        Task<UserCredentials> GetUserCredentialsByUserName(string username);
    }
}
