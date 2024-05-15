using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Controllers.Interfaces
{
    public interface IGenericController <T> where T: BaseModel
    {
        //public Task<IEnumerable<T>>? GetAll(GetRequest<T>? request);
        //public T Get(int id);
        //public Task<T> Post(T entity);
        //public Task<T>? Put(T entity);
        //public void Delete(int id);

    }
}
