using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Services.Interfaces
{
    public interface ITaskItemService : IGenericService<TaskItem, object>
    {
        Task<IEnumerable<TaskItem>> GetTasksByUserId(int userId);
    }
}
