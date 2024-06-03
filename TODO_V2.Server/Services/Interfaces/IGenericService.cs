using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Services.Interfaces
{
    public interface IGenericService<T, Y> where T : BaseModel
    {
        Task<bool> Add(T entity, Y? secondEntity);
        Task<T> Update(T entity, Y? secondEntity);
        void Delete(int entityId);
        void LogicDelete(int entityId);
        Task<IEnumerable<T>> GetAll(GetRequest<T> request);
        Task<IEnumerable<T>> GetAllLogic(GetRequest<T> request);
        Task<T> GetById(int entityId);
        Task<int> Count();
    }
}
