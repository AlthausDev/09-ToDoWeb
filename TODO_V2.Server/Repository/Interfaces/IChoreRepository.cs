using TODO_V2.Shared;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Repository.Interfaces
{
    public interface IChoreRepository : IGenericRepository<Chore>
    {
        IEnumerable<Chore> GetAllAdmin();
    }
}
