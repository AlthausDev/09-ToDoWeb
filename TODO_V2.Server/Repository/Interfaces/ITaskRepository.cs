using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Repository.Interfaces
{
    public interface ITaskRepository : IGenericRepository<TaskItem, object>
    {
        Task<IEnumerable<TaskItem>> GetTasksByUserId(int userId);
        Task<IEnumerable<TaskItem>> GetTasksByCategoryId(int categoryId);
    }
}
