using TODO_V2.Shared.Models;

//https://robdotnet.medium.com/effortlessly-create-crud-apis-in-c-bcc759691a91

namespace TODO_V2.Server.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : BaseModel
    {     
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        void Delete(int entityId);
        void LogicDelete(int entityId);
        Task<IEnumerable<T>> GetAll(GetRequest<T>? request);
        Task<IEnumerable<T>> GetAllLogic(GetRequest<T>? request);
        Task<T>? GetById(int entityId);
    }
}
