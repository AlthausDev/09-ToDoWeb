using Microsoft.AspNetCore.Mvc;
using TODO_V2.Shared;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Services.Interfaces
{
    public interface IGenericService<T> where T : BaseModel
    {
        //Task<T> Add(T entity);
        Task<bool> Add(T entity);
        Task<T> Update(T entity);
        void Delete(int entityId);
        void LogicDelete(int entityId);
        Task<IEnumerable<T>> GetAll(GetRequest<T>? request);
        Task<IEnumerable<T>> GetAllLogic(GetRequest<T>? request);
        T GetById(int entityId);
        ActionResult<int> Count();

    }
}
