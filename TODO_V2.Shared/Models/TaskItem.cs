using System;
using TODO_V2.Shared.Models.Enum;

namespace TODO_V2.Shared.Models
{
    public class TaskItem : BaseModel
    {
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public TaskStateEnum State { get; set; }
        public DateOnly? ExpirationDate { get; set; }

        public TaskItem()
        {
        }

        public TaskItem(int categoryId, int userId, TaskStateEnum state, string name, DateOnly expirationDate)
        {
            CategoryId = categoryId;
            UserId = userId;
            State = state;
            Name = name;
            ExpirationDate = expirationDate;
        }
    }
}
