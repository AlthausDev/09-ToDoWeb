using System;
using TODO_V2.Shared.Models.Enum;

namespace TODO_V2.Shared.Models
{
    public class TaskItem : BaseModel
    {
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public string State { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public TaskItem()
        {
        }

        public TaskItem(int categoryId, int userId, string state, string name, DateTime expirationDate)
        {
            CategoryId = categoryId;
            UserId = userId;
            State = state;
            Name = name;
            ExpirationDate = expirationDate;
        }
    }
}
