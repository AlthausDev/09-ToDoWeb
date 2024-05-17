﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TODO_V2.Shared;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Services.Interfaces
{
    public interface IUserService : IGenericService<User>
    {
        Task<User?> GetByUserName(string username);
        Task<ActionResult<User>> Login(string username, string password);
        Task<bool> Logout();
    }
}
