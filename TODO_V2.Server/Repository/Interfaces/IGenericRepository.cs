using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        Task<bool> Add(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(int entityId);
        Task<bool> LogicDelete(int entityId);
        Task<IEnumerable<T>> GetAll(GetRequest<T>? request);
        Task<IEnumerable<T>> GetAllLogic(GetRequest<T>? request);
        Task<T?> GetById(int entityId);
        Task<int> Count();
    }
}
