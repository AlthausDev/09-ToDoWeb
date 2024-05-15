﻿using TODO_V2.Shared;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Services.Interfaces
{
    public interface IUserService : IGenericService<User>
    {
        Task<T> Update<T>(T entity);
    }
}
